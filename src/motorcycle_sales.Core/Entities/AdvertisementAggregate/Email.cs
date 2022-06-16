using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace motorcycle_sales.Core.Entities.AdvertisementAggregate;

public class Email
{
    public string Receiver { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
