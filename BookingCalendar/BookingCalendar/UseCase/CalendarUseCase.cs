using BookingCalendar.Dto.Request;
using BookingCalendar.Dto.Response;
using BookingCalendar.Models.Domain;
using BookingCalendar.Models.Instance;
using BookingCalendar.Models.Interface;
using BookingCalendar.Utils;
using System.Globalization;

namespace BookingCalendar.UseCase
{
    public class CalendarUseCase
    {
        IKalendar calDao = InsKalendar.GetKalendar();

        private readonly ILogger logger = LoggerFactory.Create(config =>
        {
            config.AddConsole();
        }).CreateLogger<CalendarUseCase>();


        public async Task<GenericResponse> CheckAvailibility(Kalendar item)
        {
            bool isAlreadyExists = await calDao.IsAlreadyExist(item);
            logger.LogInformation("CheckAvailibility ,already exist ="+isAlreadyExists.ToString());

            if (!isAlreadyExists)
                return new GenericResponse(true, "Event Date & time available");
            else
                return new GenericResponse(false, "Event Date & time conflicts");
        }
        public async Task<DataResponse> Save(Kalendar item)
        {
            bool isAlreadyExists = await calDao.IsAlreadyExist(item);
            Kalendar itemResult = await calDao.Save(item);
            KalendarResDto result = this.CalendarResDtoToBuilder(itemResult);
            string existMessage = isAlreadyExists == true ? " but conflict with other events " : string.Empty;
            logger.LogInformation("save calendar " + existMessage.ToString());
            if (itemResult.Id > 0)
                return new DataResponse(true, "Calendar event created " + existMessage, result);
            else
                return new DataResponse(false, "failed to create Calendar event ", null);
        }
        public async Task<DataResponse> Update(Kalendar item)
        {
            Kalendar itemResult = await calDao.Update(item);
            KalendarResDto result = this.CalendarResDtoToBuilder(itemResult);
            logger.LogInformation("Update calendar ....");
            if (itemResult != null)
                return new DataResponse(true, "Calendar  event updated", result);
            else
                return new DataResponse(false, "failed to update Calendar  event", null);
        }
        public async Task<GenericResponse> Delete(long calendarId)
        {
            bool result = await calDao.Delete(calendarId);
            logger.LogInformation("Delete calendar ...." + result.ToString());
            if (result)
                return new GenericResponse(true, "Calendar  event deleted ");
            else
                return new GenericResponse(false, "failed to update Calendar  event");
        }
        public async Task<GenericResponse> SaveOrUpdate(Kalendar item)//patch
        {
            bool resultEvent = await calDao.IsAlreadyExistByEventName(item.UserName, item.EventName);
            if (resultEvent)
            {
                Kalendar cal = await calDao.Update(item);
                if (cal != null)
                    return new DataResponse(true, "Calendar  event updated", cal);
                else
                    return new DataResponse(false, "failed to update Calendar  event", null);
            }
            else
            {
                Kalendar cal = await calDao.Save(item);
                if (cal != null)
                    return new DataResponse(true, "Calendar  event created", cal);
                else
                    return new DataResponse(false, "failed to created Calendar  event", null);
            }
        }
        public async Task<DataResponse> Get(string userName)
        {
            List<KalendarResDto> result  =  await calDao.Get(userName);
            logger.LogInformation("Get all data by username ...." + result.Count.ToString());
            return new DataResponse(true, "Get all data by username", result);
        }
        public async Task<DataResponse> Get(string userName, long calendarId)
        {
            Kalendar itemResult = await calDao.Get(userName,calendarId);
            KalendarResDto result =   this.CalendarResDtoToBuilder(itemResult);
            logger.LogInformation("Get by CalendarId ...." + result.ToString());
            return new DataResponse(true, "Get by CalendarId", result);
        }
        public Kalendar CalendarToBuilder(KalendarReqDto dto, string userName)
        {
            DateOnly.TryParse(dto.CalDate, out DateOnly calDate);
            TimeOnly.TryParse(dto.CalTimeStart, out TimeOnly calTimeStart);
            TimeOnly.TryParse(dto.CalTimeEnd, out TimeOnly calTimeEnd);

            Kalendar item = new Kalendar();
            if (dto != null)
            {
                item.CalTimeStart = calTimeStart;
                item.CallTimeEnd = calTimeEnd;
                item.IsAllDay = dto.IsAllDay;
                item.EventName = dto.EventName;
                item.CalDate = calDate;
            }
            item.UserName = userName;


            return item;
        }

        public Kalendar CalendarToBuilder(KalendarWithIdReqDto dto, string userName)
        {
            DateOnly.TryParse(dto.CalDate, out DateOnly calDate);
            TimeOnly.TryParse(dto.CalTimeStart, out TimeOnly calTimeStart);
            TimeOnly.TryParse(dto.CalTimeEnd, out TimeOnly calTimeEnd);

            Kalendar item = new Kalendar();
            if (dto != null)
            {
                item.CalTimeStart = calTimeStart;
                item.CallTimeEnd = calTimeEnd;
                item.IsAllDay = dto.IsAllDay;
                item.EventName = dto.EventName;
                item.CalDate = calDate;
                item.Id = dto.Id;
                item.UserName = userName;
            }

            return item;
        }
        public KalendarResDto CalendarResDtoToBuilder(Kalendar item)
        {
            KalendarResDto result = new KalendarResDto();
            if (item != null)
            {
                result.CalTimeStart = item.CalTimeStart.ToString("HH:mm");
                result.CalTimeEnd = item.CallTimeEnd.ToString("HH:mm");
                result.IsAllDay = item.IsAllDay;
                result.EventName = item.EventName;
                result.CalDate = item.CalDate.ToString("yyyy-MM-dd");
                result.UserName = item.UserName;
                result.Id = item.Id;
            }
            return result;
        }
        public async Task<DataResponse> Getx(string userName,long calendarId)
        {
            KalEventResDto result = await calDao.Getx(userName,calendarId);
            return new DataResponse(true, "Get all data by username", result);


        }
    }
}