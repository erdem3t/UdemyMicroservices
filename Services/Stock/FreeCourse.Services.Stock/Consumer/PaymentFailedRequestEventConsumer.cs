using FreeCourse.Shared.Events;
using MassTransit;
using System.Threading.Tasks;

namespace FreeCourse.Services.Stock.Consumer
{
    public class PaymentFailedRequestEventConsumer : IConsumer<PaymentFailedRequestStock>
    {
        public Task Consume(ConsumeContext<PaymentFailedRequestStock> context)
        {
            throw new System.NotImplementedException();
        }
    }
}
