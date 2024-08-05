using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;
using SV21T1020171.DomainModels;

namespace SV21T1020171.Web.Controllers
{
    public class CategoryController : Controller
    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            int rowCount = 0;
            var data = CommonDataService.ListofCategories(out rowCount, page, PAGE_SIZE, searchValue ?? "");
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
            ViewBag.Title = "Tạo mới loại hàng";
            Category category = new Category()
            {
                CategoryId = 0
            };
            return View("Edit", category);
        }


        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật loại hàng";
            Category? category = CommonDataService.GetCategory(id);

            if (category == null)
                return RedirectToAction("Index");
            return View(category);
        }
        [HttpPost]
        public IActionResult Save(Category data)
        {
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
            ViewBag.AllowDelete = !CommonDataService.IsUsedCategory(id);
            return View(category);
        }
        /// <summary>
        /// Detail
        /// </summary>
        /// <returns></returns>
    }
}
