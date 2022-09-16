﻿using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Shared.Dtos;
using MediatR;
using System.Collections.Generic;

namespace FreeCourse.Services.Order.Application.Queries
{
    public class GetOrdersByUserIdQuery : IRequest<Response<List<OrderDto>>>
    {
        public string UserId
        {
            get; init;
        }

        public GetOrdersByUserIdQuery(string userId)
        {
            this.UserId = userId;
        }
    }
}