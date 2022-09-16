using FreeCourse.Services.Order.Application.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Infrastructure.Repositories
{
    public class OrderRepository: Repository<Domain.OrderAggregate.Order>, IOrderRepository
    {
        private readonly OrderDbContext context;

        public OrderRepository(OrderDbContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<List<Domain.OrderAggregate.Order>> GetOrderByUserId(string userId)
        {
            var orders = await context.Orders.Include(p => p.OrderItems).Where(p => p.BuyerId == userId).ToListAsync();

            return orders;
        }
    }
}
