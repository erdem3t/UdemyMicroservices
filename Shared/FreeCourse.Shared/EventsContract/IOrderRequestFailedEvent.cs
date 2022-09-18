using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.EventsContract
{
    public interface IOrderRequestFailedEvent
    {
        public int OrderId { get; set; }

        public string Reason { get; set; }
    }
}
