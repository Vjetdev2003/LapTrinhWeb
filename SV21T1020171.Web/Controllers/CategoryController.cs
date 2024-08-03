using Microsoft.AspNetCore.Mvc;
using SV21T1020171.DomainModels;

namespace SV21T1020171.Web.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
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
    }
}
