using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.SharedKernel.Interfaces;
using motorcycle_sales.Web.ViewModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace motorcycle_sales.Web.Controllers;

public class AdvertisementController : Controller
{
    private readonly IReadRepository<Brand> _brandRepository;
    private readonly IReadRepository<Model> _modelRepository;
    private readonly IRepository<Advertisement> _advertisementRepository;
    private readonly IHostingEnvironment _hostingEnvironment;

    public AdvertisementController(IReadRepository<Model> modelRepository
        , IReadRepository<Brand> brandRepository
        , IRepository<Advertisement> advertisementRepository, IHostingEnvironment hostingEnvironment)
    {

        _brandRepository = brandRepository;
        _modelRepository = modelRepository;
        _advertisementRepository = advertisementRepository;
        _hostingEnvironment = hostingEnvironment;
    }

    [Authorize]
    public async Task<IActionResult> Create()
    {
        CreateAdvertisementModel createAdvertisementModel = new CreateAdvertisementModel();
        List<Brand> brands = await _brandRepository.ListAsync();
        createAdvertisementModel.Brands = new SelectList(brands, "Id", "Name");

        return View(createAdvertisementModel);
    }

    [HttpPost]
    public ActionResult Create(CreateAdvertisementModel createAdvertisementModel)
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
            Price = createAdvertisementModel.Price,
            Mileage = createAdvertisementModel.Mileage,
            Description = createAdvertisementModel.Description,
            PhotoPath = uniqueFileName
        };

        _advertisementRepository.AddAsync(advertisement);

        return RedirectToAction("index", "home");
    }

    public async Task<JsonResult> GetModels(int id)
    {
        // TODO Use specifications 
        List<Model> models = await _modelRepository.ListAsync(); 
        List<Model> modelsFiltered = models.Where(model => (model.BrandId == id)).ToList();

        return Json(new SelectList(modelsFiltered, "Id", "Name"));
    }
}
