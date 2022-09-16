using FreeCourse.Discount.Dtos;
using FreeCourse.Discount.ServicesContract;
using FreeCourse.Shared.Controller;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FreeCourse.Discount.Controllers
{
    public class DiscountController : CustomBaseController
    {
        private readonly ISharedIdentityService sharedIdentityService;
        private readonly IDiscountService discountService;

        public DiscountController(ISharedIdentityService sharedIdentityService, IDiscountService discountService)
        {
            this.sharedIdentityService = sharedIdentityService;
            this.discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResult(await discountService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResult(await discountService.GetById(id));
        }

        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            return CreateActionResult(await discountService.GetByCodeAndUserId(code, sharedIdentityService.UserId));
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] DiscountDto discount)
        {
            return CreateActionResult(await discountService.Save(discount));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DiscountDto discount)
        {
            return CreateActionResult(await discountService.Update(discount));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResult(await discountService.Delete(id));
        }
    }
}
