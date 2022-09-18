using FreeCourse.Shared.Events;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.EventsContract
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        public string Reason { get; set; }

        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
