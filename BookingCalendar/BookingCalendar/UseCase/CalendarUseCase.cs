using BookingCalendar.Dto.Request;
using BookingCalendar.Models.Domain;
using BookingCalendar.Models.Instance;
using BookingCalendar.Models.Interface;
using BookingCalendar.Utils;

namespace BookingCalendar.UseCase
{
    public class CalendarUseCase
    {
        IKalendar calDao = InsKalendar.GetKalendar();
        /* Task<Kalendar> Save(Kalendar item);
        Task<bool> Update(Kalendar item);
        Task<bool> IsAlreadyExist(Kalendar item);
        Task<bool> Delete(int calendarId);
        Task<Kalendar> Get(string userName, int calendarId);
        Task<List<Kalendar>> Get(string userName);
        Task<bool> IsAlreadyExistByEventName(string userName, string eventName);
         */
        public async Task<DataResponse> Save(Kalendar item)//post
        {
            bool isAlreadyExists = await calDao.IsAlreadyExist(item);
            Kalendar itemResult = await calDao.Save(item);
            string existMessage = isAlreadyExists == true ? "but conflicted with other events " : string.Empty;
            if (itemResult != null)
                return new DataResponse(true, "Calendar event created " + existMessage, itemResult);
            else
                return new DataResponse(false, "failed to create Calendar event ", null);
        }
        public async Task<DataResponse> Update(Kalendar item)//patch
        {
            Kalendar result = await calDao.Update(item);
            if (result != null)
                return new DataResponse(true, "Calendar  event updated", result);
            else
                return new DataResponse(false, "failed to update Calendar  event", null);
        }
        public async Task<GenericResponse> Delete(int calendarId)//delete
        {
            bool result = await calDao.Delete(calendarId);
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
            List<Kalendar> result  =  await calDao.Get(userName);
            return new DataResponse(true, "Get all data by username", result);
        }
        public async Task<DataResponse> Get(string userName,int calendarId)
        {
            Kalendar result = await calDao.Get(userName,calendarId);
            return new DataResponse(true, "Get by CalendarId", result);
        }
    }
}