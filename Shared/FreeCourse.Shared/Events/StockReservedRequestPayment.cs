﻿using FreeCourse.Shared.EventsContract;
using System;
using System.Collections.Generic;

namespace FreeCourse.Shared.Events
{
    public class StockReservedRequestPayment : IStockReservedRequestPayment
    {
        public StockReservedRequestPayment(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public PaymentMessage Payment { get ; set ; }
      
        public List<OrderItemMessage> OrderItems { get; set ; }

        public Guid CorrelationId { get; }
    }
}