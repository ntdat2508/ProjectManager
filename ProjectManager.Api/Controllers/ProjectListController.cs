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
    public class ProjectListController : ControllerBase
    {
        private readonly IProjectListService _projectListService;

        public ProjectListController(IProjectListService projectListService)
        {
            _projectListService = projectListService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] ProjectListRequest request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                var result = await _projectListService.GetAllAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " ProjectListController GetAllAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync([FromBody] ProjectList request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                return Ok(await _projectListService.SaveAsync(request));
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " ProjectListController SaveAsync: " + ex.ToString());
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
                return Ok(await _projectListService.DeleteAsync(request.Id, request.UserName));
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " ProjectListController DeleteAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpPost("Mark")]
        public async Task<IActionResult> MarkAsync([FromBody] ProjectList request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                return Ok(await _projectListService.MarkAsync(request));
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " ProjectListController MarkAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpGet("GetAllProjectList")]
        public IActionResult GetAllProjectListAsync()
        {
            try
            {
                var result = _projectListService.GetAllProjectListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " ProjectListController GetAllProjectListAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }
    }
}
