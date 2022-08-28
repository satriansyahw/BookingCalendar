using BookingCalendar.Models.Dao;
using BookingCalendar.Models.Interface;

namespace BookingCalendar.Models.Instance
{
    public class InsKalendar
    {
        private static DaoKalendar? daoKalendar;
        public static IKalendar GetKalendar()
        {
           // if (daoKalendar == null)
                daoKalendar = new DaoKalendar();
            return daoKalendar;
        }
    }
}
