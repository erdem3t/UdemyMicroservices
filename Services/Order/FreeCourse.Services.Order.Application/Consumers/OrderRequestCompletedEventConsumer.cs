using FreeCourse.Shared.EventsContract;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class OrderRequestCompletedEventConsumer : IConsumer<IOrderRequestCompletedEvent>
    {
        public Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
