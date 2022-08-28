using BookingCalendar.Dto.Request;
using BookingCalendar.UseCase;
using BookingCalendar.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingCalendar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyCorsPolicy")]
    [Authorize]
    public class LoginController : ControllerBase
    {
        LoginUseCase loginUseCase = new LoginUseCase();
        private readonly JwtSettings jwtSettings;

        public LoginController(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<DataResponse> Post([FromBody] LoginReqDto loginDto)
        {
           return  await loginUseCase.DoAuthentication(loginDto,this.jwtSettings);
        }


        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }
    }
}
