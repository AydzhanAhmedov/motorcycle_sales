using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using motorcycle_sales.Core.Entities;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Web.ViewModels;
public class ProfileViewModel
{
    public ApplicationUser User { get; set; }
    public List<Advertisement> Advertisements { get; set; }
    public List<UserSearchFilter> UserSearchFilters { get; set; }
}
