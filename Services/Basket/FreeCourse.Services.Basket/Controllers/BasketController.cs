using FreeCourse.Services.Basket.Dtos;
using FreeCourse.Services.Basket.ServicesContract;
using FreeCourse.Shared.Controller;
using FreeCourse.Shared.Dtos;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Controllers
{
    public class BasketController : CustomBaseController
    {
        private readonly IBasketService basketService;
        private readonly ISharedIdentityService identityService;

        public BasketController(IBasketService basketService, ISharedIdentityService identityService)
        {
            this.basketService = basketService;
            this.identityService = identityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            return CreateActionResult(await basketService.GetBasket(this.identityService.UserId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdateBasket([FromBody] BasketDto basket)
        {
            basket.UserId = this.identityService.UserId;
            return CreateActionResult(await basketService.SaveOrUpdate(basket));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            return CreateActionResult(await basketService.Delete(identityService.UserId));
        }

    }
}
