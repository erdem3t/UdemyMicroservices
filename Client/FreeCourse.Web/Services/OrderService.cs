using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Models.Payment;
using FreeCourse.Web.ServicesContract;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPaymentService paymentService;
        private readonly HttpClient httpClient;
        private readonly IBasketService basketService;
        private readonly ISharedIdentityService sharedIdentityService;

        public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            this.paymentService = paymentService;
            this.httpClient = httpClient;
            this.basketService = basketService;
            this.sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await basketService.Get();

            var paymentInput = new PaymentInfoInput
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Cvv = checkoutInfoInput.Cvv,
                Expiration = checkoutInfoInput.Expiration,
                TotalPrice = basket.TotalPrice
            };

            var payment = await paymentService.ReceivePayment(paymentInput);

            if (!payment)
            {
                return new OrderCreatedViewModel
                {
                    Error = "Ödeme Alınamadı",
                    IsSuccessful = false
                };
            }

            var orderCreateInput = new OrderCreateInput
            {
                BuyerId = sharedIdentityService.UserId,
                Address = new AddressInput
                {
                    District = checkoutInfoInput.District,
                    Line = checkoutInfoInput.Line,
                    Province = checkoutInfoInput.Province,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode,
                },
            };

            basket.BasketItems.ForEach(basketItem =>
            {
                orderCreateInput.OrderItems.Add(new OrderItemCreateInput
                {
                    ProductId = basketItem.CourseId,
                    Price = basketItem.GetCurrentPrice,
                    PictureUrl = "",
                    ProductName = basketItem.CourseName
                });
            });

            var response = await httpClient.PostAsJsonAsync("Order", orderCreateInput);

            if (!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel
                {
                    Error = "Sipariş Oluşturulamadı",
                    IsSuccessful = false
                };
            }

            var result = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();
            result.Data.IsSuccessful = true;

            await basketService.Delete();

            return result.Data;
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("Order");

            return response.Data;
        }

        public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var orderCreateInput = new OrderCreateInput
            {
                BuyerId = sharedIdentityService.UserId,
                Address = new AddressInput
                {
                    District = checkoutInfoInput.District,
                    Line = checkoutInfoInput.Line,
                    Province = checkoutInfoInput.Province,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode,
                },
            };

            var basket = await basketService.Get();
            basket.BasketItems.ForEach(basketItem =>
            {
                orderCreateInput.OrderItems.Add(new OrderItemCreateInput
                {
                    ProductId = basketItem.CourseId,
                    Price = basketItem.GetCurrentPrice,
                    PictureUrl = "",
                    ProductName = basketItem.CourseName
                });
            });

            var paymentInput = new PaymentInfoInput
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Cvv = checkoutInfoInput.Cvv,
                Expiration = checkoutInfoInput.Expiration,
                TotalPrice = basket.TotalPrice,
                Order = orderCreateInput
            };

            var payment = await paymentService.ReceivePayment(paymentInput);

            if (!payment)
            {
                return new OrderSuspendViewModel
                {
                    Error = "Ödeme Alınamadı",
                    IsSuccessful = false
                };
            }

            await basketService.Delete();

            return new OrderSuspendViewModel { IsSuccessful = true };
        }
    }
}
