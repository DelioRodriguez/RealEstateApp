using AutoMapper;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Repositories.Improvements;
using RealEstateApp.Application.Interfaces.Services.Improvements;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Application.ViewModels.Improvements;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Services.Improvements;

public class ImprovementService : Service<Improvement>, IImprovementService
{
    private readonly IImprovementRepository _improvementRepository;
    private readonly IMapper _mapper;
    
    public ImprovementService(IRepository<Improvement> repository, IImprovementRepository improvementRepository, IMapper mapper) : base(repository)
    {
        _improvementRepository = improvementRepository;
        _mapper = mapper;
    }


    public async Task<List<ImprovementViewModel>> GetImprovementsByPropertyIdAsync(int propertyId)
    {
        return _mapper.Map<List<ImprovementViewModel>>(
            await _improvementRepository.GetImprovementsByPropertyIdAsync(propertyId));
    }
}