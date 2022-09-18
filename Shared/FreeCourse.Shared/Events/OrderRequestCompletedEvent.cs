using FreeCourse.Shared.EventsContract;

namespace FreeCourse.Shared.Events
{
    public class OrderRequestCompletedEvent : IOrderRequestCompletedEvent
    {
        public int OrderId { get; set; }
    }
}
