using Microsoft.AspNetCore.Mvc;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.SharedKernel.Interfaces;

namespace motorcycle_sales.Web.Controllers;
public class SearchFilterController : Controller
{
    private readonly IRepository<UserSearchFilter> _userSearchFilterRepository;

    public SearchFilterController(IRepository<UserSearchFilter> userSearchFilteRepository)
    {
        _userSearchFilterRepository = userSearchFilteRepository;
    }
    public IActionResult Index()
    {


        return View();
    }

    public async Task<IActionResult> SwitchFilterNotifications(int userSearchFilterId)
    {
        var userSearchFilter = await _userSearchFilterRepository.GetByIdAsync(userSearchFilterId);

        if (userSearchFilter == null)
            return NotFound();

        userSearchFilter.NotificationsActive = !userSearchFilter.NotificationsActive;
        await _userSearchFilterRepository.UpdateAsync(userSearchFilter);

        return Ok();
    }

    public async Task<JsonResult> IsFilterNotificationsActive(int userSearchFilterId)
    {
        var userSearchFilter = await _userSearchFilterRepository.GetByIdAsync(userSearchFilterId);

        if (userSearchFilter == null)
            return Json(false);

        return Json(userSearchFilter.NotificationsActive);
    }
}
