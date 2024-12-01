using AutoMapper;
using RealEstateApp.Application.Dtos.Agents;
using RealEstateApp.Application.Dtos.Improvements;
using RealEstateApp.Application.Dtos.Properties;
using RealEstateApp.Application.Dtos.PropertyTypes;
using RealEstateApp.Application.Dtos.SaleType;
using RealEstateApp.Application.Dtos.Users;
using RealEstateApp.Application.ViewModels.Improvements;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Application.ViewModels.PropertiesType;
using RealEstateApp.Application.ViewModels.SaleType;
using RealEstateApp.Application.ViewModels.Users;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Mapping;

public class GeneralProfile : Profile
{
    public GeneralProfile()
    {
        #region "Web App"
        CreateMap<UserInfo, AgentViewModel>().ReverseMap();
        
        
        CreateMap<Property, PropertyListViewModel>()
            .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType.Name))
            .ForMember(dest => dest.SaleTypeName, opt => opt.MapFrom(src => src.SaleType.Name))
            .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.Images.FirstOrDefault()!.ImageUrl));

        CreateMap<Property, PropertyDetailViewModel>()
            .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType.Name))
            .ForMember(dest => dest.SaleTypeName, opt => opt.MapFrom(src => src.SaleType.Name))
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.Select(img => img.ImageUrl)))
            .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements.Select(imp => imp.Name)));

        #region "Improvements"

        CreateMap<Improvement, ImprovementViewModel>().ReverseMap();
        CreateMap<Improvement, ImprovementsListViewModel>().ReverseMap();
        CreateMap<Improvement, CreateImprovementViewModel>().ReverseMap();
        CreateMap<Improvement, UpdateImprovementViewModel>().ReverseMap();
        CreateMap<Improvement, DeleteImprovementViewModel>().ReverseMap();

        #endregion

        #region SaleType

        CreateMap<SaleType, SaleTypeListViewModel>().ForMember(dest => dest.PropertiesCount, opt => opt.Ignore());
        CreateMap<SaleType, CreateSaleTypeViewModel>().ReverseMap();
        CreateMap<SaleType, UpdateSaleTypeViewModel>().ReverseMap();
        CreateMap<SaleType, DeleteSaleTypeViewModel>().ReverseMap();

        #endregion

        #region PropertyType

        CreateMap<PropertyType, PropertyTypeListViewModel>().ForMember(dest => dest.PropertiesCount, opt => opt.Ignore());
        CreateMap<PropertyType, CreatePropertyTypeViewModel>().ReverseMap();
        CreateMap<PropertyType, EditPropertyTypeViewModel>().ReverseMap();
        CreateMap<PropertyType, DeletePropertyTypeViewModel>().ReverseMap();

        #endregion

        #endregion

        #region Api

        #region Property
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
            .ForMember(dest => dest.SaleType, opt => opt.MapFrom(src => src.SaleType.Name))
            .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements.Select(i => i.Name).ToList()));

        CreateMap<Property, PropertyDetailsDto>()
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
            .ForMember(dest => dest.SaleType, opt => opt.MapFrom(src => src.SaleType.Name))
            .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements.Select(i => i.Name).ToList()));
        #endregion

        #region Agents

        CreateMap<Property, AgentPropertiesDto>()
            .ForMember(dest => dest.PropertyId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.Name))
            .ForMember(dest => dest.SaleType, opt => opt.MapFrom(src => src.SaleType.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size))
            .ForMember(dest => dest.Rooms, opt => opt.MapFrom(src => src.Rooms))
            .ForMember(dest => dest.Bathrooms, opt => opt.MapFrom(src => src.Bathrooms))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.Improvements, opt => opt.MapFrom(src => src.Improvements.Select(i => i.Name).ToList()));

        #endregion

        #region PopertyType

        CreateMap<PropertyType, PropertyTypeDto>().ReverseMap();
        CreateMap<PropertyType, PropertyTypeCreateDto>().ReverseMap();
        CreateMap<PropertyType, PropertyTypeUpdateDto>().ReverseMap();

        #endregion

        #region SaleType

        CreateMap<SaleType, SaleTypeDto>().ReverseMap();
        CreateMap<SaleType, SaleTypeCreateDto>().ReverseMap();
        CreateMap<SaleType, SaleTypeUpdateDto>().ReverseMap();

        #endregion

        #region Improvements

        CreateMap<Improvement, ImprovementDto>().ReverseMap();
        CreateMap<Improvement, ImprovementCreateDto>().ReverseMap();
        CreateMap<Improvement, ImprovementUpdateDto>().ReverseMap();

        #endregion
        #endregion
    }
}