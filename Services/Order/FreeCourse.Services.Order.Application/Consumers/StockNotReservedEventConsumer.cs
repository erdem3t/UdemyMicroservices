using FreeCourse.Services.Order.Application.RepositoryContract;
using FreeCourse.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        private readonly ILogger<StockNotReservedEventConsumer> _logger;
        private readonly IOrderRepository _orderRepository;

        public StockNotReservedEventConsumer(ILogger<StockNotReservedEventConsumer> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await this._orderRepository.GetAsync(context.Message.OrderId);

            if (order != null)
            {
                order.Status = Domain.OrderAggregate.OrderStatus.Fail;
                order.FailMessage = context.Message.Message;
                await this._orderRepository.UpdateAsync(order);

                _logger.LogInformation($"Order Id {context.Message.OrderId} status change : {order.Status}");
            }
            else
            {
                _logger.LogError($"Order Id {context.Message.OrderId} status not change");
            }
        }
    }
}
