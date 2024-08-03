using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;
using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;

namespace SV21T1020171.Web.Controllers
{
    public class SupplierController : Controller
    {
      
        public IActionResult Index(int page = 1, int pageSize = 10)
        {
            var model = BusinessLayers.SupplierDataService.ListOfSuppliers();
            var suppliers = SupplierDataService.ListOfSuppliers();
            int totalSuppliers = suppliers.Count();
            int totalPages = (int)Math.Ceiling((double)totalSuppliers / pageSize);

            ViewBag.TotalSuppliers = totalSuppliers;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            return View(model);
        }
     
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
            var provinces = BusinessLayers.SupplierDataService.GetProvinces(); // Lấy danh sách tỉnh thành
            ViewBag.Provinces = provinces; 
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {

            // return RedirectToAction(supplier.Provice);
            Console.WriteLine(supplier.Provice + "In Controller");
            if (ModelState.IsValid)
            {
                SupplierDataService.Create(supplier);
                return RedirectToAction("Index");
            }
            return Create(supplier);
        }
        public IActionResult Edit(int id=0)
        {
            ViewBag.Title = "Cập nhật nhà cung cấp";
            return View();
        }
        [HttpGet,ActionName("Details")]
        
        public IActionResult SupplierDetail(int id)
        {
            var detail = BusinessLayers.SupplierDataService.SupplierDetail(id);
            return View("SupplierDetail", detail);
        }
        public IActionResult Delete(int id)
        {
            var model = SupplierDataService.SupplierDetail(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            SupplierDataService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
       

    }
}
