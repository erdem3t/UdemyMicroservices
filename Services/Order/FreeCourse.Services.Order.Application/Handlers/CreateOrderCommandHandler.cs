using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Services.Order.Application.RepositoryContract;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Shared.Dtos;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly IOrderRepository orderRepository;

        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        
        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Address address = new(request.Address.Province, request.Address.District, request.Address.Street, request.Address.ZipCode, request.Address.Line);

            Domain.OrderAggregate.Order order = new(request.BuyerId, address, OrderStatus.Suspend);

            request.OrderItems.ForEach(x =>
            {
                order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl, x.Count);
            });

            await orderRepository.CreateAsync(order);

            return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = order.Id }, 200);

        }
    }
}
