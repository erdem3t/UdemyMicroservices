using FreeCourse.Services.Order.Application.RepositoryContract;
using FreeCourse.Services.Order.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FreeCourse.Services.Order.Infrastructure
{
    public static class ServiceRegistiration
    {
        public static void AddInfrastructureService(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.AddDbContext<OrderDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), configure =>
                {
                    configure.MigrationsAssembly("FreeCourse.Services.Order.Infrastructure");
                });
            });

            collection.AddScoped<IOrderRepository, OrderRepository>();
            collection.AddScoped<IOrderItemRepository, OrderItemRepository>();
        }
    }
}
