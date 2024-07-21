using pmes.core.Common.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Common.Contracts
{
    public interface IReadableCacheManager<TResponse> where TResponse : class
    {
        public Task<GenericResponse<TResponse>> Show(Guid id);
    }
}
