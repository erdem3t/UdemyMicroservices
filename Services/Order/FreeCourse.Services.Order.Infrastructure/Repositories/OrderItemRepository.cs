using FreeCourse.Services.Order.Application.RepositoryContract;

namespace FreeCourse.Services.Order.Infrastructure.Repositories
{
    public class OrderItemRepository : Repository<Domain.OrderAggregate.OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(OrderDbContext context) : base(context)
        {
        }
    }
}
