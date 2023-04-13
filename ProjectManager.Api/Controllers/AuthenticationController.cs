using Microsoft.AspNetCore.Mvc;
using ProjectManager.Authentication.Commands;
using ProjectManager.Authentication.Model;
using ProjectManager.Shared.Constants;
using Serilog;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace ProjectManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoginCommand _loginCommand;

        public AuthenticationController(ILoginCommand loginCommand)
        {
            _loginCommand = loginCommand;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(Constants.Message.ModelStateMessage);
                }
                return Ok(await _loginCommand.Authenticate(request));
            }
            catch (Exception ex)
            {
                Log.Fatal(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " AuthenticationController: " + ex.Message);
                return StatusCode(Convert.ToInt32(HttpStatusCode.InternalServerError), Constants.Message.InternalServer);
            }
        }
    }
}
