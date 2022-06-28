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
    public AdvertisementWithDetailsSpecification(bool bOnlyActive = true)
    {
        Query
            .Include(ad => ad.Brand)
            .Include(ad => ad.Model);

        if (bOnlyActive)
        {
            Query.Where(ad => ad.Status == AdvertisementStatus.Active);
        }
    }

    /// <summary>
    /// Advertisement by id
    /// </summary>
    /// <param name="Id">Id of advertisement</param>
    /// <param name="bOnlyActive">Should load only active ad</param>
    public AdvertisementWithDetailsSpecification(int Id, bool bOnlyActive = true)
    {
        Query
            .Where(ad => ad.Id == Id)
            .Include(ad => ad.Brand)
            .Include(ad => ad.Model);

        if (bOnlyActive)
        {
            Query.Where(ad => ad.Status == AdvertisementStatus.Active);
        }
    }

    /// <summary>
    /// Advertisements by user id
    /// </summary>
    /// <param name="Id"> User Id of advertisement holder </param>
    /// <param name="bOnlyActive">Should load only active advertisements</param>
    public AdvertisementWithDetailsSpecification(string Id, bool bOnlyActive = true)
    {
        Query
            .Where(ad => ad.UserId == Id)
            .Include(ad => ad.Brand)
            .Include(ad => ad.Model);

        if (bOnlyActive)
        {
            Query.Where(ad => ad.Status == AdvertisementStatus.Active);
        }
    }

    /// <summary>
    /// Advertisements matching filter
    /// </summary>
    /// <param name="filter">filter to search</param>
    /// <param name="bOnlyActive">Should load only active advertisements</param>
    public AdvertisementWithDetailsSpecification(AdvertisementSearchFilter filter, bool bOnlyActive = true)
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

        if (!bOnlyActive)
        {
            Query.Where(ad => ad.Status == AdvertisementStatus.Active);
        }

        Query.Include(ad => ad.Brand)
            .Include(ad => ad.Model);
    }
}
