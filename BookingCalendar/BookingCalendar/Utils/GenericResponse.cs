

using System.Text.Json.Serialization;

namespace BookingCalendar.Utils
{
	public class GenericResponse
	{

        [JsonPropertyOrder(-1)]
        public int? Status { get; set; } = 200;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(0)]
        public Boolean? IsSuccess { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(1)]
        public String? Message { get; set; }

        public GenericResponse() { }
        public GenericResponse(Boolean? isSuccess, String? message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }
    }
}

