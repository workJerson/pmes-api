using Microsoft.AspNetCore.Mvc;
using pmes.core.Common.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Common.Contracts
{
    public interface IReadableService<TResponse, TFilter, TExport>
    {
        Task<PagedResponse<List<TResponse>>> List(TFilter queryFilters);
        Task<byte[]> Export(TExport queryFilters);
        Task<GenericResponse<TResponse>> Show(int id);
        Task<GenericResponse<TResponse>> Show(Guid id);
    }
}
