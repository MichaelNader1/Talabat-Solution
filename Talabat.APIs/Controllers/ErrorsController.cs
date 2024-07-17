using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
    [ApiController]
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController :ControllerBase
    {
        public ActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code, "EndPoint is not found."));
        }
    }
}
