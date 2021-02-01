using JinnDev.Order.Models;

namespace JinnDev.Order.Core
{
    public class PaymentService : IPaymentService
    {
        public bool ProcessPayment(PaymentInfoModel paymentInfo, decimal total) => true;
    }

    public interface IPaymentService
    {
        bool ProcessPayment(PaymentInfoModel paymentInfo, decimal total);
    }
}