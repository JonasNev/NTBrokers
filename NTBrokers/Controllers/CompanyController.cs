using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NTBrokers.Models;
using NTBrokers.Services;

namespace NTBrokers.Controllers
{
    public class CompanyController : Controller
    {
        private CompanyService _companyService;
        private RealEstateService _realEstateService;

        public CompanyController(CompanyService companyService, RealEstateService realEstateModel)
        {
            _companyService = companyService;
            _realEstateService = realEstateModel;
        }
        public IActionResult Index()
        {
            RealEstateModel model = _realEstateService.GetModelForIndex();
            return View(model);
        }

        public IActionResult Create()
        {
            RealEstateModel model = _realEstateService.GetModelForCompanyCreate();
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(RealEstateModel model)
        {
            _companyService.AddCompany(model);
            return RedirectToAction("Index");
        }
    }
}
