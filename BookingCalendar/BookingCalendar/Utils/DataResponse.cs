using System;
using System.Text.Json.Serialization;

namespace BookingCalendar.Utils
{
	public class DataResponse:GenericResponse
	{
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(2)]
        public object? Data { get; set; }
        public DataResponse(Boolean? isSuccess, String? message,object? data)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
            this.Data = data;
        }
    }
}

