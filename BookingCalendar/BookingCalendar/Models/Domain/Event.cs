namespace BookingCalendar.Models.Domain
{
    public class Event
    {
        public long Id { get; set; }

        public string EventCode { get; set; } = null!;
        public string EventName { get; set; } = null!;
    }
}
