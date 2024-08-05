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
            var data = CommonDataService.ListofEmployees(out rowCount, page, PAGE_SIZE, searchValue ?? "");
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
            ViewBag.Title = "Tạo mới nhân viên";
            Employee employee = new Employee()
            {
                EmployeeID = 0
            };
            return View("Edit",employee);
        }
      
       
        public IActionResult Edit(int id=0)
        {
            ViewBag.Title = "Cập nhật nhà cung cấp";
            Employee? employee = CommonDataService.GetEmployee(id);
                    
            if (employee == null)
                return RedirectToAction("Index");
            return View(employee);
        }
        [HttpPost]
        public IActionResult Save(Employee data)
        {
            //TODO:Ktra dữ liệu đầu vào có hợp lệ hay không
            if (data.EmployeeID == 0)
            {
                CommonDataService.AddEmployee(data);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateEmployee(data);

            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {//Nếu lời gọi là POST Thì ta thực hiện xoá 
            ViewBag.Title = "Xoá thông tin khách hàng";
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            //nếu lời gọi là GET Thì hiển thị khách hàng cần xoá
            var employee = CommonDataService.GetEmployee(id);
            if (employee == null)
                return RedirectToAction("Index");
            ViewBag.AllowDelete = !CommonDataService.IsUsedEmployee(id);
            return View(employee);
        }

    }
}
