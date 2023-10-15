namespace GameStore_v2.Middleware
{
    public class IpLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _filePath;

        public CancellationToken CancellationToken { get; private set; }

        public IpLoggerMiddleware(RequestDelegate next, string filePath)
        {
            _next = next;
            _filePath = filePath;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string ipAddress = context.Connection.RemoteIpAddress?.ToString();
            if (!string.IsNullOrWhiteSpace(ipAddress))
            {
                await File.AppendAllTextAsync(_filePath, $"{DateTime.UtcNow}: {ipAddress}{Environment.NewLine}");
            }

            await _next(context);
        }
    }
}
