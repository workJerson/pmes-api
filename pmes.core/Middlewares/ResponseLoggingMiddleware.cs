
using Microsoft.AspNetCore.Http;
using Serilog.Context;
using Serilog;

namespace pmes.core.Middlewares
{
    public class ResponseLoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            await next(httpContext);
            var originalBody = httpContext.Response.Body;
            using var newBody = new MemoryStream();
            httpContext.Response.Body = newBody;
            newBody.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

            LogContext.PushProperty("StatusCode", httpContext.Response.StatusCode);
            LogContext.PushProperty("ResponseTimeUTC", DateTime.Now);
            Log.ForContext("ResponseHeaders", httpContext.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
           .ForContext("ResponseBody", responseBody)
           .Information("HTTP {RequestMethod} Request to {RequestPath} by {RequesterEmail} has Status Code {StatusCode}", httpContext.Request.Method, httpContext.Request.Path, "Anonymous", httpContext.Response.StatusCode);
            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originalBody);
        }
    }
}
