using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.Order_Specs
{
    public class OrderWithPaymentIntentSpecifications :BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpecifications(string? paymentInrentId) : base(o => o.PaymentIntentId == paymentInrentId)
        {

        }
    }
}
