using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using motorcycle_sales.SharedKernel.Interfaces;

namespace motorcycle_sales.Core.Entities.AdvertisementAggregate;

public class Brand : BaseEntity, IAggregateRoot
{
    public string Name { get; set; }
    public List<Model> Models { get; set; }
}
