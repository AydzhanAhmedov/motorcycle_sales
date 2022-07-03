using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using motorcycle_sales.Core.Attributes;

namespace motorcycle_sales.Core.Entities.AdvertisementAggregate;

public enum AdvertisementStatus
{
    [Color("#ebae34")]
    Pending,
    [Color("#50a832")]
    Active,
    [Color("#d40f0f")]
    Inactive
}
