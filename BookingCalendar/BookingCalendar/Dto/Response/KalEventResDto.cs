namespace BookingCalendar.Dto.Response
{
    public class KalEventResDto
    {
        public long Id { get; set; }
        public string UserName { get; set; } = null!;
        public string EventName { get; set; } = null!;

        public string EventCode { get; set; } = null!;
        public string CalDate { get; set; } = null!;
        public long EventId { get; set; }
        public bool IsAllDay { get; set; } = false;
        //a.Id,a.CalDate,a.UserName,a.IsAllDay,a.EventId,b.EventCode,b.EventName
    }
}
