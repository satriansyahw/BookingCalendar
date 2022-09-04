using System;
namespace BookingCalendar.Models.Interface
{
	public interface ILogin 
	{
		Task<Login> Save(Login item);
		Task<bool> IsLoginIn(string userName);

        Task<bool> IsLoginIn(string userName,string refreshToken);
    }
}

