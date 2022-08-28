using System.Text.Json.Serialization;

namespace Enitam.Dto.Response
{
	public struct BookingCalendar
    {
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string AccessToken { get; set; }
	}
}

