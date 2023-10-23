using Serilog;
using System.Net;

namespace GameStore_v2.Middleware
{
    public class IpLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        

        private readonly Serilog.ILogger _logger;

        private readonly bool _enabled;



        public IpLoggerMiddleware(RequestDelegate next, string filePath, bool enablecustomlogger)
        {
            _next = next;
            _enabled = enablecustomlogger;
            if (enablecustomlogger){ 
            _logger = new LoggerConfiguration().
                                    MinimumLevel
                                    .Debug()
                                    .WriteTo.Async(a => a.Sink(new CustomPeriodicBatchingSink(filePath, 10, TimeSpan.FromSeconds(60))))

                                    .CreateLogger();
        }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_enabled)
            {
                string ipAddress = context.Request.Headers["X-Fowarded-For"].FirstOrDefault();


                if (string.IsNullOrWhiteSpace(ipAddress))

                {

                    ipAddress = context.Connection.RemoteIpAddress?.ToString();

                }
                try
                {
                    _logger.Information($"{ipAddress}----{DateTime.UtcNow}");

                    await _next(context);

                }
                catch (Exception ex)
                {
                    await _next(context);
                }
            }
            else { await _next(context); }

            
        }
    }
}
