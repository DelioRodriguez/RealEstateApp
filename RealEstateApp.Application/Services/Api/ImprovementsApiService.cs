using AutoMapper;
using RealEstateApp.Application.Interfaces.Repositories.Api;
using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Application.Interfaces.Services.Api;
using RealEstateApp.Application.Services.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Services.Api
{
    public class ImprovementsApiService : Service<Improvement>, IImprovementsApiService
    {
        private readonly IImprovementsApiRepository _improvementsApiRepository;
        private readonly IMapper _mapper;
        public ImprovementsApiService(IRepository<Improvement> repository, IImprovementsApiRepository improvementsApiRepository, IMapper mapper) : base(repository) 
        {
            _improvementsApiRepository = improvementsApiRepository;
            _mapper = mapper;
        }


    }
}
