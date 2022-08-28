namespace BookingCalendar.Dto.Request
{
    public class KalendarWithIdReqDto
    {
        public long Id { get; set; }
        public string EventName { get; set; } = null!;
        public string CalDate { get; set; }= null!;
        public string CalTimeStart { get; set; } = null!;
        public string CalTimeEnd { get; set; } = null!;
        public bool IsAllDay { get; set; } = false;


    }
}
