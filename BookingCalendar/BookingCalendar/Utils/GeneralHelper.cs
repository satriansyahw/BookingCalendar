using System;
using System.Security.Claims;

namespace BookingCalendar.Utils
{
	public class GeneralHelper
	{
		public GeneralHelper()
		{
		}
		public string GetAuthInfo(HttpContext httpContext)
        {
            string result = String.Empty; ;
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                result = identity.Claims.First(claim => claim.Type == "un").Value.ToString();
                //FIXME later need to decide object to returned
            }
            return result;
        }
        public bool IsValidDate(string myDate)
        {
            DateOnly.TryParse(myDate, out DateOnly date);
            string myDateCheck = date.ToString("yyyy-MM-dd");
            if (myDate.Trim() == "0001-01-01" | myDateCheck == "0001-01-01")
                return false;

            return true;
        }
        public bool IsValidTime(string myTime)
        {
            TimeOnly.TryParse(myTime, out TimeOnly calMyTime);
            string calTimeStartCheck = calMyTime.ToString("HH:mm");
            if (myTime.Trim() != "00:00" && calTimeStartCheck == "00:00")
                return false;

            return true;
        }

    }
}

