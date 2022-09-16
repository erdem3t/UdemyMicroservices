using FreeCourse.Services.Order.Domain.Core;

namespace FreeCourse.Services.Order.Application.RepositoryContract
{
    public interface IOrderItemRepository : IRepository<Domain.OrderAggregate.OrderItem>
    {
    }
}
