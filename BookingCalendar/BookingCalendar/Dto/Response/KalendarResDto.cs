namespace BookingCalendar.Dto.Response
{
    public class KalendarResDto
    {
        public long Id { get; set; }
        public string UserName { get; set; } = null!;
        public string EventName { get; set; } = null!;
        public string CalDate { get; set; } = null!;
        public string CalTimeStart { get; set; } = null!;
        public string CalTimeEnd { get; set; } = null!;
        public bool IsAllDay { get; set; } = false;
    }
}
