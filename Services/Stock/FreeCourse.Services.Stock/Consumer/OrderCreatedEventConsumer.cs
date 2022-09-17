using FreeCourse.Shared.Events;
using MassTransit;
using System.Threading.Tasks;

namespace FreeCourse.Services.Stock.Consumer
{
    public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
    {
        public Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            throw new System.NotImplementedException();
        }
    }
}
