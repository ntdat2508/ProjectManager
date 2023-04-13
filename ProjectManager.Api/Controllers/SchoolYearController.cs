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
    public class SchoolYearController : ControllerBase
    {
        private readonly ISchoolYearService _schoolYearService;

        public SchoolYearController(ISchoolYearService schoolYearService)
        {
            _schoolYearService = schoolYearService;
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
                var result = await _schoolYearService.GetAllAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SchoolYearController GetAllAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync([FromBody] SchoolYear request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                return Ok(await _schoolYearService.SaveAsync(request));
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SchoolYearController SaveAsync: " + ex.ToString());
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
                return Ok(await _schoolYearService.DeleteAsync(request.Id, request.UserName));
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SchoolYearController DeleteAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpGet("GetAllSchoolYear")]
        public IActionResult GetAllSchoolYearAsync()
        {
            try
            {
                var result = _schoolYearService.GetAllSchoolYearAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " SchoolYearController GetAllSchoolYearAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }
    }
}
