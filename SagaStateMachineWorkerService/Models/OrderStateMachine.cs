﻿using Automatonymous;

namespace SagaStateMachineWorkerService.Models
{
    public class OrderStateMachine:MassTransitStateMachine<OrderStateInstance>
    {
        public OrderStateMachine()
        {

        }
    }
}
