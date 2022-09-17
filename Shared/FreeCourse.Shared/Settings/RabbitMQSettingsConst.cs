
namespace FreeCourse.Shared.Settings
{
    public class RabbitMQSettingsConst
    {
        public const string OrderQueueName = "order-queue";

        public const string StockOrderCreatedEventQueueName = "stock-order-created-event-queue";

        public const string PaymentStockReservedEventQueueName = "payment-stock-reserved-event-queue";
    }
}
