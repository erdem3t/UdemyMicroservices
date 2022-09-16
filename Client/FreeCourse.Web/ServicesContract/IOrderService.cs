using FreeCourse.Web.Models.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Web.ServicesContract
{
    public interface IOrderService
    {
        Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput);

        Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput);

        Task<List<OrderViewModel>> GetOrder();
    }
}
