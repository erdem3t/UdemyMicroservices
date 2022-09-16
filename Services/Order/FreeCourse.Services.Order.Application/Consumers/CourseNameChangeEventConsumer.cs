using FreeCourse.Services.Order.Application.RepositoryContract;
using FreeCourse.Shared.Messages;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Consumers
{
    public class CourseNameChangeEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly IOrderItemRepository orderItemRepository;

        public CourseNameChangeEventConsumer(IOrderItemRepository orderItemRepository)
        {
            this.orderItemRepository = orderItemRepository;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var orderItems = await this.orderItemRepository.GetAllAsync(p => p.ProductId == context.Message.CourseId);

            orderItems.ForEach(orderItem =>
            {
                orderItem.UpdateOrderItem(context.Message.UpdatedName, orderItem.PictureUrl, orderItem.Price, orderItem.Count);
                this.orderItemRepository.UpdateAsync(orderItem);
            });
        }
    }
}
