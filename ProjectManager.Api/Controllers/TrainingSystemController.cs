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
    public class TrainingSystemController : ControllerBase
    {
        private readonly ITrainingSystemService _trainingSystemService;

        public TrainingSystemController(ITrainingSystemService trainingSystemService)
        {
            _trainingSystemService = trainingSystemService;
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
                var result = await _trainingSystemService.GetAllAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " TrainingSystemController GetAllAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync([FromBody] TrainingSystem request)
        {
            try
            {
                if (!ModelState.IsValid || null == request)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                return Ok(await _trainingSystemService.SaveAsync(request));
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " TrainingSystemController SaveAsync: " + ex.ToString());
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
                return Ok(await _trainingSystemService.DeleteAsync(request.Id, request.UserName));
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " TrainingSystemController DeleteAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }

        [HttpGet("GetAllTrainingSystem")]
        public IActionResult GetAllTrainingSystemAsync()
        {
            try
            {
                var result = _trainingSystemService.GetAllTrainingSystemAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                Log.Error(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " TrainingSystemController GetAllTrainingSystemAsync: " + ex.ToString());
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }
    }
}
