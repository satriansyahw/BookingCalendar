using System;
namespace BookingCalendar.Models.Interface
{
	public interface ILogin 
	{
		Login Save(Login item);
		bool IsLoginIn(string userName);
	}
}

