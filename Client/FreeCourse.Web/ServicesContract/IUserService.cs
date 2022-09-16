using FreeCourse.Web.Models;
using System.Threading.Tasks;

namespace FreeCourse.Web.ServicesContract
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
