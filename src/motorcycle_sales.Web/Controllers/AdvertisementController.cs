using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using motorcycle_sales.Core;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.Core.Interfaces;
using motorcycle_sales.Core.Services;
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

    public AdvertisementController(IAdvertisementService advertisementService
        , IReadRepository<Model> modelRepository
        , IReadRepository<Brand> brandRepository
        , IRepository<Advertisement> advertisementRepository
        , IRepository<UserSearchFilter> userSearchFilterRepository
        , IHostingEnvironment hostingEnvironment)
    {
        _advertisementService = advertisementService;
        _brandRepository = brandRepository;
        _modelRepository = modelRepository;
        _advertisementRepository = advertisementRepository;
        _userSearchFilteRepository = userSearchFilterRepository;
        _hostingEnvironment = hostingEnvironment;
    }

    [Authorize]
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
    public ActionResult Create(CreateAdvertisementViewModel createAdvertisementModel)
    {
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
            Name = createAdvertisementModel.Name,
            ModelId = createAdvertisementModel.ModelId,
            BrandId = createAdvertisementModel.BrandId,
            Modification = createAdvertisementModel.Modification,
            EngineType = createAdvertisementModel.EngineType,
            HorsePower = createAdvertisementModel.HorsePower,
            EngineCapacity = createAdvertisementModel.EngineCapacity,
            TransmissionType = createAdvertisementModel.TransmissionType,
            CoolingSystemType = createAdvertisementModel.CoolingSystemType,
            ProductionYear = createAdvertisementModel.ProductionYear,
            ProductionMonth = createAdvertisementModel.Month,
            Price = createAdvertisementModel.Price,
            Mileage = createAdvertisementModel.Mileage,
            Description = createAdvertisementModel.Description,
            PhotoPath = uniqueFileName,
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        };

        _advertisementRepository.AddAsync(advertisement);

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
            UserId = currentUserId,
            CreateTime = DateTime.Now
        };

        _userSearchFilteRepository.AddAsync(userSearchFilter);

        return Ok();
    }
}
