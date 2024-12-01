using RealEstateApp.Application.Interfaces.Repositories.Generic;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Interfaces.Repositories.SalesType;

public interface ISaleTypeRepository : IRepository<SaleType>
{
    Task<int> GetPropertiesCountAsync(int propertyId);
}