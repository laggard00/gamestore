using GameStore_v2.Middleware;

namespace GameStore.WEB.Middleware.Exstensions
{
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {

            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
