using FreeCourse.Shared.EventsContract;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class OrderRequestFailedEventConsumer : IConsumer<IOrderRequestFailedEvent>
    {
        public Task Consume(ConsumeContext<IOrderRequestFailedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
