using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Core.Interfaces;

public interface IAdvertisementService
{
    Task<Result<List<Advertisement>>> GetAdvertisementsByFilter(AdvertisementSearchFilter advertisementSearchFilter);
    Task<Result<List<Advertisement>>> GetAdvertisementsByUserId(string userId);
}
