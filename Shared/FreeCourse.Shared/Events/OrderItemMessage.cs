using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.Events
{
    public class OrderItemMessage
    {
        public string ProductId
        {
            get; set;
        }

        public string ProductName
        {
            get; set;
        }

        public string PictureUrl
        {
            get; set;
        }

        public decimal Price
        {
            get; set;
        }

        public int Count
        {
            get; set;
        }
    }
}
