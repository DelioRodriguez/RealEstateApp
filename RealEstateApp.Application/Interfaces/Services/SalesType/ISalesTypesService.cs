using RealEstateApp.Application.Interfaces.Services.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Services.SalesType
{
    public interface ISalesTypesService : IService<SaleType>
    {
        Task<int> GetPropertiesCountAsync(int propertyTypeId);
    }
}
