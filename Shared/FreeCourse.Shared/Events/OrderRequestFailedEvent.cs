using FreeCourse.Shared.EventsContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Events
{
    public class OrderRequestFailedEvent : IOrderRequestFailedEvent
    {
        public int OrderId { get ; set ; }

        public string Reason { get; set ; }
    }
}
