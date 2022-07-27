using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using motorcycle_sales.Core.Attributes;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.Core.ProjectAggregate.Events;
using motorcycle_sales.Core.Specifications;
using motorcycle_sales.SharedKernel.Interfaces;

namespace motorcycle_sales.Web.Controllers;
public class AdvertisementManageController : Controller
{
    private readonly IRepository<Advertisement> _advertisementRepository;

    public AdvertisementManageController(IRepository<Advertisement> advertisementRepository)
    {
        _advertisementRepository = advertisementRepository;
    }

    [HttpGet]
    public async Task<IActionResult> ListConfirmAsync()
    {
        var spec = new AdvertisementsByStatusSpecification(AdvertisementStatus.Pending);
        var model = (await _advertisementRepository.ListAsync(spec));

        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = "Permission.Advertisement.Approve")]
    public async Task<IActionResult> ChangeStatus(int adId, AdvertisementStatus status)
    {
        var advertisement = await _advertisementRepository.GetByIdAsync(adId);
        if (advertisement == null)
        {
            return NotFound();
        }

        if (advertisement.Status == AdvertisementStatus.Pending && status == AdvertisementStatus.Active)
        {
            // if ad status is changing from Initial to Active then we activate notification event
            // Add event for email notification
            advertisement.Events.Add(new NewAdvertisementCreatedEvent(advertisement));
        }

        advertisement.Status = status;
        await _advertisementRepository.SaveChangesAsync();

        return Json(new { newStatus = advertisement.Status.ToString(), newColor = advertisement.Status.GetColor() }) ;
    }
}
