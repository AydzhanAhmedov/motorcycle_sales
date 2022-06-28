using Microsoft.AspNetCore.Mvc;

namespace motorcycle_sales.Web.Controllers;
public class AdManageController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
