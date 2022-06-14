using Microsoft.AspNetCore.Mvc.Rendering;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Web.ViewModels;

public class SearchAdvertisementViewModel
{
    public AdvertisementSearchFilter AdvertisementSearchFilter { get; set; }

    public SelectList? Brands { get; set; }
    public SelectList? Years { get; set; }

    public List<Advertisement> Advertisements;

    public SearchAdvertisementViewModel()
    {
        AdvertisementSearchFilter = new AdvertisementSearchFilter();
    }
}
