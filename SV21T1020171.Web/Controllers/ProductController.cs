using Microsoft.AspNetCore.Mvc;
using SV21T1020171.BusinessLayers;
using SV21T1020171.DomainModels;
using SV21T1020171.Web.Models;

namespace SV21T1020171.Web.Controllers
{
    public class ProductController : Controller
    {

        int PAGE_SIZE = 20;
        public IActionResult Index(int page = 1, string searchValue = "")
        {
            ViewBag.Title = "Thông tin mặt hàng";
            int id = 0;
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, page, PAGE_SIZE, searchValue);
            ProductSearchResult model = new ProductSearchResult
            {
                Page = page,
                RowCount = rowCount,
                SearchValue = searchValue,
                PageSize = PAGE_SIZE,
                Data = data,

            };

            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Thêm mới sản phẩm";
            Product product = new Product()
            {
                ProductId = 0
            };
            return View("Edit", product);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật sản phẩm";

            Product? product = ProductDataService.GetProduct(id);
            if (product == null)
                return RedirectToAction("Index");

            return View(product);
        }

        [HttpPost]
        public IActionResult Save(Product data, IFormFile uploadPhoto)
        {
            ViewBag.Title = data.ProductId == 0 ? "Bổ sung sản phẩm" : "Cập nhật thông tin sản phẩm";
            if (string.IsNullOrEmpty(data.ProductName))
                ModelState.AddModelError(nameof(data.ProductName), "Tên sản phẩm không được để trống");
            if (string.IsNullOrEmpty(data.Unit))
                ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được để trống");
            if (data.Price <= 0)
            {
                ModelState.AddModelError(nameof(data.Price), "Giá tiền không được để trống hoặc không hợp lệ");
            }
            if (uploadPhoto == null || string.IsNullOrEmpty(data.Photo))
            {
                ModelState.AddModelError(nameof(data.Photo), "Hãy chọn ảnh cho mặt hàng");
            }
            data.IsSelling = data.IsSelling;
            data.ProductDescription = data.ProductDescription ?? "";
            data.SupplierID = data.SupplierID;
            data.CategoryID = data.CategoryID;


            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }
            if (uploadPhoto != null && uploadPhoto.Length > 0)
            {
                var fileName = Path.GetFileName(uploadPhoto.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Product", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            //TODO:Ktra dữ liệu đầu vào có hợp lệ hay không
            if (data.ProductId == 0)
            {
                ProductDataService.AddProduct(data);
                return RedirectToAction("Index");
            }
            else
            {
                ProductDataService.UpdateProduct(data);

            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Product data, int productId = 0)
        {
            ViewBag.Title = "Xoá thông tin sản phẩm";
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(productId);
                return RedirectToAction("Index");
            }
            //nếu lời gọi là GET Thì hiển thị khách hàng cần xoá
            var product = ProductDataService.GetProduct(productId);
            if (product == null)
                return RedirectToAction("Index");
            ViewBag.AllowDelete = !ProductDataService.InUsedProduct(productId);
            return View(product);
        }
       
        public IActionResult Photo(int id , string method , long photoId = 0)
        {

            switch (method)
            {
                case "add":
                    {
                        ViewBag.Title = "Bổ sung ảnh cho mặt hàng";
                        ProductPhoto productPhoTo = new ProductPhoto()
                        {
                            ProductID = id,
                            PhotoId = 0
                        };
                        return View(productPhoTo);
                    }
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh mặt hàng";
                    ProductPhoto productPhoto = ProductDataService.GetProductPhoto(photoId);
                    return View(productPhoto);
                case "delete":
                    {
                        var photoPath = ProductDataService.GetProductPhoto(photoId)?.Photo;
                        if (!string.IsNullOrEmpty(photoPath))
                        {
                            //Lấy đường dẫn thư mục lưu tệp
                            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Product");
                            //Kết hợp lại để tạo một đường dẫn đầy đủ
                            var filePath = Path.Combine(uploadsFolder, photoPath);
                            //kiểm tra xem đường dẫn đó có tồn tại file ảnh không
                            if (System.IO.File.Exists(filePath))
                            {
                                //thực hiện xoá ảnh nếu tồn tại
                                System.IO.File.Delete(filePath);
                            }
                        }
                        ProductDataService.DeleteProductPhoto(photoId);
                        return RedirectToAction("Edit", new { id = id });
                    }
                default:
                    return RedirectToAction("Index");
            }


        }
        [HttpPost]
        public IActionResult Photo(ProductPhoto photo, IFormFile uploadPhoto)
        {
            var formData = Request.Form;
            photo = new ProductPhoto()
            {
                ProductID = Convert.ToInt32(formData["ProductID"]),
                PhotoId = Convert.ToInt32(formData["PhotoID"]),
                Photo = formData["Photo"],
                Description = formData["Description"],
                DisplayOrder = Convert.ToInt32(formData["DisplayOrder"]),
                IsHidden = Convert.ToBoolean(formData["IsHidden"])
            };
            ViewBag.Title = photo.PhotoId == 0 ? "Bổ sung ảnh cho mặt hàng" : "Thay đổi ảnh mặt hàng";
            if (string.IsNullOrEmpty(photo.Description))
            {
                ModelState.AddModelError(nameof(photo.Description), "Hãy nhập mô tả ảnh mặt hàng!");
            }
            if (photo.DisplayOrder == 0)
            {
                ModelState.AddModelError(nameof(photo.DisplayOrder), "Hãy nhập thứ tự ảnh mặt hàng!");
            }
            if (uploadPhoto == null && string.IsNullOrEmpty(photo.Photo))
            {
                ModelState.AddModelError(nameof(photo.Photo), "Hãy chọn ảnh mặt hàng!");
            }
            if (ModelState.IsValid == false)
            {
                return View("Photo", photo);
            }
            if (uploadPhoto != null && uploadPhoto.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Product");
                if (!string.IsNullOrEmpty(photo.Photo))
                {
                    var filePathDel = Path.Combine(uploadsFolder, photo.Photo);
                    if (System.IO.File.Exists(filePathDel))
                    {
                        System.IO.File.Delete(filePathDel);
                    }
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadPhoto.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(fileStream);
                }
                photo.Photo = uniqueFileName;
            }
            if (photo.PhotoId == 0)
            {
                ProductDataService.AddProductPhoto(photo);
            }
            if (photo.PhotoId > 0)
            {
                ProductDataService.UpdateProductPhoto(photo);
            }
            return RedirectToAction("Edit", new { id = photo.ProductID });
        }
        [HttpPost]
        public IActionResult Attribute(ProductAttribute attribute)
        {
            var formData = Request.Form;
            if (formData != null)
            {
                attribute = new ProductAttribute()
                {
                    AttributeID = Convert.ToInt32(formData["AttributeID"]),
                    ProductID = Convert.ToInt32(formData["ProductID"]),
                    AttributeName = formData["AttributeName"],
                    AttributeValue = formData["AttributeValue"],
                    DisplayOrder = Convert.ToInt32(formData["DisplayOrder"])
                };
            }
            if (string.IsNullOrEmpty(attribute.AttributeName))
            {
                ModelState.AddModelError(nameof(attribute.AttributeName), "Hãy nhập tên thuộc tính mặt hàng!");
            }
            if (string.IsNullOrEmpty(attribute.AttributeValue))
            {
                ModelState.AddModelError(nameof(attribute.AttributeValue), "Hãy nhập giá trị thuộc tính mặt hàng!");
            }
            if (attribute.DisplayOrder == 0)
            {
                ModelState.AddModelError(nameof(attribute.DisplayOrder), "Hãy nhập thứ tự thuộc tính mặt hàng!");
            }
            if (ModelState.IsValid == false)
            {
                return View("Attribute", attribute);
            }
            if (attribute.AttributeID == 0)
            {
                ProductDataService.AddAttribute(attribute);
            }
            if (attribute.AttributeID > 0)
            {
                ProductDataService.UpdateAttribute(attribute);
            }
            return RedirectToAction("Edit", new { id = attribute.ProductID });
        }
        public IActionResult Attribute(int id = 0, string method = "", long attributeId = 0)
        {
            switch (method)
            {
                case "add":
                    {
                        ViewBag.Title = "Bổ sung thuộc tính cho mặt hàng";
                        ProductAttribute attribute = new ProductAttribute()
                        {
                            ProductID = id,
                            AttributeID = 0
                        };
                        return View(attribute);
                    }
                case "edit":
                    {
                        ViewBag.Title = "Thay đổi thuộc tính cho mặt hàng";
                        ProductAttribute attribute = ProductDataService.GetAttribute(attributeId);
                        return View(attribute);
                    }
                case "delete":
                    {
                        ProductDataService.DeleteAttribute(attributeId);
                        return RedirectToAction("Edit", new { id = id });
                    }
                default:
                    return RedirectToAction("Index");
            }
        }
    }
}
