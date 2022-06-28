using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Core.Specifications;

public class ModelByIdSpecification : Specification<Model>, ISingleResultSpecification
{
    public ModelByIdSpecification()
    {
        Query.Include(model => model.Brand);
    }

    public ModelByIdSpecification(int modelId)
    {
        Query
            .Where(model => model.Id == modelId)
            .Include(model => model.Brand);
    }   
}
