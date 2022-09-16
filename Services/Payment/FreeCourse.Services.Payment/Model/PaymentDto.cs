﻿namespace FreeCourse.Services.Payment.Model
{
    public class PaymentDto
    {
        public string CardName
        {
            get; set;
        }

        public string CardNumber
        {
            get; set;
        }

        public string Expiration
        {
            get; set;
        }

        public string Cvv
        {
            get; set;
        }

        public decimal TotalPrice
        {
            get; set;
        }

        public OrderDto Order
        {
            get; set;
        }
    }
}