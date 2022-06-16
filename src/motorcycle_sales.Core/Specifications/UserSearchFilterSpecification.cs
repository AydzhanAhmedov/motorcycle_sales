using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Core.Specifications;

public class UserSearchFilterSpecification : Specification<UserSearchFilter>
{
    public UserSearchFilterSpecification(string Id)
    {
        Query.Where(filter => filter.UserId == Id);
    }

    public UserSearchFilterSpecification(Advertisement advertisement)
    {
        // Filter value is null or it is maching the advertisement value
        Query
            .Where(filter => (filter.AdvertisementSearchFilter.ModelId == null || filter.AdvertisementSearchFilter.ModelId == advertisement.ModelId))
            .Where(filter => (filter.AdvertisementSearchFilter.BrandId == null || filter.AdvertisementSearchFilter.BrandId == advertisement.BrandId))
            .Where(filter => (filter.AdvertisementSearchFilter.TransmissionType == null || filter.AdvertisementSearchFilter.TransmissionType == advertisement.TransmissionType))
            .Where(filter => (filter.AdvertisementSearchFilter.EngineType == null || filter.AdvertisementSearchFilter.EngineType == advertisement.EngineType))
            .Where(filter => (filter.AdvertisementSearchFilter.CoolingSystemType == null || filter.AdvertisementSearchFilter.CoolingSystemType == advertisement.CoolingSystemType))
            .Where(filter => (filter.AdvertisementSearchFilter.PriceFrom == null || filter.AdvertisementSearchFilter.PriceFrom <= advertisement.Price))
            .Where(filter => (filter.AdvertisementSearchFilter.PriceTo == null || filter.AdvertisementSearchFilter.PriceFrom >= advertisement.Price))
            .Where(filter => (filter.AdvertisementSearchFilter.ProductionYearFrom == null || filter.AdvertisementSearchFilter.ProductionYearFrom <= advertisement.ProductionYear))
            .Where(filter => (filter.AdvertisementSearchFilter.ProductionYearTo == null || filter.AdvertisementSearchFilter.ProductionYearTo >= advertisement.ProductionYear));
    }
}
