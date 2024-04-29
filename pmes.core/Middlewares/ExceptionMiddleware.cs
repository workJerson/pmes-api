
using Serilog.Context;
using Serilog;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using pmes.core.Common.Models;
using pmes.core.Common.Exceptions;
using pmes.core.Common.Models.Response;
using Newtonsoft.Json.Serialization;

namespace pmes.core.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                string errorId = Guid.NewGuid().ToString();
                LogContext.PushProperty("ErrorId", errorId);
                LogContext.PushProperty("StackTrace", exception.StackTrace);
                var errorResult = new ErrorResponseModel
                {
                    Source = exception.TargetSite?.DeclaringType?.FullName,
                    Exception = exception.Message.Trim(),
                    ErrorId = errorId,
                    SupportMessage = string.Format("Provide the ErrorId {0} to the support team for further analysis.", errorId)
                };
                errorResult.Messages.Add(exception.Message);
                if (exception is not CustomException && exception.InnerException != null)
                {
                    while (exception.InnerException != null)
                    {
                        exception = exception.InnerException;
                    }
                }

                switch (exception)
                {
                    case CustomException e:
                        errorResult.StatusCode = (int)e.StatusCode;
                        if (e.ErrorMessages is not null)
                        {
                            errorResult.Messages = e.ErrorMessages;
                        }

                        if (e.Details is not null)
                        {
                            errorResult.Details = e.Details;
                        }

                        break;

                    case KeyNotFoundException:
                        errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                Log.Error($"{errorResult.Exception} Request failed with Status Code {context.Response.StatusCode} and Error Id {errorId}.");
                var response = context.Response;
                if (!response.HasStarted)
                {
                    response.ContentType = "application/json";
                    response.StatusCode = errorResult.StatusCode;
                    errorResult.Source = "";
                    await response.WriteAsync(JsonConvert.SerializeObject(errorResult, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver()}));
                }
                else
                {
                    Log.Warning("Can't write error response. Response has already started.");
                }
            }
        }
    }
}
