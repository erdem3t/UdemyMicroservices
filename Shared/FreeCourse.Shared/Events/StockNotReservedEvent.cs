using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Events
{
    public class StockNotReservedEvent
    {
        public int OrderId { get; set; }

        public string Message  { get; set; }
    }
}
