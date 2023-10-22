using Serilog;
using System.Diagnostics;

namespace GameStore_v2.Middleware
{
    public class PerformanceLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        
        private readonly Serilog.ILogger _logger;

        public PerformanceLoggingMiddleware(RequestDelegate next, string filePath)
        {
            _next = next;
            
            _logger = new LoggerConfiguration().WriteTo.File(filePath, rollingInterval: RollingInterval.Month).CreateLogger();


        }

        public async Task InvokeAsync(HttpContext context)
        {
            
            Stopwatch stopwatch = new();

            stopwatch.Start();

            await _next(context);

            stopwatch.Stop();

            _logger.Information($"[{DateTime.UtcNow}] Path: {context.Request.Path} | Time elapsed: {stopwatch.ElapsedMilliseconds}ms{Environment.NewLine} ");
            
            
        }
    }
}
