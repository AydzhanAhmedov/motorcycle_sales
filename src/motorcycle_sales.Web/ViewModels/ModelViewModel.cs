using Microsoft.AspNetCore.Mvc.Rendering;

namespace motorcycle_sales.Web.ViewModels;

public class ModelViewModel
{
    public int Id { get; set; }
    public string BrandName { get; set; }
    public string ModelName { get; set; }
}
