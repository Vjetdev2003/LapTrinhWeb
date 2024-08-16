using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;
using SV21T1020171.DomainModels;
using SV21T1020171.Web.Models;
using System.Buffers;

namespace SV21T1020171.Web.Controllers
{
    public class CustomerController : Controller
    {
        private const int PAGE_SIZE = 9;
        private const string SEARCH_CONDITION = "customers_search";
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
            var data = CommonDataService.ListOfCustomers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new CustomerSearchResult()
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

            ViewBag.Title = "Bổ sung khách hàng";
            Customer customer = new Customer()
            {
                CustomerID = 0
            };
            return View("Edit", customer);
        }
        public IActionResult Edit(int id = 0)
        {

            ViewBag.Title = "Cập nhật thông tin khách hàng";
            Customer customer = CommonDataService.GetCustomer(id);
            if (customer == null)
            {
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteCustomer(id);
                return RedirectToAction("Index");
            }
            var customer = CommonDataService.GetCustomer(id);
            if (customer == null)
            {
                RedirectToAction("Index");
            }
            ViewBag.Allow.Delete = !CommonDataService.InUsedCustomer(id);
            return View(customer);
        }
        [HttpPost]
        public IActionResult Save(Customer data)
        {
            ViewBag.Title = data.CustomerID == 0 ? "Bổ sung khách hàng" : "Cập nhật thông tin khách hàng";
            if (string.IsNullOrEmpty(data.CustomerName))
                ModelState.AddModelError(nameof(data.CustomerName), "Tên khách hàng không được để trống");
            if (string.IsNullOrEmpty(data.ContactName))
                ModelState.AddModelError(nameof(data.ContactName), "Tên giao dịch không được để trống");
            if (string.IsNullOrEmpty(data.Province))
                ModelState.AddModelError(nameof(data.Province), "Vui long chọn tỉnh thành");

            data.Phone = data.Phone ?? "";
            data.Email = data.Email ?? "";
            data.Address = data.Address ?? "";

            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            if (data.CustomerID == 0)
            {
                CommonDataService.AddCustomer(data);
            }
            else
            {
                CommonDataService.UpdateCustomer(data);
            }
            return RedirectToAction("Index");
        }
    }
}
