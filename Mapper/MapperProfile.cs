using AutoMapper;
using OnlineShoppingAPI.Entites;
using OnlineShoppingAPI.Model.DTO;

namespace OnlineShoppingAPI.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<ProductAddUIDTO, Product>()
                .ForMember(m=>m.Color,mo=>mo.MapFrom(sm=>sm.ColorInfo))
                .ReverseMap();
            CreateMap<Product,ProductNPDTO>().ReverseMap();
            CreateMap<Product,ProductDTO>().ReverseMap();
        }
    }
}
