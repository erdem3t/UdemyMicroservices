using FreeCourse.Web.Models.Order;
using FreeCourse.Web.ServicesContract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService basketService;
        private readonly IOrderService orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            this.basketService = basketService;
            this.orderService = orderService;
        }

        public async Task<IActionResult> CheckOut()
        {
            var basket = await basketService.Get();
            ViewBag.basket = basket;

            return View(new CheckoutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput input)
        {
            // Asekron İletişim
            var orderStatus = await orderService.SuspendOrder(input);

            if (!orderStatus.IsSuccessful)
            {
                TempData["error"] = orderStatus.Error;

                return RedirectToAction(nameof(CheckOut));
            }

            return RedirectToAction(nameof(SuccessfulCheckout), new
            {
                orderId = new Random().Next(1, 1000)
            });
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {

            ViewBag.orderId = orderId;

            return View();
        }

        public async Task<IActionResult> CheckoutHistory()
        {
            return View(await orderService.GetOrder());
        }
    }
}
