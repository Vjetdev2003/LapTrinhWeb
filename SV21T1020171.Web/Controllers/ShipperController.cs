using Microsoft.AspNetCore.Mvc;
using SV21T1020171.DomainModels;
using SV21T1020171.DataLayers;

namespace SV21T1020171.Web.Controllers
{
    public class ShipperController : Controller

    {

        public IActionResult Index(int page=1,int pageSize=10)
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id = 0)
        {
            return View();
        }
        public IActionResult Detail(int id)
        {
           
            return View();
        }
        public IActionResult Delete(int id) { 
            return View();
        }

    }
}
