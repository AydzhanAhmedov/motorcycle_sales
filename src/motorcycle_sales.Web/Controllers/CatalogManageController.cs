using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using motorcycle_sales.Core.Entities.AdvertisementAggregate;
using motorcycle_sales.Core.Specifications;
using motorcycle_sales.SharedKernel.Interfaces;
using motorcycle_sales.Web.ViewModels;

namespace motorcycle_sales.Web.Controllers;
public class CatalogManageController : Controller
{
    private readonly IRepository<Brand> _brandRepository;
    private readonly IRepository<Model> _modelRepository;

    public CatalogManageController(IRepository<Brand> brandRepository, IRepository<Model> modelRepository)
    {
        _brandRepository = brandRepository;
        _modelRepository = modelRepository;
    }

    public async Task<IActionResult> ListBrandsAsync()
    {
        List<BrandViewModel> model = new List<BrandViewModel>();

        foreach (var brand in (await _brandRepository.ListAsync()).ToList())
        {
            BrandViewModel brandViewModel = new BrandViewModel()
            {
                Id = brand.Id,
                Name = brand.Name,
                ModelsCount = await _modelRepository.CountAsync(new ModelsByBrandIdSpecification(brand.Id))
            };

            model.Add(brandViewModel);
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> AddOrEditBrand(int brandId = 0)
    {
        if (brandId == 0)
            return View();

        var brand = await _brandRepository.GetByIdAsync(brandId);
        if (brand == null)
            return NotFound();

        var model = new AddOrEditBrandViewModel()
        {
            BrandId = brand.Id,
            BrandName = brand.Name
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrEditBrand([FromForm] AddOrEditBrandViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.BrandId == null || model.BrandId == 0)
            {
                var brand = new Brand()
                {
                    Name = model.BrandName
                };

                var result = await _brandRepository.AddAsync(brand);
            }
            else
            {
                var brand = await _brandRepository.GetByIdAsync(model.BrandId);
                if (brand == null)
                {
                    return NotFound();
                    // Log fail
                }

                brand.Name = model.BrandName;
                await _brandRepository.UpdateAsync(brand);
            }

            return Json(new { isValid = true, html = Url.Action("ListBrands", "CatalogManage", null, "http") });
        }

        return Json(new { isValid = false, html = this.RenderRazorViewToString("AddOrEditBrand", model) });
    }

    public async Task<IActionResult> DeleteBrandAsync(int brandId)
    {
        var brand = await _brandRepository.GetByIdAsync(brandId);
        if (brand == null)
        {
            return NotFound();
        }

        await _brandRepository.DeleteAsync(brand);

        return Ok();
    }

    public async Task<IActionResult> ListModelsAsync()
    {
        List<ModelViewModel> modelView = new List<ModelViewModel>();

        foreach (var model in (await _modelRepository.ListAsync(new ModelByIdSpecification())).ToList())
        {
            ModelViewModel modelViewModel = new ModelViewModel()
            {
                Id = model.Id,
                ModelName = model.Name,
                BrandName = model.Brand.Name
            };

            modelView.Add(modelViewModel);
        }

        return View(modelView);
    }

    [HttpGet]
    public async Task<IActionResult> AddOrEditModel(int modelId = 0)
    {
        var viewModel = new AddOrEditModelViewModel()
        {
            Brands = new SelectList(await _brandRepository.ListAsync(), "Id", "Name", String.Empty)
        };

        if (modelId == 0)
        {
            return View(viewModel);
        }

        var model = await _modelRepository.GetBySpecAsync(new ModelByIdSpecification(modelId));
        if (model == null)
            return NotFound();

        viewModel.ModelId = model.Id;
        viewModel.BrandId = model.Brand.Id;
        viewModel.ModelName = model.Name;

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddOrEditModel([FromForm] AddOrEditModelViewModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.ModelId == null || model.ModelId == 0)
            {
                var newModel = new Model()
                {
                    Name = model.ModelName,
                    BrandId = model.BrandId
                };

                await _modelRepository.AddAsync(newModel);
            }
            else
            {
                var editModel = await _modelRepository.GetByIdAsync(_modelRepository);
                if (editModel == null)
                {
                    return NotFound();
                    // Log fail
                }

                editModel.Name = model.ModelName;
                editModel.BrandId = model.BrandId;
                await _modelRepository.UpdateAsync(editModel);
            }

            return Json(new { isValid = true, html = Url.Action("ListModels", "CatalogManage", null, "http") });
        }

        return Json(new { isValid = false, html = this.RenderRazorViewToString("AddOrEditModel", model) });
    }

    public async Task<IActionResult> DeleteModelAsync(int modelId)
    {
        var model = await _modelRepository.GetByIdAsync(modelId);
        if (model == null)
        {
            return NotFound();
        }

        await _modelRepository.DeleteAsync(model);

        return Ok();
    }
}
