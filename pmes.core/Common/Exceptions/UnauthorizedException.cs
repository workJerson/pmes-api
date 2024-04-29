using System.Net;

namespace pmes.core.Common.Exceptions;

public class UnauthorizedException : CustomException
{
    public UnauthorizedException(string message)
       : base(message, null, null, HttpStatusCode.Unauthorized)
    {
    }
}