using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Services.Order.Application.RepositoryContract;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Events;
using Mass = MassTransit;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using FreeCourse.Shared.Settings;
using FreeCourse.Shared.EventsContract;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Mass.ISendEndpointProvider _sendEndpointProvider;

        public CreateOrderCommandHandler(IOrderRepository orderRepository, Mass.ISendEndpointProvider sendEndpointProvider)
        {
            _orderRepository = orderRepository;
            _sendEndpointProvider = sendEndpointProvider;
        }


        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            Address address = new(request.Address.Province, request.Address.District, request.Address.Street, request.Address.ZipCode, request.Address.Line);

            Domain.OrderAggregate.Order order = new(request.BuyerId, address, OrderStatus.Suspend);

            request.OrderItems.ForEach(x =>
            {
                order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl, x.Count);
            });

            await _orderRepository.CreateAsync(order);

            var orderCreatedRequestEvent = new OrderCreatedRequestEvent
            {
                BuyerId = request.BuyerId,
                OrderId = order.Id,
                Payment = new PaymentMessage
                {
                    CardName = request.Payment.CardNumber,
                    CardNumber = request.Payment.CardNumber,
                    CVV = request.Payment.CVV,
                    Expiration = request.Payment.Expiration,
                    TotalPrice = order.TotalPrice()
                },
            };

            request.OrderItems.ForEach(x =>
            {
                orderCreatedRequestEvent.OrderItems.Add(new OrderItemMessage
                {
                    Count = x.Count,
                    ProductId = x.ProductId,
                    Price = x.Price,
                    PictureUrl = x.PictureUrl,
                    ProductName = x.ProductName
                });
            });

            var sendEndPoint =await _sendEndpointProvider.GetSendEndpoint(new System.Uri($"queue:{RabbitMQSettingsConst.OrderQueueName}"));

            await sendEndPoint.Send<IOrderCreatedRequestEvent>(orderCreatedRequestEvent);

            return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = order.Id }, 200);

        }
    }
}
