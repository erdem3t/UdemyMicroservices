using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreateDate
        {
            get; private set;
        }

        public Address Address
        {
            get; private set;
        }

        public string BuyerId
        {
            get; private set;
        }

        public OrderStatus? Status { get; set; }

        public string FailMessage { get; set; }


        private readonly List<OrderItem> _orderItems;

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order()
        {

        }

        public Order(string buyerId, Address address, OrderStatus status)
        {
            _orderItems = new List<OrderItem>();
            CreateDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
            Status = status;
        }

        public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl, int count)
        {
            var existProduct = _orderItems.Any(p => p.ProductId == productId);

            if (!existProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price, count);
                _orderItems.Add(newOrderItem);
            }
        }

        public decimal TotalPrice()
        {
            return _orderItems.Sum(p => p.Price * p.Count);
        }
    }


    public enum OrderStatus
    {
        Suspend,
        Success,
        Fail
    }
}
