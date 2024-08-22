using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;
using SV21T1020171.DomainModels;
using SV21T1020171.Web.Models;

namespace SV21T1020171.Web.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
       private const int PAGE_SIZE = 9;
        private const string SEARCH_CONDITION = "categories_search"; //Tên biến dùng để lưu trong session


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
            var data = CommonDataService.ListOfCategories(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new CategorySearchResult()
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
            ViewBag.Title = "Bổ sung loại hàng";
            Category category = new Category()
            {
                CategoryId = 0
            };
            return View("Edit", category);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật người giao hàng";
            Category? category = CommonDataService.GetCategory(id);

            if (category == null)
                return RedirectToAction("Index");
            return View(category);
        }
        [HttpPost]
        public IActionResult Save(Category data)
        {
            ViewBag.Title = data.CategoryId == 0 ? "Bổ sung loại hàng" : "Cập nhật thông tin loại hàng";
            if (string.IsNullOrEmpty(data.CategoryName))
                ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng không được để trống");
            data.Description = data.Description ?? "";
            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            //TODO:Ktra dữ liệu đầu vào có hợp lệ hay không
            if (data.CategoryId == 0)
            {
                CommonDataService.AddCategory(data);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateCategory(data);

            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {//Nếu lời gọi là POST Thì ta thực hiện xoá 
            ViewBag.Title = "Xoá thông tin khách hàng";
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            //nếu lời gọi là GET Thì hiển thị khách hàng cần xoá
            var category = CommonDataService.GetCategory(id);
            if (category == null)
                return RedirectToAction("Index");
            ViewBag.AllowDelete = !CommonDataService.InUsedCategory(id);
            return View(category);
        }
        /// Detail
        /// </summary>
        /// <returns></returns>
    }
}
