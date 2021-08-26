using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NTBrokers.Models;
using NTBrokers.Services;

namespace NTBrokers.Controllers
{
    public class ApartmentController : Controller
    {

        private RealEstateService _realEstateService;
        private ApartmentService _apartmentService;

        public ApartmentController(RealEstateService realEstateModel, ApartmentService apartmentService)
        {
            _realEstateService = realEstateModel;
            _apartmentService = apartmentService;
        }
        public IActionResult Index()
        {
            RealEstateModel model = _realEstateService.GetModelForIndex();
            return View(model);
        }

        public IActionResult Create()
        {
            RealEstateModel model = _realEstateService.GetModelForApartmentCreate();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(RealEstateModel model)
        {
            _apartmentService.AddApartment(model);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            RealEstateModel model = _realEstateService.GetModelForApartmentEdit(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(RealEstateModel model)
        {
            _apartmentService.UpdateApartment(model);
            return RedirectToAction("Index");
        }
    }
}
