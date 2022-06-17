using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Web.ViewModels;

public class CreateAdvertisementViewModel
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Select model")]
    [DisplayName("Model")]
    public int ModelId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Select brand")]
    [DisplayName("Brand")]
    public int BrandId { get; set; }
    public SelectList? Brands { get; set; }
    public int ProductionYear { get; set; }
    public SelectList? Years { get; set; }
    public int Month { get; set; }
    public SelectList? Months { get; set; }
    [DisplayName("Engine Type")]
    public EngineType EngineType { get; set; }
    public string Modification { get; set; }
    public int HorsePower { get; set; }
    public int EngineCapacity { get; set; }
    [DisplayName("Transmission Type")]
    public TransmissionType TransmissionType { get; set; }
    [DisplayName("Cooling System")]
    public CoolingSystemType CoolingSystemType { get; set; }
    public Category Category { get; set; }
    public double Price { get; set; }
    public int Mileage { get; set; }
    public string Description { get; set; }
    public IFormFile Photo { get; set; }
}
