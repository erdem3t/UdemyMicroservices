using FreeCourse.Services.Order.Domain.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.RepositoryContract
{
    public interface IOrderRepository : IRepository<Domain.OrderAggregate.Order>
    {
        public Task<List<Domain.OrderAggregate.Order>> GetOrderByUserId(string userId);
    }
}
