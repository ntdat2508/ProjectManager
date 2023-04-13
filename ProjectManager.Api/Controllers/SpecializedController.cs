using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Entity;
using ProjectManager.Services.Api;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Model.Request;
using Serilog;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace ProjectManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializedController : ControllerBase
    {
        private readonly ISpecializedService _specializedService;

        public SpecializedController(ISpecializedService specializedService)
        {
            _specializedService = specializedService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PagingRequest request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                var result = await _specializedService.GetAllAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SpecializedController GetAllAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync([FromBody] Specialized request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                return Ok(await _specializedService.SaveAsync(request));
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SpecializedController SaveAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                return Ok(await _specializedService.DeleteAsync(request.Id, request.UserName));
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SpecializedController DeleteAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpGet("GetAllSpecialized")]
        public IActionResult GetAllSpecializedAsync()
        {
            try
            {
                var result = _specializedService.GetAllSpecializedAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SpecializedController GetAllSpecializedAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }
    }
}
