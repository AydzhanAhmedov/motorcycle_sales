using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.SharedKernel;

namespace motorcycle_sales.Core.ProjectAggregate.Events;

public class NewAdvertisementCreatedEvent : BaseDomainEvent
{
    public Advertisement Advertisement { get; set; }

    public NewAdvertisementCreatedEvent(Advertisement advertisement)
    {
        Advertisement = advertisement;
    }
}
