using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Core.Entities;

public class ApplicationUser : IdentityUser
{
    public virtual List<Advertisement> FavoriteAdvertisements { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
}
