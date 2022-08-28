using BookingCalendar.Dto.Request;
using BookingCalendar.Dto.Response;
using BookingCalendar.Models;
using BookingCalendar.Models.Dao;
using BookingCalendar.Models.Interface;
using BookingCalendar.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingCalendar.UseCase
{
    public class LoginUseCase
    {
        ILogin loginDao = InsLogin.GetLogin();
        public async Task<DataResponse> DoAuthentication(LoginReqDto _reqDto,JwtSettings jwtSettings)
        {
            LoginResDto loginRes = new LoginResDto();
            string token = GenerateUserToken(_reqDto.UserName,jwtSettings);
            if (!string.IsNullOrEmpty(token)){
                loginRes.AccessToken = token;
                Login login = new Login { UserName = _reqDto.UserName, IsActive = true };
                login = await loginDao.Save(login);
                if(login.Id > 0)
                    return new DataResponse( true, "token created", loginRes);
            }
            return new DataResponse(false, "failed token creation", loginRes);
        }
        private string GenerateUserToken(string userName, JwtSettings jwtSettings)
        {

            var now = DateTime.UtcNow;
            string _issuer = jwtSettings.ValidIssuer;
            string _keyToken = jwtSettings.IssuerSigningKey;
            int _tokenExpired = jwtSettings.TokenExpiredInMinutes;

            DateTime dt = DateTime.Now;
            var claims = new[] {
            new Claim("un",userName),
            new Claim(JwtRegisteredClaimNames.AuthTime,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"))
            };
            DateTime dtExpired = DateTime.Now.AddMinutes(_tokenExpired);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_keyToken));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            if (!string.IsNullOrEmpty(_issuer)
                && !string.IsNullOrEmpty(_keyToken)
                && key != null && claims != null
                && creds != null)
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
