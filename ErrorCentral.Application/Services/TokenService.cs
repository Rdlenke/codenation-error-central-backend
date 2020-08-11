using ErrorCentral.Application.Settings;
using ErrorCentral.Domain.AggregatesModel.UserAggregate;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ErrorCentral.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly Jwt _jwt;
        public TokenService(Jwt jwt)
        {
            _jwt = jwt;
        }

        public string GenerateToken(User user)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwt.Secret);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(_jwt.Expiration),
                Issuer = _jwt.Issuer,
                Audience = _jwt.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
