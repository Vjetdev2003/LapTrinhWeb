using Microsoft.AspNetCore.Mvc;
using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;

namespace SV21T1020171.Web.Controllers
{
    public class SupplierController : Controller
    {
      
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            return View();
        }
     
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            return View();
        }
      
       
        public IActionResult Edit(int id=0)
        {
            ViewBag.Title = "Cập nhật nhà cung cấp";
            return View();
        }
        

    }
}
