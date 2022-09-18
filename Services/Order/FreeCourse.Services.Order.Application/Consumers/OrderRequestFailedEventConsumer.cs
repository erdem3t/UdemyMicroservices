using FreeCourse.Services.Order.Application.RepositoryContract;
using FreeCourse.Shared.Events;
using FreeCourse.Shared.EventsContract;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class OrderRequestFailedEventConsumer : IConsumer<IOrderRequestFailedEvent>
    {
        private readonly ILogger<OrderRequestFailedEventConsumer> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderRequestFailedEventConsumer(ILogger<OrderRequestFailedEventConsumer> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<IOrderRequestFailedEvent> context)
        {
            var order = await this._orderRepository.GetAsync(context.Message.OrderId);

            if (order != null)
            {
                order.Status = Domain.OrderAggregate.OrderStatus.Fail;
                order.FailMessage = context.Message.Reason;
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
