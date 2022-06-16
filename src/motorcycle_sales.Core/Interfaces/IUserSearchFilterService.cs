using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;

namespace motorcycle_sales.Core.Interfaces;

public interface IUserSearchFilterService
{
    Task<Result<List<UserSearchFilter>>> GetUserSearchFiltersByUserId(string UserId);
    Task<Result<List<UserSearchFilter>>> GetMatchingFiltersForAdvertisement(Advertisement advertisement);
}
