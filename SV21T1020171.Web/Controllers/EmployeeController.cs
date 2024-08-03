using Microsoft.AspNetCore.Mvc;


namespace SV21T1020171.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index(int page=1,int pageSize=10)
        {
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Tạo mới nhân viên";
            return View("Edit");
        }
        public IActionResult Edit(int id=0)
        {
            return View();
        }
        public IActionResult Delete(int id) { 
            return View();
        }
    }
}
