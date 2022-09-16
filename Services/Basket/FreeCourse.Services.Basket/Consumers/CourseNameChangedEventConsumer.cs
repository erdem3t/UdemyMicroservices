using FreeCourse.Services.Basket.ServicesContract;
using FreeCourse.Shared.Messages;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Consumers
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly IBasketService basketService;

        public CourseNameChangedEventConsumer(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var basket = await basketService.GetBasket(context.Message.UserId);

            if (basket != null)
            {
                basket.Data.BasketItems.Where(p=>p.CourseId==context.Message.CourseId).ToList().ForEach(basketItem => {

                    basketItem.CourseName = context.Message.UpdatedName;
                });
            }

            await basketService.SaveOrUpdate(basket.Data);

        }
    }
}
