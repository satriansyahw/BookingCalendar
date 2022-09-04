using BookingCalendar.Dto.Request;
using BookingCalendar.Dto.Response;
using BookingCalendar.Models;
using BookingCalendar.Models.Dao;
using BookingCalendar.Models.Instance;
using BookingCalendar.Models.Interface;
using BookingCalendar.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingCalendar.UseCase
{
    public class LoginUseCase:ILoginUseCase
    {
        ILogin loginDao = InsLogin.GetLogin();
        private readonly ILogger logger = LoggerFactory.Create(config =>
        {
            config.AddConsole();
        }).CreateLogger<LoginUseCase>();
        public async virtual Task<DataResponse> DoAuthentication(LoginReqDto reqDto,JwtSettings jwtSettings)
        {
            LoginResDto loginRes = new LoginResDto();
            if (!string.IsNullOrEmpty(reqDto.UserName))
            {
                string token = GenerateUserToken(reqDto.UserName, jwtSettings);
                if (!string.IsNullOrEmpty(token))
                {
                    logger.LogInformation("token not empty");
                    loginRes.AccessToken = token;
                    Login login = new Login { UserName = reqDto.UserName, IsActive = true };
                    login = await loginDao.Save(login);
                    logger.LogInformation("token saved __" + login.Id.ToString());
                    if (login.Id > 0)
                        return new DataResponse(true, "token created", loginRes);
                }
            }
            return new DataResponse(false, "failed token creation", loginRes);
        }
        private string GenerateUserToken(string userName, JwtSettings jwtSettings)
        {
            logger.LogInformation("starting create token ");
            var now = DateTime.UtcNow;
            string _issuer = jwtSettings.ValidIssuer;
            string _keyToken = jwtSettings.IssuerSigningKey;
            int _tokenExpired = jwtSettings.TokenExpiredInMinutes;

            if (string.IsNullOrEmpty(_issuer) | string.IsNullOrEmpty(_keyToken) | _tokenExpired == 0)
                return String.Empty;

            DateTime dt = DateTime.Now;
            var claims = new[] {
            new Claim("un",userName),
            new Claim(JwtRegisteredClaimNames.AuthTime,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"))
            };
            DateTime dtExpired = DateTime.Now.AddMinutes(_tokenExpired);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyToken));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            if (key != null && claims != null && creds != null)
            {
                var token = new JwtSecurityToken(_issuer,
                  _issuer,
                   claims,
                  expires: dtExpired,
                  signingCredentials: creds);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return String.Empty;
        }
    }
}
