using BookingCalendar.Models.Dao;
using BookingCalendar.Models.Interface;

namespace BookingCalendar.Models.Instance
{
    public class InsLogin
    {
        public static ILogin GetLogin()
        {
            return new DaoLogin();
        }
    }
}
