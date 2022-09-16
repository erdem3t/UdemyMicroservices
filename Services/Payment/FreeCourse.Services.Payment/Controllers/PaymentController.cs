using FreeCourse.Services.Payment.Model;
using FreeCourse.Shared.Controller;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Services.Payment.Controllers
{

    public class PaymentController : CustomBaseController
    {
        private readonly ISendEndpointProvider sendEndpointProvider;

        public PaymentController(ISendEndpointProvider sendEndpointProvider)
        {
            this.sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto payment)
        {
            // payment ile ödeme işlemi gerçekleşir

            var sendEndPoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand
            {
                BuyerId = payment.Order.BuyerId,
                Address = new Address
                {
                    District = payment.Order.Address.District,
                    Line = payment.Order.Address.Line,
                    Province = payment.Order.Address.Province,
                    Street = payment.Order.Address.Street,
                    ZipCode = payment.Order.Address.ZipCode,
                },
            };

            payment.Order.OrderItems.ForEach(item =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem
                {
                    PictureUrl = item.PictureUrl,
                    Price = item.Price,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName
                });
            });

            await sendEndPoint.Send(createOrderMessageCommand);

            return CreateActionResult(Shared.Dtos.Response<NoContent>.Success(200));
        }
    }
}
