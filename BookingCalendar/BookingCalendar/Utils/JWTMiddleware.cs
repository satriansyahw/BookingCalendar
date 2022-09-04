using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BookingCalendar.Utils
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings jwtSettings;

        public JWTMiddleware(RequestDelegate next, IConfiguration configuration, JwtSettings jwtSettings)
        {
            _next = next;
            _configuration = configuration;
            this.jwtSettings = jwtSettings;

        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var pathRefresh = context.Request.Path.Value.ToString();
            if (token != null)
            {
                if(pathRefresh.Equals("/api/login/refresh"))
                {
                    attachAccountToContext(context, token);
                }
                else
                {
                    attachAccountToContext(context, token);

                }
            }
            await _next(context);
        }

        private void attachAccountToContext(HttpContext context, string token)
        {
            try
            {


                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = this.jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this.jwtSettings.IssuerSigningKey)),
                    ValidateIssuer = this.jwtSettings.ValidateIssuer,
                    ValidIssuer = this.jwtSettings.ValidIssuer,
                    ValidateAudience = this.jwtSettings.ValidateAudience,
                    ValidAudience = this.jwtSettings.ValidAudience,
                    RequireExpirationTime = this.jwtSettings.RequireExpirationTime,
                    ValidateLifetime = this.jwtSettings.RequireExpirationTime,
                    ClockSkew = TimeSpan.FromDays(1),
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                //var accountId = jwtToken.Claims.First(x => x.Type == "id").Value;

                // attach account to context on successful jwt validation
             //   context.Items["User"] = _userService.GetUserDetails();
            }
            catch
            {
                // do nothing if jwt validation fails
                // account is not attached to context so request won't have access to secure routes
            }
        }
    }
}

