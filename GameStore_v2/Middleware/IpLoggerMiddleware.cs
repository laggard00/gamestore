using Serilog;
using System.Net;

namespace GameStore_v2.Middleware
{
    public class IpLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        

        private readonly Serilog.ILogger _logger;

        

        public IpLoggerMiddleware(RequestDelegate next, string filePath)
        {
            _next = next;
            
            _logger = new LoggerConfiguration().
                                    MinimumLevel
                                    .Debug()
                                    .WriteTo
                                    .File(filePath, rollingInterval: RollingInterval.Month)
                                    .WriteTo
                                    .Console()
                                    .CreateLogger();
        }

        public async Task InvokeAsync(HttpContext context)
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
            catch(Exception ex)
            {
                await _next(context);
            }

            
        }
    }
}
