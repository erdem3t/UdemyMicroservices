namespace FreeCourse.Shared.EventsContract
{
    public interface IOrderRequestCompletedEvent
    {
        public int OrderId { get; set; }
    }
}
