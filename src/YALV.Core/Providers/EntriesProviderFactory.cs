using System;
using YALV.Core.Domain;

namespace YALV.Core.Providers
{
    public static class EntriesProviderFactory
    {
        private static UdpEntriesProvider udpEntriesProvider;

        public static AbstractEntriesProvider GetProvider(string dataSource, EntriesProviderType type = EntriesProviderType.Xml)
        {
            if (Uri.TryCreate(dataSource, UriKind.Absolute, out var uri) && uri.Scheme.Equals("udp", StringComparison.OrdinalIgnoreCase)) {
                return udpEntriesProvider ?? (udpEntriesProvider = new UdpEntriesProvider());
            }
            switch (type)
            {
                case EntriesProviderType.Xml:
                    return new XmlEntriesProvider();

                case EntriesProviderType.Sqlite:
                    return new SqliteEntriesProvider();

                case EntriesProviderType.MsSqlServer:
                    return new MsSqlServerEntriesProvider();

                default:
                    var message = String.Format((string) "Type {0} not supported", (object) type);
                    throw new NotImplementedException(message);
            }
        }
    }
}