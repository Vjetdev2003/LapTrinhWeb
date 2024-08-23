using Microsoft.AspNetCore.Mvc;
using SV21T1020171.DomainModels;
using SV21T1020171.DataLayers;
using SV21T1020171.BusinessLayers;
using System.Buffers;
using SV21T1020171.Web.Models;
using Microsoft.AspNetCore.Authorization;

namespace SV21T1020171.Web.Controllers
{
     [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ShipperController : Controller

    {
        private const int PAGE_SIZE = 9;
        private const string SEARCH_CONDITION = "shippers_search"; //Tên biến dùng để lưu trong session


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
            var data = CommonDataService.ListOfShippers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new ShipperSearchResult()
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
            ViewBag.Title = "Bổ sung người giao hàng";
            Shipper shipper = new Shipper()
            {
                ShipperID = 0
            };
            return View("Edit", shipper);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật người giao hàng";
            Shipper? shipper = CommonDataService.GetShipper(id);

            if (shipper == null)
                return RedirectToAction("Index");
            return View(shipper);
        }
        [HttpPost]
        public IActionResult Save(Shipper data)
        {
            ViewBag.Title = data.ShipperID == 0 ? "Bổ sung người giao hàng" : "Cập nhật thông tin giao hàng";
            if (string.IsNullOrEmpty(data.ShipperName))
                ModelState.AddModelError(nameof(data.ShipperName), "Tên giao hàng không được để trống");
            data.Phone =data.Phone?? "";
            if (!ModelState.IsValid) { 
                return View("Edit",data);
            }

            //TODO:Ktra dữ liệu đầu vào có hợp lệ hay không
            if (data.ShipperID == 0)
            {
                CommonDataService.AddShipper(data);
                return RedirectToAction("Index");
            }
            else
            {
                CommonDataService.UpdateShipper(data);

            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {//Nếu lời gọi là POST Thì ta thực hiện xoá 
            ViewBag.Title = "Xoá thông tin khách hàng";
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }
            //nếu lời gọi là GET Thì hiển thị khách hàng cần xoá
            var shipper = CommonDataService.GetShipper(id);
            if (shipper == null)
                return RedirectToAction("Index");
            ViewBag.AllowDelete = !CommonDataService.InUsedShipper(id);
            return View(shipper);
        }

    }
}
