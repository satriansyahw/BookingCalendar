using System;
namespace BookingCalendar.Models
{
	public class Login
	{
		public long Id { get; set; }
		public string UserName { get; set; } = null!;
		public bool IsActive { get; set; } = true;
	}
}
