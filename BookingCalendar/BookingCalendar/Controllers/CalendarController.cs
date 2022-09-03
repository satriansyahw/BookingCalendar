using BookingCalendar.Dto.Request;
using BookingCalendar.Models.Domain;
using BookingCalendar.UseCase;
using BookingCalendar.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;

namespace BookingCalendar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyCorsPolicy")]
    [Authorize]
    public class CalendarController : ControllerBase
    {
        CalendarUseCase calUseCase = new CalendarUseCase();
        GeneralHelper gh = new GeneralHelper();
        private readonly ILogger logger;

        public CalendarController( ILoggerFactory _logger)
        { 
            this.logger = _logger.CreateLogger<CalendarController>();
        }
        [HttpGet]
        [Route("{calendarId:long}")]
        public async Task<DataResponse> GetById(long calendarId)
        {
            logger.LogInformation("Get data Calendar By Id");
            string userName = gh.GetAuthInfo(this.HttpContext);
            return await calUseCase.Get(userName,calendarId);
        }
        [HttpGet]
        public async Task<DataResponse> Get()
        {
            logger.LogInformation("Get data Calendar By User name");
            string userName = gh.GetAuthInfo(this.HttpContext);
            return await calUseCase.Get(userName);
        }
        [HttpGet]
        [Route("Getx/{calendarId}")]
        public async Task<DataResponse> Getx(long calendarId)
        {
            logger.LogInformation("Get data Calendar By User name___"+calendarId.ToString());
            string userName = gh.GetAuthInfo(this.HttpContext);
            return await calUseCase.Getx(userName,calendarId);
        }
        [HttpGet]
        [Route("Gety")]
        public async Task<DataResponse> Gety([FromQuery]long calendarId)
        {
            logger.LogInformation("Get data Calendar By User name___xxx" + calendarId.ToString());
            string userName = gh.GetAuthInfo(this.HttpContext);
            return await calUseCase.Getx(userName,calendarId);
        }

        [HttpPatch]
        public async Task<DataResponse> Patch([FromBody] KalendarWithIdReqDto dto)
        {
            logger.LogInformation("Starting patching data ...");
            DateOnly.TryParse(dto.CalDate, out DateOnly calDate);
            if (!gh.IsValidDate(dto.CalDate))
                return new DataResponse(false, "wrong cal date", null);

            TimeOnly.TryParse(dto.CalTimeStart, out TimeOnly calTimeStart);
            if (!gh.IsValidTime(dto.CalTimeStart))
                return new DataResponse(false, "wrong cal time start", null);

            TimeOnly.TryParse(dto.CalTimeEnd, out TimeOnly calTimeEnd);
            if (!gh.IsValidTime(dto.CalTimeEnd))
                return new DataResponse(false, "wrong cal time end", null);



            string userName = gh.GetAuthInfo(this.HttpContext);
            Kalendar item = calUseCase.CalendarToBuilder(dto, userName);
            return await calUseCase.Update(item); ;

        }
        [HttpPost]
        public async Task<DataResponse> Post([FromBody] KalendarReqDto dto)
        {
            logger.LogInformation("Starting inserting data ...");
            DateOnly.TryParse(dto.CalDate, out DateOnly calDate);
            if(!gh.IsValidDate(dto.CalDate))
                return new DataResponse(false, "wrong cal date", null);

            TimeOnly.TryParse(dto.CalTimeStart, out TimeOnly calTimeStart);
            if (!gh.IsValidTime(dto.CalTimeStart))
                return new DataResponse(false, "wrong cal time start", null);

            TimeOnly.TryParse(dto.CalTimeEnd, out TimeOnly calTimeEnd);
            if (!gh.IsValidTime(dto.CalTimeEnd))
                return new DataResponse(false, "wrong cal time end", null);



            string userName = gh.GetAuthInfo(this.HttpContext);
            Kalendar item = calUseCase.CalendarToBuilder(dto, userName);
            return await calUseCase.Save(item);
        }
        [HttpPost]
        [Route("availibility")]
        public async Task<GenericResponse> PostCheckAvailability([FromBody] KalendarReqDto dto)
        {
            logger.LogInformation("Starting checking availibility ...");
            DateOnly.TryParse(dto.CalDate, out DateOnly calDate);
            if (!gh.IsValidDate(dto.CalDate))
                return new DataResponse(false, "wrong cal date", null);

            TimeOnly.TryParse(dto.CalTimeStart, out TimeOnly calTimeStart);
            if (!gh.IsValidTime(dto.CalTimeStart))
                return new DataResponse(false, "wrong cal time start", null);

            TimeOnly.TryParse(dto.CalTimeEnd, out TimeOnly calTimeEnd);
            if (!gh.IsValidTime(dto.CalTimeEnd))
                return new DataResponse(false, "wrong cal time end", null);



            string userName = gh.GetAuthInfo(this.HttpContext);
            Kalendar item = calUseCase.CalendarToBuilder(dto, userName);
            return await calUseCase.CheckAvailibility(item);
        }
        [HttpDelete]
        [Route("{calendarId:long}")]
        public async Task<GenericResponse> Delete(long calendarId)
        {
            logger.LogInformation("Starting deleting data ...");
            string userName = gh.GetAuthInfo(this.HttpContext);
            return await calUseCase.Delete(calendarId);
        }
    }
}
