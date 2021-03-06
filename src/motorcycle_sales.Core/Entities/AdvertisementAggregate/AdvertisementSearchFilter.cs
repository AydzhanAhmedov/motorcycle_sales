using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using motorcycle_sales.SharedKernel;
using motorcycle_sales.SharedKernel.Interfaces;

namespace motorcycle_sales.Core.Entities.AdvertisementAggregate;

public class AdvertisementSearchFilter : BaseEntity, IAggregateRoot
{
    public int? BrandId { get; set; }
    public int? ModelId { get; set; }
    public EngineType? EngineType { get; set; }
    public TransmissionType? TransmissionType { get; set; }
    public CoolingSystemType? CoolingSystemType { get; set; }
    public Category? Category { get; set; }
    public double? PriceFrom { get; set; }
    public double? PriceTo { get; set; }
    public int? ProductionYearFrom { get; set; }
    public int? ProductionYearTo { get; set; }
}
