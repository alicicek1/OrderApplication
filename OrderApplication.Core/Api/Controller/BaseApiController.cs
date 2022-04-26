using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OrderApplication.Core.Model.Util.AppSettings;
using OrderApplication.Core.Model.Util.Response;

namespace OrderApplication.Core.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private readonly IOptions<AppSetting> options;
        public BaseApiController(IOptions<AppSetting> options)
        {
            this.options = options;
        }
        protected virtual IActionResult ApiResponse<T>(List<T> data)
        {
            if (data != null && data.Count > 0)
                return Ok(data);
            else return NotFound(new ApiResponse { IsSuccessful = false, ErrorMessageList = new List<string> { "Not found." } });
        }

        protected virtual IActionResult ApiResponse<T>(T data)
        {
            if (data != null)
                return Ok(data);
            else return NotFound(new ApiResponse { IsSuccessful = false, ErrorMessageList = new List<string> { "Not found." } });
        }

        protected virtual IActionResult ApiResponse(DataResponse result = null)
        {
            if (result.IsSuccessful)
            {
                switch (result.HttpStatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return Ok(result.Document);
                        break;
                    case System.Net.HttpStatusCode.Created:
                        return Created(options.Value.Path, result.Document);
                        break;
                    case System.Net.HttpStatusCode.Accepted:
                        return Accepted(options.Value.Path, result.Document);
                        break;
                    case System.Net.HttpStatusCode.NoContent:
                        return NoContent();
                        break;
                }
            }
            else
            {
                switch (result.HttpStatusCode)
                {
                    case System.Net.HttpStatusCode.BadRequest:
                        return BadRequest(new ErrorDataResonse(result.ErrorMessageList, result.ErrorCode, System.Net.HttpStatusCode.BadRequest));
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        return Unauthorized(new { Message = "Unauthorized request" });
                        break;
                    case System.Net.HttpStatusCode.Forbidden:
                        return Forbid();
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        return NotFound();
                    default:
                        return BadRequest();
                }
            }

            return BadRequest();
        }
    }
}
