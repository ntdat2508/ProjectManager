using Microsoft.Extensions.Options;
using ProjectManager.Authentication.Model;
using ProjectManager.Shared.Constants;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ProjectManager.Authentication.Commands.Factory
{
    public interface IJwtFactory
    {
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity, List<Claim> additionalClaims);
        ClaimsIdentity GenerateClaimsIdentity(string userName, long id);
    }

    public class JwtFactory : IJwtFactory
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtFactory(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, long id)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
           {
                new Claim(Constants.Strings.JwtClaimIdentifiers.Id, id.ToString()),
                new Claim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess)
            });
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity, List<Claim> additionalClaims)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Rol),
                identity.FindFirst(Constants.Strings.JwtClaimIdentifiers.Id)
            };
            if (additionalClaims != null && additionalClaims.Count > 0)
            {
                claims.AddRange(additionalClaims);
            }
            var jwt = new JwtSecurityToken(
               issuer: _jwtOptions.Issuer,
               audience: _jwtOptions.Audience,
               claims: claims,
               notBefore: _jwtOptions.NotBefore,
               expires: _jwtOptions.Expiration,
               signingCredentials: _jwtOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() -
                             new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
            .TotalSeconds);
    }
}
