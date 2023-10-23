using Serilog;
using System.Diagnostics;

namespace GameStore_v2.Middleware
{
    public class PerformanceLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        
        private readonly Serilog.ILogger _logger;

        private readonly bool _enabled;

        public PerformanceLoggingMiddleware(RequestDelegate next, string filePath, bool enablecustomlogger)
        {
            _enabled = enablecustomlogger; 
            _next = next;
            if (enablecustomlogger)
            {
                _logger = new LoggerConfiguration()
                        .WriteTo.Async(a => a.Sink(new CustomPeriodicBatchingSink(filePath, 10, TimeSpan.FromSeconds(60))))
                        .CreateLogger();
            }


        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_enabled)
            {
                Stopwatch stopwatch = new();

                stopwatch.Start();

                await _next(context);

                stopwatch.Stop();

                _logger.Information($"[{DateTime.UtcNow}] Path: {context.Request.Path} | Time elapsed: {stopwatch.ElapsedMilliseconds}ms{Environment.NewLine} ");
            }
            else { await _next(context); }
            
        }
    }
}
