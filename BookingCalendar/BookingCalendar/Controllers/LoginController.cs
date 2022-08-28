using BookingCalendar.Dto.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookingCalendar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public void Post([FromBody] LoginReqDto loginDto)
        {
           //FIXME : will return Response with token
        }
    }
}
