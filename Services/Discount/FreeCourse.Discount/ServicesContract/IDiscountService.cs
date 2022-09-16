using FreeCourse.Discount.Dtos;
using FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Discount.ServicesContract
{
    public interface IDiscountService
    {
        Task<Response<List<DiscountDto>>> GetAll();

        Task<Response<DiscountDto>> GetById(int id);

        Task<Response<NoContent>> Save(DiscountDto discount);

        Task<Response<NoContent>> Update(DiscountDto discount);

        Task<Response<NoContent>> Delete(int id);

        Task<Response<DiscountDto>> GetByCodeAndUserId(string code, string userId);
    }
}
