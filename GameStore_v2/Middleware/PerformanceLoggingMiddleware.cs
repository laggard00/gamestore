using System.Diagnostics;

namespace GameStore_v2.Middleware
{
    public class PerformanceLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _filePath;

        public PerformanceLoggingMiddleware(RequestDelegate next, string filePath)
        {
            _next = next;
            _filePath = filePath;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            await _next(context);

            stopwatch.Stop();

            string message = $"[{DateTime.UtcNow}] Path: {context.Request.Path} | Time elapsed: {stopwatch.ElapsedMilliseconds}ms{Environment.NewLine}";
            await File.AppendAllTextAsync(_filePath, message);
        }
    }
}
