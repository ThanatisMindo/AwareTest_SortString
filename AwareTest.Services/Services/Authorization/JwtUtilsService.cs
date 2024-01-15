using AwareTest.Model.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AwareTest.Services.IService;
using Microsoft.Extensions.Configuration;

namespace AwareTest.Services.Services.Authorization
{
    public class JwtUtilsService : IJwtUtilsService
    {
        private AppSettingsModel _appSettings;
        private static IConfiguration _configuration = null;

        public JwtUtilsService(IConfiguration configuration)
        {
            _configuration = configuration;
            _appSettings = new AppSettingsModel();

            if (string.IsNullOrEmpty(_configuration.GetSection("AppSettings:Secret").Value))
            {
                throw new Exception("JWT secret not configured");
            }
            else
            {
                _appSettings.Secret = _configuration.GetSection("AppSettings:Secret").Value;
            }
        }

        public string GenerateJwtToken(UserModel user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                //var validations = new TokenValidationParameters
                //{
                //    ValidateIssuerSigningKey = true,
                //    IssuerSigningKey = key,
                //    ValidateIssuer = true,
                //    ValidateAudience = true,
                //    ValidateLifetime = false,
                //    // Add these...
                //    ValidIssuer = configuration["JwtAuthentication:Issuer"],
                //    ValidAudience = configuration["JwtAuthentication:Audience"]
                //};

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
