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
        ILogin loginDao;//= InsLogin.GetLogin();
        private readonly ILogger logger = LoggerFactory.Create(config =>
        {
            config.AddConsole();
        }).CreateLogger<LoginUseCase>();

        public LoginUseCase(ILogin loginDao)
        {
            this.loginDao = loginDao;
            this.logger = logger;
        }

        public async Task<DataResponse> DoAuthentication(LoginReqDto reqDto,JwtSettings jwtSettings)
        {
            LoginResDto loginRes = new LoginResDto();
            if (!string.IsNullOrEmpty(reqDto.UserName))
            {
                string token = GenerateUserToken(reqDto.UserName, jwtSettings,true);
                string tokenRefresh = GenerateUserToken(reqDto.UserName, jwtSettings,false);
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(tokenRefresh))
                {
                    logger.LogInformation("token not empty");
                    loginRes.AccessToken = token;
                    loginRes.RefreshToken = tokenRefresh;
                    Login login = new Login { UserName = reqDto.UserName, IsActive = true,RefreshToken=tokenRefresh };
                    login =  await loginDao.Save(login);
                    logger.LogInformation("token saved __" + login.Id.ToString());
                    if (login.Id > 0)
                        return new DataResponse(true, "token created", loginRes);
                }
            }
            return new DataResponse(false, "failed token creation", loginRes);
        }
        private string GenerateUserToken(string userName, JwtSettings jwtSettings,bool isAccessToken)
        {
            logger.LogInformation("starting create token ");
            var now = DateTime.UtcNow;
            string _issuer = jwtSettings.ValidIssuer;
            string _keyToken = isAccessToken? jwtSettings.IssuerSigningKey:jwtSettings.IssuerSigningKeyRefresh;
            int _tokenExpired = isAccessToken? jwtSettings.TokenExpiredInMinutes:jwtSettings.TokenExpiredInMinutesRefresh;

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
