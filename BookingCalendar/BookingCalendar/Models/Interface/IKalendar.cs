using BookingCalendar.Dto.Response;
using BookingCalendar.Models.Domain;

namespace BookingCalendar.Models.Interface
{
    public interface IKalendar
    {
        Task<Kalendar> Save(Kalendar item);
        Task<Kalendar> Update(Kalendar item);
        Task<bool> IsAlreadyExist(Kalendar item);
        Task<bool> Delete(long calendarId);
        Task<Kalendar> Get(string userName, long calendarId);
        Task<KalEventResDto> Getx(string userName, long calendarId);
        Task<List<KalendarResDto>> Get(string userName);
        Task<bool> IsAlreadyExistByEventName(string userName, string eventName);
    }
}
