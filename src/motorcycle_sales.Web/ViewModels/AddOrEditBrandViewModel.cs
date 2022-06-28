using System.ComponentModel;

namespace motorcycle_sales.Web.ViewModels;

public class AddOrEditBrandViewModel
{
    public int? BrandId { get; set; }
    [DisplayName("Brand Name")]
    public string BrandName { get; set; }
}
