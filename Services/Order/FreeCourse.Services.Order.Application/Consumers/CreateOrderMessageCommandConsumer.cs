using FreeCourse.Services.Order.Application.RepositoryContract;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Shared.Messages;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly IOrderRepository orderRepository;

        public CreateOrderMessageCommandConsumer(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            Domain.OrderAggregate.Address address = new(context.Message.Address.Province, context.Message.Address.District, context.Message.Address.Street, context.Message.Address.ZipCode, context.Message.Address.Line);

            Domain.OrderAggregate.Order order = new(context.Message.BuyerId, address, OrderStatus.Suspend);

            context.Message.OrderItems.ForEach(x =>
            {
                order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl, x.Count);
            });

            await orderRepository.CreateAsync(order);
        }
    }
}
