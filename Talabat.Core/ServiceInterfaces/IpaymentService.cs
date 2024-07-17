using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.ServiceInterfaces
{
    public interface IpaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId);
        Task<Order.Order> UpdatePaymentIntentToSuccessOrFail(string PaymentIntntId, bool flag);
    }
}
