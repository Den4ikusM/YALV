using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using YALV.Core.Domain;

namespace YALV.Core.Providers
{
    

    class UdpEntriesProvider : AbstractEntriesProvider
    {
        class UdpEndpoint : IDisposable
        {
            private readonly UdpClient udpClient;

            public UdpEndpoint(Uri uri)
            {
                var address = Dns.GetHostAddresses(uri.Host).Where(x => x.AddressFamily == AddressFamily.InterNetwork).ToArray();
                var ep = new IPEndPoint(address[0], uri.Port);
                udpClient = new UdpClient(ep);
            }

            public void Listen(Action<LogEntry> entryCallback)
            {

                void ProcessUdpReceiveResult(UdpReceiveResult result)
                {
                    try {
                        var entry = JsonSerializer.Deserialize<LogEntry>(result.Buffer);
                        entryCallback(entry);
                    }
                    catch (Exception) {
                    }
                }

                async Task<(UdpReceiveResult udpResult, Exception exception)> ReceiveAsync()
                {
                    try {
                        var result = await udpClient.ReceiveAsync();
                        return (result, null);
                    }
                    catch (Exception ex) when (ex is ObjectDisposedException || ex is SocketException) {
                        return (default, ex);
                    }
                }

                Observable.FromAsync(ReceiveAsync).Repeat()
                    .TakeWhile(result => !(result.exception is ObjectDisposedException))
                    .Subscribe(result => ProcessUdpReceiveResult(result.udpResult));
            }

            public void Dispose()
            {
                udpClient.Dispose();
            }
        }

        private UdpEndpoint udpEndpoint;
        private List<LogItem> logItems = new List<LogItem>(1024);

        public override IEnumerable<LogItem> GetEntries(string dataSource, FilterParams filter)
        {
            if (udpEndpoint == null) {
                try {
                    var endpoint = new UdpEndpoint(new Uri(dataSource));
                    endpoint.Listen(entry => AddLogEntry(ref entry));
                    udpEndpoint = endpoint;
                }
                catch (Exception ex) {
                }
            }
            HasChanges = false;
            return logItems;
        }

        private void AddLogEntry(ref LogEntry logEntry)
        {
            var item = new LogItem() {
                Id = logItems.Count,
                Level = logEntry.Level,
                TimeStamp = logEntry.TimeStamp,
                Thread = logEntry.Thread,
                Logger = logEntry.Logger,
                App = logEntry.Application,
                HostName = logEntry.Host,
                Message = logEntry.Message,
                Throwable = logEntry.Exception,
            };
            logItems.Add(item);
            HasChanges = true;
        }
    }
}
