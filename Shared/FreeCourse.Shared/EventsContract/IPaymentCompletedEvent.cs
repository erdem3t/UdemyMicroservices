using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.EventsContract
{
    public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
    {
    }
}
