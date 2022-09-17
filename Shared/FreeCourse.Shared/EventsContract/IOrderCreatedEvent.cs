using MassTransit;
using System;
using System.Collections.Generic;

namespace FreeCourse.Shared.Events
{
    public interface IOrderCreatedEvent:CorrelatedBy<Guid>
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
