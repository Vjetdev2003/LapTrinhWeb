using Microsoft.AspNetCore.Mvc;
using SV21T1020171.DomainModels;
using SV21T1020171.DataLayers;
using SV21T1020171.BusinessLayers;
using System.Buffers;
using SV21T1020171.Web.Models;

namespace SV21T1020171.Web.Controllers
{
    public class ShipperController : Controller

    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page=1,int pageSize=10,string searchValue="")
        {
            int rowCount = 0;
            var data = CommonDataService.ListofShippers(out rowCount, page, PAGE_SIZE, searchValue ?? "");

            Models.ShipperSearchResult model = new ShipperSearchResult()
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
            ViewBag.AllowDelete = !CommonDataService.IsUsedShipper(id);
            return View(shipper);
        }

    }
}
