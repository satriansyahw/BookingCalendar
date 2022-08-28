namespace BookingCalendar.Dto.Request
{
    public class KalendarReqDto
    {
        public string EventName { get; set; } = null!;
        public string CalDate { get; set; }= null!;
        public string CalTimeStart { get; set; } = null!;
        public string CalTimeEnd { get; set; } = null!;
        public bool IsAllDay { get; set; } = false;
    }
}
