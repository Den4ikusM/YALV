using System;

namespace YALV.Core
{
    struct LogEntry
    {
        public string Level { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Application { get; set; }
        public string Host { get; set; }
        public string Thread { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
