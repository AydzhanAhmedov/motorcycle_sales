using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Core;

public class AdvertisementWithDetailsSpecification : Specification<Advertisement>, ISingleResultSpecification
{
    public AdvertisementWithDetailsSpecification()
    {
        Query
    .Include(ad => ad.Brand)
    .Include(ad => ad.Model);
    }

    public AdvertisementWithDetailsSpecification(int id)
    {
        Query
    .Where(ad => ad.Id == id)
    .Include(ad => ad.Brand)
    .Include(ad => ad.Model);
    }

    public AdvertisementWithDetailsSpecification(AdvertisementSearchFilter filter)
    {
        if (filter.BrandId != null && filter.BrandId != 0) 
            Query.Where(ad => ad.BrandId == filter.BrandId);

        if (filter.ModelId != null && filter.ModelId != 0)
            Query.Where(ad => ad.ModelId == filter.ModelId);

        if (filter.EngineType != null)
            Query.Where(ad => ad.EngineType == filter.EngineType);

        if (filter.TransmissionType != null)
            Query.Where(ad => ad.TransmissionType == filter.TransmissionType);

        if (filter.CoolingSystemType != null)
            Query.Where(ad => ad.CoolingSystemType == filter.CoolingSystemType);

        if (filter.PriceFrom != null)
            Query.Where(ad => ad.Price >= filter.PriceFrom);

        if (filter.PriceTo != null)
            Query.Where(ad => ad.Price <= filter.PriceTo);

        if (filter.ProductionYearFrom != null)
            Query.Where(ad => ad.ProductionYear >= filter.ProductionYearFrom);

        if (filter.ProductionYearTo != null)
            Query.Where(ad => ad.ProductionYear <= filter.ProductionYearTo);

        Query.Include(ad => ad.Brand)
            .Include(ad => ad.Model);
    }
}
