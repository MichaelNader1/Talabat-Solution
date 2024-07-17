using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Order;
using Talabat.Core.RepositoryInterfaces;
using Talabat.Core.ServiceInterfaces;
using Talabat.Core.Specifications.OrderSpecs;
using Talabat.Repository;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Services
{
    public class PaymentService : IpaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketRepository basketRepository, UnitOfWork UnitOfWork, IConfiguration Configuration)
        {
            _basketRepository = basketRepository;
            _unitOfWork = UnitOfWork;
            _configuration = Configuration;
        }

        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            var basket= await  _basketRepository.GetBasketAsync(BasketId);
            if (basket == null) return null;

            if(basket.Items.Count>0)
            {
                foreach(var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    if (item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }
            var SubTotal= basket.Items.Sum(x => x.Price*x.Quantity);
            var ShippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
                ShippingPrice = DeliveryMethod.Cost;
            }

            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            var service = new PaymentIntentService();
            PaymentIntent paymentIntent ;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" },
                };
                paymentIntent = await service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.CientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShippingPrice * 100)
                };
                paymentIntent = await service.UpdateAsync(basket.PaymentIntentId,options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.CientSecret = paymentIntent.ClientSecret;

            }
                await _basketRepository.UpdateBasketAsync(basket); ;
                return basket; 


        }

        public async Task<Order> UpdatePaymentIntentToSuccessOrFail(string PaymentIntntId, bool flag)
        {
            var spec = new OrderWithPaymentIntentSpecification(PaymentIntntId);
            var order= await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            if (flag)
            {
                order.Status = OrderStatus.PaymentRecived;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;

            }
            await _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
