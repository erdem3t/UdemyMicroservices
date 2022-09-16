using FreeCourse.Web.Models.Payment;
using FreeCourse.Web.ServicesContract;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient httpClient;

        public PaymentService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<bool> ReceivePayment(PaymentInfoInput payment)
        {
            var response = await httpClient.PostAsJsonAsync("Payment", payment);

            return response.IsSuccessStatusCode;
        }
    }
}
