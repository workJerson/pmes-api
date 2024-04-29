using System.Net;

namespace pmes.core.Common.Exceptions;

public class NotFoundException : CustomException
{
    public NotFoundException(string message)
        : base(message, null, null, HttpStatusCode.NotFound)
    {
    }
}