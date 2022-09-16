﻿using System.Collections.Generic;

namespace FreeCourse.Web.Models.Order
{
    public class OrderCreateInput
    {
        public OrderCreateInput()
        {
            OrderItems = new List<OrderItemCreateInput>();
        }

        public string BuyerId
        {
            get; set;
        }

        public AddressInput Address
        {
            get; set;
        }

        public List<OrderItemCreateInput> OrderItems
        {
            get; set;
        }
    }
}
