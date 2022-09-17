using FreeCourse.Shared.Events;
using MassTransit;
using System;
using System.Collections.Generic;

namespace FreeCourse.Shared.EventsContract
{
    public interface IStockReservedEvent : CorrelatedBy<Guid>
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
