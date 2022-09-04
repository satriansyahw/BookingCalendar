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
        private ILoginUseCase loginUseCase;
        private readonly JwtSettings jwtSettings;
        private readonly ILogger logger;
        public LoginController(JwtSettings jwtSettings, ILoggerFactory _logger,ILoginUseCase loginUseCase)
        {
            this.jwtSettings = jwtSettings;
            this.logger = _logger.CreateLogger<LoginController>();
            this.loginUseCase = loginUseCase;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<DataResponse> Post([FromBody] LoginReqDto loginDto)
        {
            this.logger.LogInformation("Authenticating process ...");
           return  await loginUseCase.DoAuthentication(loginDto,this.jwtSettings);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<DataResponse> Refresh([FromBody] LoginReqDto loginDto)
        {
            this.logger.LogInformation("Authenticating process ...");
            return await loginUseCase.DoAuthentication(loginDto, this.jwtSettings);
        }


        [HttpGet]
        public string Get()
        {
            this.logger.LogInformation("Hello World ...");
            return "Hello World";
        }
    }
}
