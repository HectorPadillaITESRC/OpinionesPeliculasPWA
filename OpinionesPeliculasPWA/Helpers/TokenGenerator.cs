using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using OpinionesPeliculasPWA.Models.Entities;

namespace OpinionesPeliculasPWA.Helpers
{
    public class TokenGenerator
    {
        private readonly IConfiguration configuration;
        public TokenGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Generate(Usuarios user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();


            var key = configuration.GetSection("JWT:Secret").Value;
            var issuer = configuration.GetSection("JWT:Issuer").Value;
            var audience = configuration.GetSection("JWT:Audience").Value;

            var claims = new List<Claim>
            {
                new ("Id", user.Id.ToString()),
                new (ClaimTypes.Email, user.Correo),
                new (ClaimTypes.Role, user.IdRol.ToString()),
            };

            var signinKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);


            JwtSecurityToken token = new(issuer, audience, claims, DateTime.Now, DateTime.Now.AddMinutes(20), signinKey);

            return tokenHandler.WriteToken(token);
        }
        public string GenerateRefresh(Usuarios user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();


            var key = configuration.GetSection("JWT:Secret").Value;
            var issuer = configuration.GetSection("JWT:Issuer").Value;
            var audience = configuration.GetSection("JWT:Audience").Value;

            var claims = new List<Claim>
            {
                new ("Id", Guid.NewGuid().ToString()),
                // new ("Id", user.Id.ToString())
            };

            var signinKey = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256);


            JwtSecurityToken token = new(issuer, audience, claims, DateTime.Now, DateTime.Now.AddDays(20), signinKey);

            return tokenHandler.WriteToken(token);
        }
    }
}