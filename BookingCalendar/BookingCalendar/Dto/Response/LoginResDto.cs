using System.Text.Json.Serialization;

namespace BookingCalendar.Dto.Response
{
	public class LoginResDto
    {
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string AccessToken { get; set; }
	}
}

