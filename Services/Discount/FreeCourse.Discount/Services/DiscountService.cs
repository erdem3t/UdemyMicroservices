using Dapper;
using FreeCourse.Discount.Dtos;
using FreeCourse.Discount.ServicesContract;
using FreeCourse.Shared.Dtos;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Discount.Services
{
    public class DiscountService : IDiscountService
    {
        private readonly IDbConnection dbConnection;

        public DiscountService(IConfiguration configuration)
        {
            this.dbConnection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSql"));
        }

        public async Task<Response<List<DiscountDto>>> GetAll()
        {
            var discounts = await dbConnection.QueryAsync<DiscountDto>("select * from discount");
            return Response<List<DiscountDto>>.Success(discounts.ToList(), 200);
        }

        public async Task<Response<DiscountDto>> GetById(int id)
        {
            var discount = (await dbConnection.QueryAsync<DiscountDto>("select * from discount where id=@id", new
            {
                id
            })).SingleOrDefault();

            if (discount == null)
            {
                return Response<DiscountDto>.Fail("Discount not found", 404);
            }

            return Response<DiscountDto>.Success(discount, 200);
        }

        public async Task<Response<NoContent>> Save(DiscountDto discount)
        {
            var status = await dbConnection.ExecuteAsync("Insert Into discount(userId,rate,code) Values(@UserId,@Rate,@Code)", discount);

            if (status > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("Discount not save", 500);
        }

        public async Task<Response<NoContent>> Update(DiscountDto discount)
        {
            var status = await dbConnection.ExecuteAsync("UPDATE discount SET userId = @UserId, rate = @Rate, code=@Code WHERE id = @Id", discount);

            if (status > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("Discount not found", 404);
        }

        public async Task<Response<NoContent>> Delete(int id)
        {
            var status = await dbConnection.ExecuteAsync("Delete from discount where id=@id", new
            {
                id
            });

            if (status > 0)
            {
                return Response<NoContent>.Success(204);
            }

            return Response<NoContent>.Fail("Discount not found", 404);
        }

        public async Task<Response<DiscountDto>> GetByCodeAndUserId(string code, string userId)
        {
            var discount = (await dbConnection.QueryAsync<DiscountDto>("select * from discount where userId=@userId and code=@code", new
            {
                code,
                userId
            })).FirstOrDefault();

            if (discount == null)
            {
                return Response<DiscountDto>.Fail("Discount not found", 404);

            }

            return Response<DiscountDto>.Success(discount, 200);
        }

    }
}
