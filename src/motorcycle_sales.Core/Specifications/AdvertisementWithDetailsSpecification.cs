using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Core;

public class AdvertisementWithDetailsSpecification : Specification<Advertisement>
{
    public AdvertisementWithDetailsSpecification()
    {
        Query
    .Include(ad => ad.Brand)
    .Include(ad => ad.Model);
    }
}
