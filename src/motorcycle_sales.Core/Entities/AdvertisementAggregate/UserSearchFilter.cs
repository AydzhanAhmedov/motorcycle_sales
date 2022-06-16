using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using motorcycle_sales.SharedKernel;
using motorcycle_sales.SharedKernel.Interfaces;

namespace motorcycle_sales.Core.Entities.AdvertisementAggregate;

public class UserSearchFilter : BaseEntity, IAggregateRoot
{
    public int AdvertisementSearchFilterId { get; set; }
    public string FilterName { get; set; }
    public virtual AdvertisementSearchFilter AdvertisementSearchFilter { get; set; }
    public string UserId { get; set; }
    public DateTime CreateTime { get; set; }
    public bool NotificationsActive { get; set; }
}
