﻿using Automatonymous;
using FreeCourse.Shared.Events;
using FreeCourse.Shared.EventsContract;
using FreeCourse.Shared.Settings;
using System;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {
        public Event<IOrderCreatedRequestEvent> OrderCreatedRequestEvent { get; set; }

        public Event<IStockReservedEvent> StockReservedEvent { get; set; }

        public Event<IStockNotReservedEvent> StockNotReservedEvent { get; set; }

        public Event<IPaymentCompletedEvent> PaymentCompletedEvent { get; set; }

        public State OrderCreated { get; private set; }

        public State StockReserved { get; private set; }

        public State StockNotReserved { get; private set; }

        public State PaymentCompleted { get; private set; }

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreatedRequestEvent, y => y.CorrelateById<int>(x => x.OrderId, z => z.Message.OrderId).SelectId(context => Guid.NewGuid()));

            Event(() => StockReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Event(() => StockNotReservedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Event(() => PaymentCompletedEvent, x => x.CorrelateById(y => y.Message.CorrelationId));

            Initially(When(OrderCreatedRequestEvent)
                     .Then(context =>
                     {
                         context.Instance.BuyyerId = context.Data.BuyerId;
                         context.Instance.OrderId = context.Data.OrderId;
                         context.Instance.CreatedDate = DateTime.Now;
                         context.Instance.CardName = context.Data.Payment.CardName;
                         context.Instance.CardNumber = context.Data.Payment.CardNumber;
                         context.Instance.CVV = context.Data.Payment.CVV;
                         context.Instance.Expiration = context.Data.Payment.Expiration;
                         context.Instance.TotalPrice = context.Data.Payment.TotalPrice;
                     })
                     .Then(context =>
                     {
                         Console.WriteLine($"OrderCreatedRequestEvent before : {context.Instance}");
                     })
                     .Publish(context => new OrderCreatedEvent(context.Instance.CorrelationId) { OrderItems = context.Data.OrderItems })
                     .TransitionTo(OrderCreated)
                     .Then(context =>
                     {
                         Console.WriteLine($"OrderCreatedRequestEvent after : {context.Instance}");
                     })
                     );

            During(OrderCreated,
                     When(StockReservedEvent)
                     .TransitionTo(StockReserved)
                     .Send(new Uri($"queue:{RabbitMQSettingsConst.PaymentStockReservedEventQueueName}"), context =>
                           new StockReservedRequestPayment(context.Instance.CorrelationId)
                           {
                               OrderItems = context.Data.OrderItems,
                               Payment = new PaymentMessage
                               {
                                   CardName = context.Instance.CardName,
                                   CardNumber = context.Instance.CardNumber,
                                   CVV = context.Instance.CVV,
                                   Expiration = context.Instance.Expiration,
                                   TotalPrice = context.Instance.TotalPrice,
                               },
                               BuyerId = context.Instance.BuyyerId
                           })
                     .Then(context =>
                      {
                          Console.WriteLine($"StockReservedEvent after : {context.Instance}");
                      }),
                     When(StockNotReservedEvent)
                     .TransitionTo(StockNotReserved)
                     .Publish(context => new OrderRequestFailedEvent { OrderId = context.Instance.OrderId, Reason = context.Data.Reason })
                     );

            During(StockReserved,
                     When(PaymentCompletedEvent)
                     .TransitionTo(PaymentCompleted)
                     .Publish(context => new OrderRequestCompletedEvent { OrderId = context.Instance.OrderId })
                     .Then(context =>
                      {
                          Console.WriteLine($"PaymentCompletedEvent after : {context.Instance}");
                      })
                     .Finalize()
                     );
        }
    }
}
