using System;
namespace BookingCalendar.Models.Interface
{
	public interface ILogin 
	{
		Task<Login> Save(Login item);
		bool IsLoginIn(string userName);
	}
}

