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
    
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("GetStudentByClasss")]
        public async Task<IActionResult> GetStudentByClasssAsync([FromQuery] long classsId)
        {
            try
            {
                if (!ModelState.IsValid || 0 == classsId)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                var result = await _studentService.GetStudentByClasssAsync(classsId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentController GetStudentByClasssAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] StudentRequest request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                var result = await _studentService.GetAllAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentController GetAllAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync([FromBody] Student request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                return Ok(await _studentService.SaveAsync(request));
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentController SaveAsync: " + ex.ToString());
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
                return Ok(await _studentService.DeleteAsync(request.Id, request.UserName));
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentController DeleteAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpGet("GetAllStudent")]
        public IActionResult GetAllStudentAsync()
        {
            try
            {
                var result = _studentService.GetAllStudentAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentController GetAllStudentAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpGet("GetSelectAllByUsername")]
        public async Task<IActionResult> GetSelectAllByUsernameAsync(string username)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(username))
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                var result = await _studentService.GetSelectAllByUsernameAsync(username);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " StudentController GetSelectAllByUsernameAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }
    }
}
