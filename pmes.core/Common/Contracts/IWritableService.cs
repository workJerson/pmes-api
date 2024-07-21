using Microsoft.AspNetCore.JsonPatch;
using pmes.core.Common.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Common.Contracts
{
    public interface IWritableService<TResponse, TCreate, TUpdate> where TUpdate : class
    {
        Task<GenericResponse<TResponse>> Create(TCreate payload);
        Task<GenericResponse<IEnumerable<TResponse>>> Create(IEnumerable<TCreate> payload);
        Task<GenericResponse<TResponse>> Update(Guid id, TUpdate payload);
        Task<GenericResponse<TResponse>> Update(Guid id, JsonPatchDocument<TUpdate> patchDocument);
        Task<GenericResponse<TResponse>> Delete(Guid id);
    }
}
