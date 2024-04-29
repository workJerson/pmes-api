using pmes.core.Common.Models.Response;
using System.Net;

namespace pmes.core.Common.Exceptions
{
    public class CustomException : Exception
    {
        public List<string>? ErrorMessages { get; }
        public HttpStatusCode StatusCode { get; }
        public List<ErrorDetailModel>? Details { get; }

        public CustomException(string message, List<string>? errors = default, List<ErrorDetailModel>? details = default, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
            : base(message)
        {
            ErrorMessages = errors;
            StatusCode = statusCode;
            Details = details;
        }
    }
}
