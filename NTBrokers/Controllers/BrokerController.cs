using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NTBrokers.Models;
using NTBrokers.Services;

namespace NTBrokers.Controllers
{
    public class BrokerController : Controller
    {
        private BrokerService _brokerService;
        private RealEstateService _realEstateService;
        private ApartmentService _apartmentService;

        public BrokerController(BrokerService brokerService, RealEstateService realEstateService, ApartmentService apartmentService)
        {
            _brokerService = brokerService;
            _realEstateService = realEstateService;
            _apartmentService = apartmentService;
        }
        public IActionResult Index()
        {
            List<BrokerModel> brokers = _brokerService.GetBrokers();
            return View(brokers);
        }

        public IActionResult Create()
        {
            BrokerModel broker = new BrokerModel();
            return View(broker);
        }
        [HttpPost]
        public IActionResult Create(BrokerModel broker)
        {
            _brokerService.AddBroker(broker);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            RealEstateModel model = _realEstateService.GetModelForBrokerDetails(id);
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            _brokerService.DeleteBroker(id);
            _apartmentService.RemoveBrokerFromAll(id);
            return RedirectToAction("Index");
        }
    }
}
