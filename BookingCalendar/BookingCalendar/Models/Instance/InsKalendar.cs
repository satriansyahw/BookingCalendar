using BookingCalendar.Models.Dao;
using BookingCalendar.Models.Interface;

namespace BookingCalendar.Models.Instance
{
    public class InsKalendar
    {
        public static IKalendar GetKalendar()
        {
            return new DaoKalendar();
        }
    }
}
