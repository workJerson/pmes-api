using System.Net;

namespace pmes.core.Common.Exceptions;

public class InternalServerException : CustomException
{
    public InternalServerException(string message, List<string>? errors = default)
        : base(message, errors, null, HttpStatusCode.InternalServerError)
    {
    }
}