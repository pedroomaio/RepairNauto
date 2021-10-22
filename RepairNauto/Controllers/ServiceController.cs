using AutoRepair.Data;
using AutoRepair.Helpers;
using AutoRepair.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRepair.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserHelper _userHelper;

        public ServiceController(
            IServiceRepository serviceRepository, IUserHelper userHelper)
        {
            _serviceRepository = serviceRepository;
            _userHelper = userHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_serviceRepository.GetAll());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var service = await _serviceRepository.GetByIdAsync(id.Value);
            if (service == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            return View(service);
        }


        ////// GET: Products/Create
        //[Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicesViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                var service = _serviceRepository.ToService(model, true, user.Id);

                await _serviceRepository.CreateAsync(service);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        ////// GET: Products/Edit/5
        //[Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var service = await _serviceRepository.GetByIdAsync(id.Value);
            if (service == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
            var model = _serviceRepository.ToServiceViewModel(service, user.Id);
            return View(model);
        }


        ////// POST: Products/Edit/5
        ////// To protect from overposting attacks, enable the specific properties you want to bind to.
        ////// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ServicesViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                    var service = _serviceRepository.ToService(model, false, user.Id);


                    await _serviceRepository.UpdateAsync(service);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _serviceRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        ////// GET: Products/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AutoPieceNotFound");
            }

            var car = await _serviceRepository.GetByIdAsync(id.Value);
            if (car == null)
            {
                return new NotFoundViewResult("AutoPieceNotFound");
            }

            return View(car);
        }

        ////// POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var service = await _serviceRepository.GetByIdAsync(id);

            try
            {
                //throw new Exception("Excepção de Teste");
                await _serviceRepository.DeleteAsync(service);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {

                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{service.ServiceName} provavelmente está a ser usado!!";
                    ViewBag.ErrorMessage = $"{service.ServiceName} não pode ser apagado visto haverem encomendas que o usam.</br></br>" +
                        $"Experimente primeiro apagar todas as encomendas que o estão a usar," +
                        $"e torne novamente a apagá-lo";
                }

                return View("Error");
            }

        }

        public IActionResult AutoPieceNotFound()
        {
            return View();
        }

    }
}
