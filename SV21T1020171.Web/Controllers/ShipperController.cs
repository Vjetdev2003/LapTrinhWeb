using Microsoft.AspNetCore.Mvc;
using SV21T1020171.DomainModels;
using SV21T1020171.DataLayers;
using SV21T1020171.BusinessLayers;
using System.Buffers;

namespace SV21T1020171.Web.Controllers
{
    public class ShipperController : Controller

    {
        const int PAGE_SIZE = 20;
        public IActionResult Index(int page=1,int pageSize=10,string searchValue="")
        {
            int rowCount = 0;
            var data = CommonDataService.ListofShippers(out rowCount, page, PAGE_SIZE, searchValue ?? "");
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
                CommonDataService.DeleteCategory(id);
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
