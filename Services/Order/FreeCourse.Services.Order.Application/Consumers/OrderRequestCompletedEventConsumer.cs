using FreeCourse.Services.Order.Application.RepositoryContract;
using FreeCourse.Shared.Events;
using FreeCourse.Shared.EventsContract;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class OrderRequestCompletedEventConsumer : IConsumer<IOrderRequestCompletedEvent>
    {
        private readonly ILogger<OrderRequestCompletedEventConsumer> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderRequestCompletedEventConsumer(IOrderRepository orderRepository, ILogger<OrderRequestCompletedEventConsumer> logger)
        {
            this._orderRepository = orderRepository;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<IOrderRequestCompletedEvent> context)
        {
            var order = await this._orderRepository.GetAsync(context.Message.OrderId);

            if (order != null)
            {
                order.Status = Domain.OrderAggregate.OrderStatus.Success;

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
