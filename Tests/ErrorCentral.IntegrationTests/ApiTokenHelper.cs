using ErrorCentral.Application.Services;
using ErrorCentral.Application.Settings;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ErrorCentral.IntegrationTests
{
    public class ApiTokenHelper
    {
        public ApiTokenHelper() { }
        public static string GetNormalUserToken()
        {
            return CreateToken();
        }

        private static string CreateToken(int id = 1, string name = "João", string email = "joao@email.com")
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes("js6%\\$(tzp8E(Lu!s4DhM.$ABNtj4Cqmf.:U");

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, email),
                    new Claim(ClaimTypes.Name, name),
                    new Claim(JwtRegisteredClaimNames.Email, email),
                    new Claim("id", id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = "MyEnvironment",
                Audience = "https://localhost",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
