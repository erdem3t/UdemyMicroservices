using System;
using System.Collections.Generic;
using System.Text;
using FreeCourse.Shared.Events;

namespace FreeCourse.Shared.EventsContract
{
    public interface IOrderCreatedRequestEvent
    {
        public int OrderId { get; set; }

        public string BuyerId { get; set; }

        public List<OrderItemMessage> OrderItems { get; set; }

        public PaymentMessage Payment { get; set; }
    }
}
