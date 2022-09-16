using FreeCourse.Services.Order.Application.RepositoryContract;
using FreeCourse.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly ILogger<PaymentCompletedEventConsumer> _logger;
        private readonly IOrderRepository _orderRepository;

        public PaymentFailedEventConsumer(ILogger<PaymentCompletedEventConsumer> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
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
