using Microsoft.AspNetCore.Mvc;
using OrderApplication.Core.Model.Util.Response;

namespace OrderApplication.Core.Api.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
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

        protected virtual IActionResult ApiDocumentResponse(DataResponse result = null)
        {
            if (result.IsSuccessful)
            {
                return Ok(new SuccessDataResponse(result.Document));
            }
            else
            {
                if (result.HttpStatusCode == System.Net.HttpStatusCode.BadRequest)
                    return BadRequest(new ErrorDataResonse(result.ErrorMessageList, result.ErrorCode, System.Net.HttpStatusCode.BadRequest));

                return NoContent();
            }
        }
    }
}
