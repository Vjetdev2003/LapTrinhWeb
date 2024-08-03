using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;

namespace SV21T1020171.Web.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index(int page=1,int pageSize=10)
        {
            var model = BusinessLayers.EmployeeDataService.ListOfEmployee();
            var employees = EmployeeDataService.ListOfEmployee();
            int totalEmployees = employees.Count();
            int totalPages = (int)Math.Ceiling((double)totalEmployees / pageSize);

            ViewBag.totalCustomers = totalEmployees;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            return View(model);
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
