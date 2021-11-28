using AutoMapper;
using System;
using WalletAPIService;
using WalletService.API.ViewModels.Product;

namespace Ordering.gRPC.Mapper
{
    public class WalletProfile : Profile
    {
        public WalletProfile()
        {
            CreateMap<WalletService.Domain.Entities.Product, ProductModel>();
            CreateMap<ProductCreateRequest, WalletService.Domain.Entities.Product>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => String.IsNullOrEmpty(src.Name) ? "N/A" : src.Name));
            CreateMap<ProductUpdateRequest, WalletService.Domain.Entities.Product>()
                .ForMember(dest => dest.Name, act => act.MapFrom(src => String.IsNullOrEmpty(src.Name) ? "N/A" : src.Name));
        }
    }
}
