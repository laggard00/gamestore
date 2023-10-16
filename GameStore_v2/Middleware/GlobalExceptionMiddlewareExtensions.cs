namespace GameStore_v2.Middleware
{
    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
