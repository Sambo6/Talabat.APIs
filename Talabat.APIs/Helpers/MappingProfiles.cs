using AutoMapper;
using Talabat.APIs.Dto;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product,ProductToReturnDto>()
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());


            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<AddressDto, Core.Entities.Order_Aggregate.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(D => D.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(D => D.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(D => D.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(D => D.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(D => D.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                .ForMember(D => D.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());

        }
	}
}
