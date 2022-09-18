using FreeCourse.Shared.EventsContract;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Events
{
    public class PaymentCompletedEvent : IPaymentCompletedEvent
    {
        public PaymentCompletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
    }
}
