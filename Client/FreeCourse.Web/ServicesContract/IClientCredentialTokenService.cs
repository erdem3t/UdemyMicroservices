using System.Threading.Tasks;

namespace FreeCourse.Web.ServicesContract
{
    public interface IClientCredentialTokenService
    {
        Task<string> GetToken();
    }
}
