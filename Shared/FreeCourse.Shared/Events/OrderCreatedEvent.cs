using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Events
{
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; }

        public string BuyerID { get; set; }

        public PaymentMessage Payment { get; set; }

        public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>();
    }
}
