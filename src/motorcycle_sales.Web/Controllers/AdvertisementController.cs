using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using motorcycle_sales.Core;
using motorcycle_sales.Core.Entities;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.Core.Interfaces;
using motorcycle_sales.Core.ProjectAggregate.Events;
using motorcycle_sales.Core.Services;
using motorcycle_sales.Core.Specifications;
using motorcycle_sales.SharedKernel.Interfaces;
using motorcycle_sales.Web.ViewModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace motorcycle_sales.Web.Controllers;

[Authorize]
public class AdvertisementController : Controller
{
    private readonly IAdvertisementService _advertisementService;
    private readonly IReadRepository<Brand> _brandRepository;
    private readonly IReadRepository<Model> _modelRepository;
    private readonly IRepository<Advertisement> _advertisementRepository;
    private readonly IRepository<UserSearchFilter> _userSearchFilteRepository;
    private readonly IHostingEnvironment _hostingEnvironment;
    private readonly ILogger<AdvertisementController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdvertisementController(IAdvertisementService advertisementService
        , IReadRepository<Model> modelRepository
        , IReadRepository<Brand> brandRepository
        , IRepository<Advertisement> advertisementRepository
        , IRepository<UserSearchFilter> userSearchFilterRepository
        , IHostingEnvironment hostingEnvironment
        , ILogger<AdvertisementController> logger
        , IUserSearchFilterService userSearchFilterService
        , UserManager<ApplicationUser> userManager)
    {
        _advertisementService = advertisementService;
        _brandRepository = brandRepository;
        _modelRepository = modelRepository;
        _advertisementRepository = advertisementRepository;
        _userSearchFilteRepository = userSearchFilterRepository;
        _hostingEnvironment = hostingEnvironment;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<IActionResult> Create()
    {
        CreateAdvertisementViewModel createAdvertisementModel = new CreateAdvertisementViewModel();
        List<Brand> brands = await _brandRepository.ListAsync();
        createAdvertisementModel.Brands = new SelectList(brands, "Id", "Name");
        
        createAdvertisementModel.Years = new SelectList(Enumerable.Range(1950, (DateTime.Now.Year - 1950) + 1), 2000);

        var months = CultureInfo
            .GetCultureInfo("en-GB")
            .DateTimeFormat
            .MonthNames.Select((monthName, Index) => new SelectListItem
            {
                Value = (Index + 1).ToString(),
                Text = monthName
            });

        createAdvertisementModel.Months = new SelectList(months, "Value", "Text", 1);


        return View(createAdvertisementModel);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateAdvertisementViewModel createAdvertisementModel)
    {
        _logger.LogDebug("Testting log controller asdf");

        if (!ModelState.IsValid)
        {
            createAdvertisementModel.Brands = new SelectList(Enumerable.Empty<SelectList>()) ;

            return View(createAdvertisementModel);
        }         


        string uniqueFileName = null;

        if (createAdvertisementModel.Photo != null)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            uniqueFileName = Guid.NewGuid().ToString() + createAdvertisementModel.Photo.FileName; ;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            createAdvertisementModel.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
        }

        // TODO replace with mapper
        Advertisement advertisement = new Advertisement()
        {
            ModelId = createAdvertisementModel.ModelId,
            BrandId = createAdvertisementModel.BrandId,
            Modification = createAdvertisementModel.Modification,
            EngineType = createAdvertisementModel.EngineType,
            HorsePower = createAdvertisementModel.HorsePower,
            EngineCapacity = createAdvertisementModel.EngineCapacity,
            TransmissionType = createAdvertisementModel.TransmissionType,
            CoolingSystemType = createAdvertisementModel.CoolingSystemType,
            Category = createAdvertisementModel.Category,
            ProductionYear = createAdvertisementModel.ProductionYear,
            ProductionMonth = createAdvertisementModel.Month,
            Price = createAdvertisementModel.Price,
            Mileage = createAdvertisementModel.Mileage,
            Description = createAdvertisementModel.Description,
            PhotoPath = uniqueFileName,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            CreationDate = DateTime.Now
        };

        _logger.LogDebug("Inserting new advertisement");

        UserSearchFilterSpecification x = new UserSearchFilterSpecification(advertisement);

        // Add event for email notification
        advertisement.Events.Add(new NewAdvertisementCreatedEvent(advertisement));

        await _advertisementRepository.AddAsync(advertisement);


        return RedirectToAction("index", "home");
    }

    [AllowAnonymous]
    public async Task<JsonResult> GetModels(int id)
    {
        // TODO Use specifications 
        List<Model> models = await _modelRepository.ListAsync(); 
        List<Model> modelsFiltered = models.Where(model => (model.BrandId == id)).ToList();
        //modelsFiltered.Insert(0, new Model
        //{
        //    Name = ""
        //});
        return Json(new SelectList(modelsFiltered, "Id", "Name"));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> View(int id)
    {
        var advertisementByIdSpec = new AdvertisementWithDetailsSpecification(id);
        var advertisement = await _advertisementRepository.GetBySpecAsync(advertisementByIdSpec);

        if (advertisement == null)
            return NotFound();

        return View(advertisement);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> Search()
    {
        SearchAdvertisementViewModel model = new SearchAdvertisementViewModel();

        List<Brand> brands = await _brandRepository.ListAsync();
        //brands.Insert(0, new Brand
        //{
        //    Name = ""
        //});
        model.Brands = new SelectList(brands, "Id", "Name");

        return View(model);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> Search(SearchAdvertisementViewModel model)
    { 
        model.Advertisements = await _advertisementService.GetAdvertisementsByFilter(model.AdvertisementSearchFilter);

        List<Brand> brands = await _brandRepository.ListAsync();
        brands.Insert(0, new Brand
        {
            Name = ""
        });

        model.Brands = new SelectList(brands, "Id", "Name", String.Empty);

        return View(model);
    }


    public async Task<IActionResult> SaveSearchFilter(AdvertisementSearchFilter searchFilter)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var userSearchFilter = new UserSearchFilter()
        {
            AdvertisementSearchFilter = searchFilter,
            FilterName = "Filter ",
            UserId = currentUserId,
            CreateTime = DateTime.Now
        };

        _userSearchFilteRepository.AddAsync(userSearchFilter);

        return Ok();
    }

    public async Task<IActionResult> AddAdvertisementToFavorite(int advertisementId, bool add)
    {
        var advertisement = await _advertisementRepository.GetByIdAsync(advertisementId);
        var user = await _userManager.GetUserAsync(HttpContext.User);

        if (add)
        {
            if (user.FavoriteAdvertisements.FindIndex(ad => ad.Id == advertisementId) == -1)
                user.FavoriteAdvertisements.Add(advertisement);
        }
        else
        {
            int index = user.FavoriteAdvertisements.FindIndex(ad => ad.Id == advertisementId);
            if (index != -1)
                user.FavoriteAdvertisements.RemoveAt(index);
        }

        await _advertisementRepository.SaveChangesAsync();
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return BadRequest();

        return Ok();
    }

    [AllowAnonymous]
    public async Task<JsonResult> IsFavorite(int advertisementId)
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);

        // If no user is logged in
        if (user == null)
            return Json(false);

        int index = user.FavoriteAdvertisements.FindIndex(ad => ad.Id == advertisementId);
        bool found = index != -1;

        return Json(found);
    }

    [HttpGet]
    public async Task<ActionResult> Favorites()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        if (user == null)
            return NotFound();

        // TODO for optimize
        List<Advertisement> advertisements = user.FavoriteAdvertisements;
        return View(advertisements);
    }
}
