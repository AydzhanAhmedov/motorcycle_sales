using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using motorcycle_sales.SharedKernel;
using motorcycle_sales.SharedKernel.Interfaces;

namespace motorcycle_sales.Core.Entities.AdvertisementAggregate;

public class Model : BaseEntity, IAggregateRoot
{
    public string Name { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
}
