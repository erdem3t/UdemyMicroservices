using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.EventsContract
{
    public interface IStockNotReservedEvent : CorrelatedBy<Guid>
    {
        public string Reason { get; set; }
    }
}
