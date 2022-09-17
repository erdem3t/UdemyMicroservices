using FreeCourse.Shared.Events;
using MassTransit;
using System;
using System.Collections.Generic;

namespace FreeCourse.Shared.EventsContract
{
    public interface IStockReservedRequestPayment : CorrelatedBy<Guid>
    {
        public PaymentMessage Payment { get; set; }

        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
