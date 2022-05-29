using Ardalis.Specification;
using Microsoft.AspNetCore.Mvc;
using motorcycle_sales.Core;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.SharedKernel.Interfaces;

namespace motorcycle_sales.Web.Controllers;

/// <summary>
/// A sample MVC controller that uses views.
/// Razor Pages provides a better way to manage view-based content, since the behavior, viewmodel, and view are all in one place,
/// rather than spread between 3 different folders in your Web project. Look in /Pages to see examples.
/// See: https://ardalis.com/aspnet-core-razor-pages-%E2%80%93-worth-checking-out/
/// </summary>
public class HomeController : Controller
{
    private readonly IReadRepository<Advertisement> _advertisementRepository;

    public HomeController(IReadRepository<Advertisement> advertisementRepository)
    {
        _advertisementRepository = advertisementRepository;
    }

    public async Task<IActionResult> IndexAsync()
    {
        List<Advertisement> advertisements = await _advertisementRepository.ListAsync(new AdvertisementWithDetailsSpecification());
        return View(advertisements);
    }

    public IActionResult Error()
    {
        return View();
    }
}
