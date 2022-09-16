using System.Collections.Generic;

namespace FreeCourse.Services.Payment.Model
{
    public class OrderDto
    {
        public OrderDto()
        {
            OrderItems = new List<OrderItemDto>();
        }

        public string BuyerId
        {
            get; set;
        }

        public AdressDto Address
        {
            get; set;
        }

        public List<OrderItemDto> OrderItems
        {
            get; set;
        }
    }
}
