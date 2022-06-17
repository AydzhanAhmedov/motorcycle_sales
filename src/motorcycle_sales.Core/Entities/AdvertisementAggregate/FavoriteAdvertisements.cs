using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motorcycle_sales.Core.Entities.AdvertisementAggregate;

// We need this table because in IdentityUser doesnt exists in core
public class FavoriteAdvertisements
{
    public string UserId { get; set; }
    public int AdvertisementId { get; set; }
}
