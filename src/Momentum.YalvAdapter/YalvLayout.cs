using System.IO;
using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;
using YALV.Core;

namespace Momentum.YalvAdapter
{
    internal class YalvLayout : LayoutSkeleton
    {
        public override void ActivateOptions()
        {
            IgnoresException = false;
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var host = log4net.GlobalContext.Properties["log4net:HostName"] as string;
            var instance = log4net.GlobalContext.Properties["ServiceUniqueName"] as string;
            var logEntry = new LogEntry() {
                Level = loggingEvent.Level.ToString(),
                TimeStamp = loggingEvent.TimeStamp,
                Application = instance ?? loggingEvent.Domain,
                Host = host,
                Thread = loggingEvent.ThreadName,
                Logger = loggingEvent.LoggerName,
                Message = loggingEvent.RenderedMessage,
                Exception = loggingEvent.GetExceptionString(),
            };
            var str = JsonConvert.SerializeObject(logEntry);
            writer.Write(str);
        }
    }
}
