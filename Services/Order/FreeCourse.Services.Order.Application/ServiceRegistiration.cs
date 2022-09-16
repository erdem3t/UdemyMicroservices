using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FreeCourse.Services.Order.Application
{
    public static class ServiceRegistiration
    {
        public static void AddApplicationService(this IServiceCollection collection)
        {
            var asembly = Assembly.GetExecutingAssembly();
            collection.AddAutoMapper(asembly);
            collection.AddMediatR(asembly);
        }
    }
}
