using FreeCourse.Shared.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.EventsContract
{
    public interface IPaymentFailedRequestStock
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
