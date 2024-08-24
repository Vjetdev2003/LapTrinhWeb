using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SV21T1020171.BusinessLayers;
using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;
using SV21T1020171.Web.Models;

namespace SV21T1020171.Web.Controllers
{
     [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class SupplierController : Controller
    {
        private const int PAGE_SIZE = 9;
        private const string SEARCH_CONDITION = "suppliers_search"; //Tên biến dùng để lưu trong session


        public IActionResult Index()
        {
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SEARCH_CONDITION);
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }
            return View(input);
        }
        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfSuppliers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new SupplierSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };
            ApplicationContext.SetSessionData(SEARCH_CONDITION, input);
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung nhà cung cấp";
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
            ViewBag.Title = data.SupplierID == 0 ? "Bổ sung nhà cung cấp" : "Cập nhật thông tin nhà cung cấp";

            if (string.IsNullOrEmpty(data.SupplierName))
                ModelState.AddModelError(nameof(data.SupplierName), "Tên nhà cung cấp không được để trống");
            if (string.IsNullOrEmpty(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được để trống");
            if (string.IsNullOrEmpty(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Tỉnh/Thành chưa được chọn");
            if (string.IsNullOrEmpty(data.Email))
                ModelState.AddModelError(nameof(data.Email), "Email không được để trống");
            if (string.IsNullOrEmpty(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Số điện thoại không được để trống");
            if (string.IsNullOrEmpty(data.Address))
                ModelState.AddModelError(nameof(data.Address), "Địa chỉ không được để trống");

            if (!ModelState.IsValid) { 
                return View("Edit",data);
            }
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
            ViewBag.AllowDelete = !CommonDataService.InUsedSupplier(id);
            return View(supplier);
        }

    }
}
