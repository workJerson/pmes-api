using System.Net;

namespace pmes.core.Common.Exceptions;

public class ForbiddenException : CustomException
{
    public ForbiddenException(string message)
        : base(message, null, null, HttpStatusCode.Forbidden)
    {
    }
}