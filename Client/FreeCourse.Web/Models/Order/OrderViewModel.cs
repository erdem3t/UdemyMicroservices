using System;
using System.Collections.Generic;

namespace FreeCourse.Web.Models.Order
{
    public class OrderViewModel
    {
        public int Id
        {
            get; set;
        }

        public DateTime CreateDate
        {
            get; set;
        }

        public string BuyerId
        {
            get; set;
        }

        public List<OrderItemViewModel> OrderItems
        {
            get; set;
        }
    }
}
