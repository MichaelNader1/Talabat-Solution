using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using UserAddress= Talabat.Core.Entities.Identity.Adress;
using OrderAddress = Talabat.Core.Order.Address;
using Talabat.Core.Order;

namespace Talabat.APIs.Helper
{
    public class MappingProfile: Profile
    {
        public MappingProfile() {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>()); //MapFrom(s=>$"https://localhost:7224/{s.PictureUrl}")
            CreateMap<Adress, AddressDto>().ReverseMap();

            CreateMap<Order , OrderToReturnDto>()
              .ForMember(d=>d.DeliveryMethod,o=>o.MapFrom(s=>s.DeliveryMethod.ShortName)) 
              .ForMember(d=>d.DeliveryMethodCost, o=>o.MapFrom(s=>s.DeliveryMethod.Cost)) ;

            CreateMap<OrderItem,OrderItemDto>()
              .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
              .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
              .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl ))
              .ForMember(d => d.PictureUrl, o => o.MapFrom< OrderItemPictureUrlResolver>());

            CreateMap<CustomerBasket, CustomerBasketDto>().ReverseMap() ;

            CreateMap<UserAddress,AddressDto>().ReverseMap();
            CreateMap<OrderAddress,AddressDto>().ReverseMap()
                .ForMember(d=>d.FName,o=>o.MapFrom(s=>s.FName))
                .ForMember(d=>d.LName, o=>o.MapFrom(s=>s.LName)) ;
        }
    }
}
