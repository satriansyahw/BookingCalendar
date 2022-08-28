using BookingCalendar.Models.Domain;

namespace BookingCalendar.Models.Interface
{
    public interface IKalendar
    {
        Task<Kalendar> Save(Kalendar item);
        Task<Kalendar> Update(Kalendar item);
        Task<bool> IsAlreadyExist(Kalendar item);
        Task<bool> Delete(int calendarId);
        Task<Kalendar> Get(string userName, int calendarId);
        Task<List<Kalendar>> Get(string userName);
        Task<bool> IsAlreadyExistByEventName(string userName, string eventName);
    }
}
