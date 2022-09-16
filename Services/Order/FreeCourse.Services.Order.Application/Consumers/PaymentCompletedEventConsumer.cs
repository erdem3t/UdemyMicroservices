using FreeCourse.Services.Order.Application.RepositoryContract;
using FreeCourse.Shared.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly ILogger<PaymentCompletedEventConsumer> _logger;
        private readonly IOrderRepository _orderRepository;

        public PaymentCompletedEventConsumer(IOrderRepository orderRepository, ILogger<PaymentCompletedEventConsumer> logger)
        {
            this._orderRepository = orderRepository;
            this._logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
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
