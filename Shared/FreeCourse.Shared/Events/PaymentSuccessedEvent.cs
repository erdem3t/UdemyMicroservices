using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Events
{
    public class PaymentCompletedEvent
    {
        public int OrderId { get; set; }

        public string BuyerId { get; set; }
    }
}
