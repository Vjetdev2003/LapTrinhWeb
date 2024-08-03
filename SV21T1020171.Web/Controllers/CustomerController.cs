using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;
using SV21T1020171.DomainModels;
using System.Buffers;

namespace SV21T1020171.Web.Controllers
{
    public class CustomerController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page=1,string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListofCustomers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
            int pageCount = 1;
            pageCount = rowCount/PAGE_SIZE;
            if (rowCount % PAGE_SIZE > 0)
                pageCount += 1;
            ViewBag.Page = page;
            ViewBag.RowCount=rowCount;
            ViewBag.PageCount=pageCount;
            ViewBag.SearchValue=searchValue;
            
            return View(data);
        }
        public IActionResult Create() {
            ViewBag.Title = "Tạo mới thông tin khách hàng";

            Customer customer = new Customer()
            {
                CustomerID = 0
            };
            return View("Edit",customer);
        }
        public IActionResult Edit(int id = 0) {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            Customer? customer =CommonDataService.GetCustomer(id);

            if (customer == null)
                return RedirectToAction("Index");
            return View(customer);
        }
        /// <summary>
        /// Xem thông tin chi tiết khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Save(Customer data)
        {
            //TODO:Ktra dữ liệu đầu vào có hợp lệ hay không
            if(data.CustomerID == 0)
            {
                CommonDataService.AddCustomer(data);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateCustomer(data);

            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {//Nếu lời gọi là POST Thì ta thực hiện xoá 
            ViewBag.Title = "Xoá thông tin khách hàng";
            if (Request.Method == "POST") { 
                CommonDataService.DeleteCustomer(id);
                return RedirectToAction("Index");
            }
            //nếu lời gọi là GET Thì hiển thị khách hàng cần xoá
            var customer = CommonDataService.GetCustomer(id);
            if(customer == null)
                return RedirectToAction("Index");
            ViewBag.AllowDelete = CommonDataService.IsUsedCustomer(id);
            return View(customer);
        }
    }
}
