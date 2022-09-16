using FreeCourse.Services.Order.Domain.Core;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public class OrderItem : Entity
    {
        public string ProductId
        {
            get; private set;
        }

        public string ProductName
        {
            get; private set;
        }

        public string PictureUrl
        {
            get; private set;
        }

        public decimal Price
        {
            get; private set;
        }

        public int Count
        {
            get; private set;
        }


        public OrderItem()
        {

        }

        public OrderItem(string productId, string productName, string pictureUrl, decimal price, int count)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
            Count = count;
        }

        public void UpdateOrderItem(string productName, string pictureUrl, decimal price, int count)
        {
            ProductName = productName;
            PictureUrl = pictureUrl;
            Price = price;
            Count = count;
        }
    }
}
