using AdminPanal.Models;
using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.APIs.Helper;
using Talabat.Core.Entities;

namespace AdminPanal.Helpers
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(PV => PV.ProductBrand, O => O.MapFrom(P => P.Brand.Name))
                .ForMember(PV => PV.ProductType, O => O.MapFrom(P => P.Type.Name))
                .ForMember(PV => PV.PictureUrl, O => O.MapFrom(P => P.PictureUrl));



        }
    }
}
