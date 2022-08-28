using BookingCalendar.Models.Dao;
using BookingCalendar.Models.Interface;

namespace BookingCalendar.Models.Instance
{
    public class InsLogin
    {
        private static DaoLogin? daoLogin;
        public static ILogin GetLogin()
        {
            if (daoLogin == null)
                daoLogin = new DaoLogin();
            return daoLogin;
        }
    }
}
