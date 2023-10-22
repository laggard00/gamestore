using System.Net;
using Serilog;

namespace GameStore_v2.Middleware
{
    public class GlobalExceptionMiddleware
    {
        
            private readonly RequestDelegate _next;

        //when I put it in constructor it throws error System is unable to resolve serivce for type System.String,
        //I Don't know how to fix it so I'll leave it hardcoded here.
            private readonly string _filePath="logs/globalexceptions-.txt";

            private readonly Serilog.ILogger _logger;

            public GlobalExceptionMiddleware( RequestDelegate next)
            {
                _next = next;

                
               _logger = new LoggerConfiguration().WriteTo.File(_filePath, rollingInterval:RollingInterval.Month).CreateLogger();

            }
            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context);

                }
                catch (Exception ex)
                {

                    _logger.Error( $"{DateTime.UtcNow}:" +
                                         $"{Environment.NewLine}" +
                                         $"{Environment.NewLine}" +
                                         $"{ex}" +
                                         $"{Environment.NewLine}" +
                                         $"{Environment.NewLine}" +
                                         $"{ex.Message}" +
                                         $"{Environment.NewLine}" +
                                         $"{Environment.NewLine}" +
                                         $"{Environment.NewLine}");

                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

                }
            }
        
    }
}
