using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Order;

namespace Talabat.Core.ServiceInterfaces
{
    public interface IOrderInterface
    {
        Task<Order.Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress);
        Task <IReadOnlyList<Order.Order?>> GetOrderForSpecificUserAsync(string BuyerEmail);
        Task <Order.Order?> GetOrderByIdForSpecificUserAsync(string BuyerEmail , int OrderId);

    }
}
