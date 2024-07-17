using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : BaseApiController
    {
        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            return NotFound(new ApiResponse(404));
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("servererror")]
        public IActionResult GetServerError()
        {
            return StatusCode(500, "Internal server error.");
        }

        [HttpGet("validationerror")]
        public IActionResult GetValidationError()
        {
            ModelState.AddModelError("Field", "Validation error message.");
            return BadRequest(ModelState);
        }

        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized(new ApiResponse(401));
        }
    }
}
