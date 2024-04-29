using Microsoft.AspNetCore.Mvc;

namespace pmes_api.Common.Contracts
{
    public interface IAuditableController<T>
    {
        Task<IActionResult> AuditLogs([FromBody] T filters);
    }
}
