using FreeCourse.Services.Stock.Model;
using FreeCourse.Shared.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Stock.Consumer
{
    public class OrderCreatedEventConsumer : IConsumer<IOrderCreatedEvent>
    {
        private readonly AppDbContext _context;
        private ILogger<OrderCreatedEventConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedEventConsumer(AppDbContext context, ILogger<OrderCreatedEventConsumer> logger, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<IOrderCreatedEvent> context)
        {
            var stockResult = new List<bool>();

            foreach (var item in context.Message.OrderItems)
            {
                stockResult.Add(await _context.Stocks.AnyAsync(p => p.ProductId == item.ProductId && p.Count > item.Count));
            }

            if (stockResult.All(x => x.Equals(true)))
            {
                foreach (var item in context.Message.OrderItems)
                {
                    var stock = await _context.Stocks.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);

                    if (stock != null)
                    {
                        stock.Count -= item.Count;
                    }

                    await _context.SaveChangesAsync();
                }

                _logger.LogInformation($"Stock was reserved for Correlation Id : {context.Message.CorrelationId}");

                var stockReservedEvent = new StockReservedEvent(context.Message.CorrelationId)
                {
                    OrderItems = context.Message.OrderItems,
                };

                await _publishEndpoint.Publish(stockReservedEvent);
            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent(context.Message.CorrelationId)
                {
                    Reason = "Not enought stock"
                });

                _logger.LogInformation($"Stock was not reserved for CorrelationId Id : {context.Message.CorrelationId}");
            }
        }
    }
}
