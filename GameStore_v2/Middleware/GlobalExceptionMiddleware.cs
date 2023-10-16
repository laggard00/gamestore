
namespace GameStore_v2.Middleware
{
    public class GlobalExceptionMiddleware
    {
        
            private readonly RequestDelegate _next;

            private readonly string _filePath="globallogs.txt";

            public GlobalExceptionMiddleware( RequestDelegate next)
            {
                _next = next;
               

            }
            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context);

                }
                catch (Exception ex)
                {

                    await File.AppendAllTextAsync(_filePath, $"{DateTime.UtcNow}:" +
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
