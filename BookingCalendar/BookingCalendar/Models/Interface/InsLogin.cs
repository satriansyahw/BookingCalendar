using BookingCalendar.Models.Dao;

namespace BookingCalendar.Models.Interface
{
    public class InsLogin
    {
        private static DaoLogin? daoLogin;
        public static ILogin GetLogin()
        {
            if(daoLogin==null)
                daoLogin = new DaoLogin();
            return daoLogin;
        }
    }
}
