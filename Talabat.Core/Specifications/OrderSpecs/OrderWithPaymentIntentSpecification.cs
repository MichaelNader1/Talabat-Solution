using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.APIs.Specifications;

namespace Talabat.Core.Specifications.OrderSpecs
{
    public class OrderWithPaymentIntentSpecification : BaseSpecification<Order.Order>
    {
        public OrderWithPaymentIntentSpecification(string PaymentIntentId):base(o=>o.PaymentIntentId==PaymentIntentId) { }
    }
}
