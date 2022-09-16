using FreeCourse.Web.Models;
using FreeCourse.Web.ServicesContract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient httpClient;

        public UserService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser()
        {
            return await httpClient.GetFromJsonAsync<UserViewModel>("/api/User/GetUser"); 
        }
    }
}
