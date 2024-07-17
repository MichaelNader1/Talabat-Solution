using Talabat.Core.Entities;

namespace Talabat.APIs.Dtos
{
    public class CustomerBasketDto
    {
        public String Id { get; set; }
        public List<BasketItem> Items { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string? PaymentIntentId{ get; set; }
        public string? CientSecret { get; set; }
    }
}
