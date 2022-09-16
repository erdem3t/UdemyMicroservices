using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Shared.Controller;
using FreeCourse.Shared.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Controllers
{
    public class OrderController : CustomBaseController
    {
        private readonly IMediator mediatR;
        private readonly ISharedIdentityService sharedIdentityService;

        public OrderController(IMediator mediatR, ISharedIdentityService sharedIdentityService)
        {
            this.mediatR = mediatR;
            this.sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await mediatR.Send(new GetOrdersByUserIdQuery(sharedIdentityService.UserId));

            return CreateActionResult(response);
        }


        [HttpPost]
        public async Task<IActionResult> SaveOrder(CreateOrderCommand order)
        {
            var response = await mediatR.Send(order);

            return CreateActionResult(response);
        }

    }
}
