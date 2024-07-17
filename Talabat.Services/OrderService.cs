
using Talabat.Core.ServiceInterfaces;
using Talabat.Core.Order;
using Talabat.Core.RepositoryInterfaces;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.OrderSpecs;



namespace Talabat.Services
{
    public class OrderService : IOrderInterface
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaymentService _paymentService;

        public OrderService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, PaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;

        }


        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress)
        {
            var Basket= await _basketRepository.GetBasketAsync(BasketId);
            var OrderItem = new List<OrderItem>();
            if (Basket?.Items.Count()>0) {
                foreach(var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);
                    var orderItem= new OrderItem(ProductItemOrdered, item.Price ,item.Quantity);
                    OrderItem.Add(orderItem);
                }
            }
            var SubTotal = OrderItem.Sum(OI => OI.Price * OI.Quantity);
            var DeliveryMethod= await _unitOfWork.Repository<DeliveryMethod>().GetAsync(DeliveryMethodId);

            var spec = new OrderWithPaymentIntentSpecification(Basket.PaymentIntentId);
            var ExistOrder= await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            if (ExistOrder!=null)
            {
                await _unitOfWork.Repository<Order>().Delete(ExistOrder);
                Basket = await _paymentService.CreateOrUpdatePaymentIntent(BasketId);
            }

            var Order = new Order(BuyerEmail, ShippingAddress, DeliveryMethod, OrderItem, SubTotal, Basket.PaymentIntentId );
            await _unitOfWork.Repository<Order>().AddAsync(Order);
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return null;
            return Order;
        }

        public async Task<Order?> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int OrderId)
        {
            var spec = new OrderSpecification(BuyerEmail, OrderId);
            var Orders = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            if(Orders == null) return null; 
            return Orders;


        }

        public async Task<IReadOnlyList<Order?>> GetOrderForSpecificUserAsync(string BuyerEmail)
        {
            var spec = new OrderSpecification(BuyerEmail);
            var Orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            var ordersList = Orders.Select(o => (Order?)o).ToList();
            return ordersList;
        }

    }
}
