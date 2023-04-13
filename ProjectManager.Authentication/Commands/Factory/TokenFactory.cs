using Newtonsoft.Json;
using ProjectManager.Authentication.Model;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManager.Authentication.Commands.Factory
{
    public interface ITokenFactory
    {
        Task<string> GenerateJwt(ClaimsIdentity identity,
                                    IJwtFactory jwtFactory,
                                   string userName,
                                   string roleName,
                                   JwtIssuerOptions jwtOptions,
                                   JsonSerializerSettings serializerSettings,
                                   List<Claim> claims = null
                                   );
    }

    public class TokenFactory : ITokenFactory
    {
        public async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtFactory jwtFactory,
            string userName, string roleName, JwtIssuerOptions jwtOptions,
            JsonSerializerSettings serializerSettings,
             List<Claim> claims = null)
        {
            var response = new AuthenticateResponse
            {
                Id = identity.Claims.Single(c => c.Type == "id").Value,
                Token = await jwtFactory.GenerateEncodedToken(userName, identity, claims),
                ExpireTime = (int)jwtOptions.ValidFor.TotalSeconds,
                Username = userName,
                RoleName = roleName
            };
            return JsonConvert.SerializeObject(response, serializerSettings);
        }
    }
}
