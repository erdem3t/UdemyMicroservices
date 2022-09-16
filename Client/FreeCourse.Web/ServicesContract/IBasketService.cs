using FreeCourse.Web.Models.Basket;
using System.Net.Http;
using System.Threading.Tasks;

namespace FreeCourse.Web.ServicesContract
{
    public interface IBasketService
    {
        Task<bool> SaveOrUpdate(BasketViewModel basket);

        Task<BasketViewModel> Get();

        Task<bool> Delete();

        Task AddBasketItem(BasketItemViewModel basketItem);

        Task<bool> RemoveBasketItem(string courseId);

        Task<bool> ApplyDiscount(string discountCode);

        Task<bool> CancelApplyDiscount();
    }
}
