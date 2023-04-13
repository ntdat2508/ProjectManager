using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ProjectManager.Authentication.Commands.Factory;
using ProjectManager.Authentication.Model;
using ProjectManager.Repository;
using ProjectManager.Shared.Common.Enum;
using ProjectManager.Shared.Constants;
using ProjectManager.Shared.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManager.Authentication.Commands
{
    public interface ILoginCommand
    {
        Task<Response<AuthenticateResponse>> Authenticate(LoginRequest request);
    }

    public class LoginCommand : ILoginCommand
    {
        private readonly ITokenFactory _tokenFactory;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommand(ITokenFactory tokenFactory, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, IUnitOfWork unitOfWork)
        {
            _tokenFactory = tokenFactory;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<AuthenticateResponse>> Authenticate(LoginRequest request)
        {
            var claims = new List<Claim>();
            var roleName = request.Role;
            if (request.Role == RoleEnum.Student.ToString())
            {
                var user = await _unitOfWork.StudentRepository.GetSingleAsync(x => x.Username.ToLower() == request.UserName.ToLower() && x.Password == EncriptFunctionsHelper.GeneratePassword(request.Password) );
                if (user == null)
                {
                    return new Response<AuthenticateResponse>
                    {
                        Code = Convert.ToInt32(HttpStatusCode.NotFound),
                        Message = Constants.Message.UserInActiveMessage
                    };
                }
            }

            if (request.Role == RoleEnum.Teacher.ToString())
            {
                var user = await _unitOfWork.TeacherRepository.GetSingleAsync(x => x.Username.ToLower() == request.UserName.ToLower() && x.Password == EncriptFunctionsHelper.GeneratePassword(request.Password) );
                if (user == null)
                {
                    return new Response<AuthenticateResponse>
                    {
                        Code = Convert.ToInt32(HttpStatusCode.NotFound),
                        Message = Constants.Message.UserInActiveMessage
                    };
                }
            }

            if (request.Role == RoleEnum.Admin.ToString())
            {
                var user = await _unitOfWork.UserManagementRepository.GetSingleAsync(x => x.Username.ToLower() == request.UserName.ToLower() && x.Password == EncriptFunctionsHelper.GeneratePassword(request.Password) );
                if (user == null)
                {
                    return new Response<AuthenticateResponse>
                    {
                        Code = Convert.ToInt32(HttpStatusCode.NotFound),
                        Message = Constants.Message.UserInActiveMessage
                    };
                }
            }

            var identity = await GetClaimsIdentity(request);

            claims.Add(new Claim(ClaimTypes.Role, roleName));

            var jwt = await _tokenFactory.GenerateJwt(identity,
                                                        _jwtFactory,
                                                       request.UserName,
                                                       roleName,
                                                       _jwtOptions,
                                                       new JsonSerializerSettings { Formatting = Formatting.Indented },
                                                       claims);
            var jwtObject = JsonConvert.DeserializeObject<AuthenticateResponse>(jwt);
            return new Response<AuthenticateResponse>
            {
                Code = Convert.ToInt32(HttpStatusCode.OK),
                Message = Constants.Message.Successfully,
                Data = jwtObject
            };
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request?.UserName) || string.IsNullOrEmpty(request?.Password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            if (request.Role == RoleEnum.Student.ToString())
            {
                var userToVerify = _unitOfWork.StudentRepository.FindBy(x => x.Username == request.UserName);

                if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

                // check the credentials
                if (EncriptFunctionsHelper.GeneratePassword(request.Password) == userToVerify.FirstOrDefault().Password)
                {
                    return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(request.UserName, userToVerify.FirstOrDefault().Id));
                }
            }

            if (request.Role == RoleEnum.Teacher.ToString())
            {
                var userToVerify = _unitOfWork.TeacherRepository.FindBy(x => x.Username == request.UserName);

                if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

                // check the credentials
                if (EncriptFunctionsHelper.GeneratePassword(request.Password) == userToVerify.FirstOrDefault().Password)
                {
                    return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(request.UserName, userToVerify.FirstOrDefault().Id));
                }
            }

            if (request.Role == RoleEnum.Admin.ToString())
            {
                var userToVerify = _unitOfWork.UserManagementRepository.FindBy(x => x.Username == request.UserName);

                if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

                // check the credentials
                if (EncriptFunctionsHelper.GeneratePassword(request.Password) == userToVerify.FirstOrDefault().Password)
                {
                    return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(request.UserName, userToVerify.FirstOrDefault().Id));
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
