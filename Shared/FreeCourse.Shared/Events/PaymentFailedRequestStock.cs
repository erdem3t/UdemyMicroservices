using FreeCourse.Shared.EventsContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Events
{
    public class PaymentFailedRequestStock : IPaymentFailedRequestStock
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
