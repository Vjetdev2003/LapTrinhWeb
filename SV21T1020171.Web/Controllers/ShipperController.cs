using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;
using SV21T1020171.DomainModels;
using SV21T1020171.DataLayers;

namespace SV21T1020171.Web.Controllers
{
    public class ShipperController : Controller

    {

        public IActionResult Index(int page=1,int pageSize=10)
        {
            
            var model = BusinessLayers.ShipperDataService.ListOfShipper();
            var shippers = ShipperDataService.ListOfShipper();
            int totalShippers = shippers.Count();
            int totalPages = (int)Math.Ceiling((double)totalShippers / pageSize);

            ViewBag.TotalShippers = totalShippers;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id = 0)
        {
            return View();
        }
        [HttpGet, ActionName("Details")]
        public IActionResult Detail(int id)
        {
            var modelSearch = BusinessLayers.ShipperDataService.Detail(id);
            return View("Details", modelSearch);
        }
        public IActionResult Delete(int id)
        {

            var shipper = BusinessLayers.ShipperDataService.Detail(id);
            if (shipper == null)
            {
                TempData["ErrorMessage"] = "Shipper not found.";
                return RedirectToAction("Index");
            }

            return View(shipper);
        }

    }
}
