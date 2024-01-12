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
            var logEntry = new LogEntry() {
                Level = loggingEvent.Level.ToString(),
                TimeStamp = loggingEvent.TimeStamp,
                Application = loggingEvent.Domain,
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
