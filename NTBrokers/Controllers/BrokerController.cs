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

        public BrokerController(BrokerService brokerService)
        {
            _brokerService = brokerService;
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

    }
}
