using Microsoft.Extensions.Hosting;

namespace BookingCalendar.Models.Domain
{
    public class Kalendar
    {
        public long Id { get; set; }

        public string UserName { get; set; } = null!;
        public string EventName { get; set; } = null!;
        public DateOnly CalDate { get; set; }
        public TimeOnly CalTimeStart { get; set; }
        public TimeOnly CallTimeEnd { get; set; }
        public bool IsAllDay { get; set; } = false;
        public long EventId { get; set; }

        public Event? Evening { get; set; }

    }
}
