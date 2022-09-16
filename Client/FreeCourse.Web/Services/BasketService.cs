using FreeCourse.Shared.Dtos;
using FreeCourse.Web.Models.Basket;
using FreeCourse.Web.ServicesContract;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class BasketService : IBasketService
    {
        private readonly IDiscountService discountService;
        private readonly HttpClient httpClient;

        public BasketService(IDiscountService discountService, HttpClient httpClient)
        {
            this.discountService = discountService;
            this.httpClient = httpClient;
        }

        public async Task AddBasketItem(BasketItemViewModel basketItem)
        {
            var basket = await Get();

            if (basket != null)
            {

                if (!basket.BasketItems.Any(p => p.CourseId == basketItem.CourseId))
                {
                    basket.BasketItems.Add(basketItem);
                }
            }
            else
            {
                basket = new BasketViewModel();
                basket.BasketItems.Add(basketItem);
            }

            await SaveOrUpdate(basket);
        }

        public async Task<bool> ApplyDiscount(string discountCode)
        {
            await CancelApplyDiscount();

            var basket = await Get();

            if (basket == null)
                return false;

            var hasDiscount = await discountService.GetDiscount(discountCode);

            if (hasDiscount == null)
                return false;

            basket.ApplyDiscount(hasDiscount.Code, hasDiscount.Rate);

            return await SaveOrUpdate(basket);
        }

        public async Task<bool> CancelApplyDiscount()
        {
            var basket = await Get();

            if (basket == null || basket.DiscountCode == null)
                return false;

            basket.CancelDiscount();

            return await SaveOrUpdate(basket);
        }

        public async Task<bool> Delete()
        {
            var result = await httpClient.DeleteAsync("Basket");

            return result.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> Get()
        {
            var response = await httpClient.GetAsync("Basket");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var basketViewModel = await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();

            return basketViewModel.Data;
        }

        public async Task<bool> RemoveBasketItem(string courseId)
        {
            var basket = await Get();

            if (basket == null)
                return false;

            var deleteBasketItem = basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId);

            if (deleteBasketItem == null)
                return false;

            var result = basket.BasketItems.Remove(deleteBasketItem);

            if (!result)
            {
                return false;
            }

            if (!basket.BasketItems.Any())
            {

                basket.DiscountRate = null;
            }

            return await SaveOrUpdate(basket);
        }

        public async Task<bool> SaveOrUpdate(BasketViewModel basket)
        {
            var response = await httpClient.PostAsJsonAsync("Basket", basket);

            return response.IsSuccessStatusCode;

        }
    }
}
