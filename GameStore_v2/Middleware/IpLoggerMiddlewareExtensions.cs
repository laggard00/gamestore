namespace GameStore_v2.Middleware
{
    public static class IpLoggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseIpLogger(this IApplicationBuilder builder, string filePath, bool enablecustomerlogger)
        {
            return builder.UseMiddleware<IpLoggerMiddleware>(filePath, enablecustomerlogger);
        }
    }
}
