using pmes.core.Common.Models.Response;
using System.Net;

namespace pmes.core.Common.Exceptions
{
    public class ValidationException : CustomException
    {
        public ValidationException(string message, List<ErrorDetailModel>? details = null) : base(message, null, details, HttpStatusCode.BadRequest)
        {

        }
    }
}
