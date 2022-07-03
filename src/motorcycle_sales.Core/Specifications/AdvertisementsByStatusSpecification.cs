using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Core.Specifications;

public class AdvertisementsByStatusSpecification : Specification<Advertisement>
{
    public AdvertisementsByStatusSpecification(AdvertisementStatus status)
    {
        Query
        .Where(ad => ad.Status == status)
        .Include(ad => ad.Brand)
        .Include(ad => ad.Model)
        .Include(ad => ad.User);
    }
}
