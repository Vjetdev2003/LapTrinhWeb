using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;

namespace SV21T1020171.Web.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index(int page=1,int pageSize=10)
        {
            var model = BusinessLayers.CustomerDataService.ListOfCustomers();
         
            var customers = CustomerDataService.ListOfCustomers();
            int totalCustomers = customers.Count();
            int totalPages = (int)Math.Ceiling((double)totalCustomers / pageSize);

            ViewBag.totalCustomers = totalCustomers;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            return View(model);
        }
        public IActionResult Create() {
            ViewBag.Title = "Bổ sung khách hàng";
            return View("Edit");
        }
        public IActionResult Edit(int id = 0) {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            return View();
        }
        /// <summary>
        /// Xem thông tin chi tiết khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet,ActionName("Details")]
        public IActionResult Detail(int id) {
            var detail = BusinessLayers.CustomerDataService.CustomerDetail(id);
            return View("Details", detail);
           }
            /// <summary>
            /// Xoá Khách hàng
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
        public IActionResult Delete(int id)
        {
            var model = CustomerDataService.CustomerDetail(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            CustomerDataService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
