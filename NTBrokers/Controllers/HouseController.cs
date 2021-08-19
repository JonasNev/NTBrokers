using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTBrokers.Controllers
{
    public class HouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
