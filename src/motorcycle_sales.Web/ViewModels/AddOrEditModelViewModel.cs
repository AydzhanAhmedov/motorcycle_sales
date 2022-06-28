using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace motorcycle_sales.Web.ViewModels;

public class AddOrEditModelViewModel
{
    public int ModelId { get; set; }
    [DisplayName("Brand")]
    public int BrandId { get; set; }
    [DisplayName("Model Name")]
    public string ModelName { get; set; }

    public SelectList? Brands { get; set; }
}
