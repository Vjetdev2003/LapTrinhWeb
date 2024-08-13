using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;
using SV21T1020171.DomainModels;
using SV21T1020171.Web.Models;

namespace SV21T1020171.Web.Controllers
{
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, int pageSize = 10, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCategories(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            Models.CategorySearchResult model = new CategorySearchResult()
            {
                Page = page,
                PageSize = PAGE_SIZE,
                SearchValue = searchValue ?? "",
                RowCount = rowCount,
                Data = data

            };
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
