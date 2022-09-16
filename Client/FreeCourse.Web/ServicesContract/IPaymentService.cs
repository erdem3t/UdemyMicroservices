using FreeCourse.Web.Models.Payment;
using System.Threading.Tasks;

namespace FreeCourse.Web.ServicesContract
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInfoInput payment);
    }
}
