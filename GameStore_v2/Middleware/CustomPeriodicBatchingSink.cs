using Serilog.Events;
using Serilog.Sinks.PeriodicBatching;

namespace GameStore_v2.Middleware
{
    public class CustomPeriodicBatchingSink : PeriodicBatchingSink
    {
        private readonly string _filePath;

        public CustomPeriodicBatchingSink(string filePath, int batchSizeLimit, TimeSpan period)
            : base(batchSizeLimit, period)
        {
            _filePath = filePath;
            //if(!File.Exists("../logs")) { File.Create("../logs"); }
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            using var fileWriter = File.AppendText(_filePath);
            foreach (var logEvent in events)
            {
                var logMessage = logEvent.RenderMessage();
                await fileWriter.WriteLineAsync(logMessage);
            }
        }
    }
}
