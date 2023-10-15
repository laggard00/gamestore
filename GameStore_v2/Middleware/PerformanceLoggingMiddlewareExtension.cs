namespace GameStore_v2.Middleware
{
    public static class PerformanceLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UsePerformanceLogging(this IApplicationBuilder builder, string filePath)
        {
            return builder.UseMiddleware<PerformanceLoggingMiddleware>(filePath);
        }
    }
}
