using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;
using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;

namespace SV21T1020171.Web.Controllers
{
    public class SupplierController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListofSuppliers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            int pageCount = 1;
            pageCount = rowCount / PAGE_SIZE;
            if (rowCount % PAGE_SIZE > 0)
                pageCount += 1;
            ViewBag.Page = page;
            ViewBag.RowCount = rowCount;
            ViewBag.PageCount = pageCount;
            ViewBag.SearchValue = searchValue;

            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhân viên";
            Supplier supplier = new Supplier()
            {
                SupplierID = 0
            };
            return View("Edit",supplier);
        }
      
       
        public IActionResult Edit(int id=0)
        {
            ViewBag.Title = "Cập nhật nhà cung cấp";
            Supplier? supplier = CommonDataService.GetSupplier(id);
                    
            if (supplier == null)
                return RedirectToAction("Index");
            return View(supplier);
        }
        [HttpPost]
        public IActionResult Save(Supplier data)
        {
            //TODO:Ktra dữ liệu đầu vào có hợp lệ hay không
            if (data.SupplierID == 0)
            {
                CommonDataService.AddSupplier(data);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateSupplier(data);

            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {//Nếu lời gọi là POST Thì ta thực hiện xoá 
            ViewBag.Title = "Xoá thông tin khách hàng";
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteSupplier(id);
                return RedirectToAction("Index");
            }
            //nếu lời gọi là GET Thì hiển thị khách hàng cần xoá
            var supplier = CommonDataService.GetSupplier(id);
            if (supplier == null)
                return RedirectToAction("Index");
            ViewBag.AllowDelete = !CommonDataService.IsUsedSupplier(id);
            return View(supplier);
        }

    }
}
