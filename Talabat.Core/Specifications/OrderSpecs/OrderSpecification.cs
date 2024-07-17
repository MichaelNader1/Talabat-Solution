using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.APIs.Specifications;
using UserOrder = Talabat.Core.Order.Order;

namespace Talabat.Core.Specifications.OrderSpecs
{
    public class OrderSpecification:BaseSpecification<UserOrder>
    {

            public OrderSpecification(string email) : base(o => o.BuyerEmail == email)
            {
                Includes.Add(o => o.DeliveryMethod);
                Includes.Add(o => o.Items);
                AddOrderByDesc(o => o.OrderDate);
            }
            public OrderSpecification(string email, int orderId) : base(o => o.BuyerEmail == email && o.Id== orderId)
            {
                Includes.Add(o => o.DeliveryMethod);
                Includes.Add(o => o.Items);
            }
            
    }
}
