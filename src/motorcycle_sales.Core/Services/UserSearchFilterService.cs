using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.Core.Interfaces;
using motorcycle_sales.Core.Specifications;
using motorcycle_sales.SharedKernel.Interfaces;

namespace motorcycle_sales.Core.Services;

internal class UserSearchFilterService : IUserSearchFilterService
{
    private readonly IRepository<UserSearchFilter> _userSearchFilterRepository;

    public UserSearchFilterService(IRepository<UserSearchFilter> userSearchFilterRepository)
    {
        _userSearchFilterRepository = userSearchFilterRepository;
    }

    public async Task<Result<List<UserSearchFilter>>> GetMatchingFiltersForAdvertisement(Advertisement advertisement)
    {
        var specification = new UserSearchFilterSpecification(advertisement);
        var userSearchFilters = await _userSearchFilterRepository.ListAsync(specification);

        return new Result<List<UserSearchFilter>>(userSearchFilters);
    }

    public async Task<Result<List<UserSearchFilter>>> GetUserSearchFiltersByUserId(string UserId)
    {
        var specification = new UserSearchFilterSpecification(UserId);
        var userSearchFilters = await _userSearchFilterRepository.ListAsync(specification);

        return new Result<List<UserSearchFilter>>(userSearchFilters);
    }
}
