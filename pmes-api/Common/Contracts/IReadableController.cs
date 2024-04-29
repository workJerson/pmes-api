using Microsoft.AspNetCore.Mvc;
using pmes.core.Common.Models.Request;

namespace pmes_api.Common.Contracts
{
    public interface IReadableController
    {
        Task<IActionResult> Index([FromBody] BaseFilteringViewModel filters);
        Task<FileResult> Export([FromBody] BaseExportFilteringViewModel filters);
        Task<IActionResult> Show(Guid id);
    }
}
