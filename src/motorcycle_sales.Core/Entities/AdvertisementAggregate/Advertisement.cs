using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using motorcycle_sales.SharedKernel;
using motorcycle_sales.SharedKernel.Interfaces;

namespace motorcycle_sales.Core.Entities.AdvertisementAggregate;

public class Advertisement : BaseEntity, IAggregateRoot
{
    public int? ModelId { get; set; }
    public virtual Model Model { get; set; }
    public int BrandId { get; set; }
    public virtual Brand Brand { get; set; }
    public string? Modification { get; set; }
    public EngineType EngineType { get; set; }
    public int HorsePower { get; set; }
    public int EngineCapacity { get; set; }
    public TransmissionType TransmissionType { get; set; }
    public CoolingSystemType CoolingSystemType { get; set; }
    public Category Category { get; set; }
    public int ProductionYear { get; set; }
    public int ProductionMonth { get; set; }
    public double Price { get; set; }
    public int Mileage { get; set; }
    public string? Description { get; set; }
    public string? PhotoPath { get; set; }
    public string UserId { get; set; }
    public DateTime CreationDate { get; set; }
    // Other side of the many to many relation - advertisement can be favorited from many users
    public virtual List<ApplicationUser> FavoritedFromUsers { get; set; }
}
