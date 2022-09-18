using FreeCourse.Shared.EventsContract;
using MassTransit;
using System.Threading.Tasks;

namespace FreeCourse.Services.Payment.Consumers
{
    public class StockReservedRequestPaymentConsumer : IConsumer<IStockReservedRequestPayment>
    {
        public Task Consume(ConsumeContext<IStockReservedRequestPayment> context)
        {
            throw new System.NotImplementedException();
        }
    }
}
