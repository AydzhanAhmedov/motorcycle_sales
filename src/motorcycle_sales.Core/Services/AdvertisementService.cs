using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.Core.Interfaces;
using motorcycle_sales.SharedKernel.Interfaces;

namespace motorcycle_sales.Core.Services;

public class AdvertisementService : IAdvertisementService
{
    private readonly IRepository<Advertisement> _advertisementRepository;

    public AdvertisementService(IRepository<Advertisement> advertisementRepository)
    {
        _advertisementRepository = advertisementRepository;
    }

    public async Task<Result<List<Advertisement>>> GetAdvertisementsByUserId(string userId)
    {
        var specification = new AdvertisementWithDetailsSpecification(userId);
        var advertisements = await _advertisementRepository.ListAsync(specification);

        return new Result<List<Advertisement>>(advertisements);
    }

    async Task<Result<List<Advertisement>>> IAdvertisementService.GetAdvertisementsByFilter(AdvertisementSearchFilter advertisementSearchFilter)
    {
        var specification = new AdvertisementWithDetailsSpecification(advertisementSearchFilter);
        var advertisements = await _advertisementRepository.ListAsync(specification);

        return new Result<List<Advertisement>>(advertisements);
    }
}
