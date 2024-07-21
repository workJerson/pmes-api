using Microsoft.AspNetCore.JsonPatch;
using pmes.core.Common.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Common.Contracts
{
    public interface IWritableCacheManager<TResponse, TPayload> where TPayload : class
    {
        Task<GenericResponse<TResponse>> Update(Guid id, TPayload payload);
        Task<GenericResponse<TResponse>> Update(Guid id, JsonPatchDocument<TPayload> patchDocument);
    }
}
