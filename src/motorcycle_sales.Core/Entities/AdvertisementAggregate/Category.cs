using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motorcycle_sales.Core.Entities.AdvertisementAggregate;

public enum Category
{
    Custom,
    Chopper,
    Cross,
    Enduro,
    Cruiser,
    ATV,
    Touring,
    Naked,
    Street,
    [Display(Name = "Sport Tourist")]
    SportTourist,
    Sport,
    Scooter
}
