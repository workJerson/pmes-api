using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace pmes_api.Common.Contracts
{
    public interface IWritableController<T, O> where O : class
    {
        Task<IActionResult> Create([FromBody] T payload);
        Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<O> payload);
        Task<IActionResult> Put(Guid id, [FromBody] O payload);
        Task<IActionResult> Delete(Guid id);
    }
}
