using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRepair.Data;
using AutoRepair.Data.Entities;
using AutoRepair.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vereyon.Web;

namespace AutoRepair.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ModelsController : Controller
    {
        private readonly IModelRepository _modelRepository;
        private readonly IFlashMessage _flashMessage;

        public ModelsController(
            IModelRepository modelRepository,
            IFlashMessage flashMessage)
        {
            _modelRepository = modelRepository;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> DeleteBrand(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _modelRepository.GetBrandWithUserAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            var countryId = await _modelRepository.DeleteBrandAsync(brand);
            return this.RedirectToAction($"Details", new { id = countryId });
        }

        public async Task<IActionResult> EditBrand(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _modelRepository.GetBrandWithUserAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }


        [HttpPost]
        public async Task<IActionResult> EditBrand(Brand brand)
        {
            if (this.ModelState.IsValid)
            {
                var modelId = await _modelRepository.UpdateBrandAsync(brand);
                if (modelId != 0)
                {
                    return this.RedirectToAction($"Details", new { id = modelId });
                }
            }

            return this.View(brand);
        }

        public async Task<IActionResult> AddBrand(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modelRepo = await _modelRepository.GetByIdAsync(id.Value);
            if (modelRepo == null)
            {
                return NotFound();
            }

            var model = new BrandViewModel { ModelId = modelRepo.Id };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddBrand(BrandViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                await _modelRepository.AddBrandAsync(model);
                return RedirectToAction("Details", new { id = model.ModelId });
            }

            return this.View(model);
        }

        public IActionResult Index()
        {
            return View(_modelRepository.GetModelsWithBrands());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _modelRepository.GetModelsWithBrandsAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Model model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _modelRepository.CreateAsync(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    _flashMessage.Danger("This country already exist!");
                }

                return View(model);
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _modelRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Model model)
        {
            if (ModelState.IsValid)
            {
                await _modelRepository.UpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _modelRepository.GetByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound();
            }

            await _modelRepository.DeleteAsync(model);
            return RedirectToAction(nameof(Index));
        }
    }
}

