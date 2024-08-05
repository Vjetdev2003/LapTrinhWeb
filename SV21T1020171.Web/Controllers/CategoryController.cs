using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;
using SV21T1020171.DomainModels;

namespace SV21T1020171.Web.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index(int page=1,int pageSize=10)
        {
            var model = BusinessLayers.CategoryDataService.ListofCategory();
            var categories = CategoryDataService.ListofCategory();
            int totalCategories = categories.Count();
            int totalPages = (int)Math.Ceiling((double)totalCategories / pageSize);
          
            ViewBag.totalCategories = totalCategories;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
        /// <summary>
        /// Detail
        /// </summary>
        /// <returns></returns>
        [HttpGet,ActionName("Details")]
        public IActionResult Detail(int id)
        {
            var detail = BusinessLayers.CategoryDataService.Detail(id);
            return View("Details",detail);
        }
        public IActionResult Delete(int id)
        {
            var model = BusinessLayers.CategoryDataService.Detail(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            CategoryDataService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
