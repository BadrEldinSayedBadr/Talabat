using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;


namespace Talabat.APIs.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(PD => PD.ProductBrand, O => O.MapFrom(P => P.Brand.Name))
                .ForMember(PD => PD.ProductType, O => O.MapFrom(P => P.Type.Name))
                .ForMember(PD => PD.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());
             
            CreateMap<Talabat.Core.Entities.Identity.Address, AddressDto>().ReverseMap(); //OrderAddress For Identity

            CreateMap<AddressDto, Talabat.Core.Entities.Order_Aggregation.OrderAddress>(); //OrderAddress For Order

            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto, BasketItem>();
        }
                
    }
}
