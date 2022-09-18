using FreeCourse.Services.Stock.Model;
using FreeCourse.Shared.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FreeCourse.Services.Stock.Consumer
{
    public class PaymentFailedRequestEventConsumer : IConsumer<PaymentFailedRequestStock>
    {
        private readonly AppDbContext _context;
        private ILogger<PaymentFailedRequestEventConsumer> _logger;

        public PaymentFailedRequestEventConsumer(AppDbContext context, ILogger<PaymentFailedRequestEventConsumer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailedRequestStock> context)
        {
            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _context.Stocks.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);

                if (stock != null)
                {
                    stock.Count += item.Count;
                    await _context.SaveChangesAsync();
                }
            }

            _logger.LogInformation($"Stock was released");
        }
    }
}
