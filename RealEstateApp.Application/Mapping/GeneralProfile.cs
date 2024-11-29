using AutoMapper;
using RealEstateApp.Application.Dtos.Users;
using RealEstateApp.Application.ViewModels.Improvements;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Application.ViewModels.Users;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Mapping;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        CreateMap<UserInfo, AgentViewModel>().ReverseMap();
        CreateMap<Improvement, ImprovementViewModel>().ReverseMap();
        
        CreateMap<Property, PropertyListViewModel>()
            .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType.Name))
            .ForMember(dest => dest.SaleTypeName, opt => opt.MapFrom(src => src.SaleType.Name))
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault()!.ImageUrl));

        CreateMap<Property, PropertyDetailViewModel>()
            .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType.Name))
            .ForMember(dest => dest.SaleTypeName, opt => opt.MapFrom(src => src.SaleType.Name))
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(img => img.ImageUrl)));
    }
}