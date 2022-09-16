using AutoMapper;
using FreeCourse.Services.Order.Application.Dtos;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Services.Order.Application.RepositoryContract;
using FreeCourse.Shared.Dtos;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDto>>>
    {
        private readonly IOrderRepository repository;
        private readonly IMapper mapper;

        public GetOrdersByUserIdQueryHandler(IMapper mapper,IOrderRepository repository)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async  Task<Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await repository.GetOrderByUserId(request.UserId);

            if (!orders.Any()) {
                return Response<List<OrderDto>>.Success(new List<OrderDto>(), 200);
            }

            return Response<List<OrderDto>>.Success(mapper.Map<List<OrderDto>>(orders),200);
        }
    }
}
