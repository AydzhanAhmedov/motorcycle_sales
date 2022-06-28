using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Core.Specifications;

public class ModelsByBrandIdSpecification : Specification<Model>
{
    public ModelsByBrandIdSpecification(int brandId)
    {
        Query
            .Where(model => model.BrandId == brandId);
    }
}
