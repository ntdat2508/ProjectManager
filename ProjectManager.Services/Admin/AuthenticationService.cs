using Microsoft.Extensions.Options;
using ProjectManager.Authentication.Model;
using ProjectManager.Shared.Helper;
using ProjectManager.Shared.Model;
using System.Threading.Tasks;

namespace ProjectManager.Services.Admin
{
    public interface IAuthenticationService
    {
        Task<Response<AuthenticateResponse>> LoginAsync(LoginRequest request);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;

        public AuthenticationService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<Response<AuthenticateResponse>> LoginAsync(LoginRequest request)
        {
            var client = new HttpClientHelper();
            var response = await client.PostAsync<Response<AuthenticateResponse>>(request, _appSettings.BaseUri, _appSettings.Authentication, string.Empty);
            return response;
        }
    }
}
