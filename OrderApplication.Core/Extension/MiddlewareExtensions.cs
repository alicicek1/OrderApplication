using Microsoft.AspNetCore.Builder;

namespace OrderApplication.Core.Extension
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthMiddleware<T>(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<T>();
        }
        public static IApplicationBuilder UseExceptionMiddleware<T>(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<T>();
        }

    }
}
