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
                ProductID = 0
            };
            return View("Edit", product);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Chỉnh sửa thông tin mặt hàng";
            var product = ProductDataService.GetProduct(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult SaveProduct(Product product, IFormFile uploadPhoto)
        {

            if (product == null)
            {
                // Xử lý trường hợp product null, ví dụ như trả về lỗi hoặc thông báo
                return View("Error");
            }

            ViewBag.Title = product.ProductID == 0 ? "Bổ sung mặt hàng" : "Chỉnh sửa thông tin mặt hàng";

            if (string.IsNullOrEmpty(product.ProductName))
            {
                ModelState.AddModelError(nameof(product.ProductName), "Tên mặt hàng không được để trống");
            }
            if (string.IsNullOrEmpty(product.ProductDescription))
            {
                ModelState.AddModelError(nameof(product.ProductDescription), "Mô tả mặt hàng không được để trống");
            }
            if (product.CategoryID == 0)
            {
                ModelState.AddModelError(nameof(product.CategoryID), "Vui lòng chọn loại hàng");
            }
            if (product.SupplierID == 0)
            {
                ModelState.AddModelError(nameof(product.SupplierID), "Vui lòng chọn nhà cung cấp");
            }
            if (string.IsNullOrEmpty(product.Unit))
            {
                ModelState.AddModelError(nameof(product.Unit), "Đơn vị tính không được để trống");
            }
            if (product.Price == 0)
            {
                ModelState.AddModelError(nameof(product.Price), "Vui lòng nhập giá mặt hàng hợp lệ");
            }

            if (uploadPhoto == null || uploadPhoto.Length < 1)
            {
                ModelState.AddModelError(nameof(product.Photo), "Hãy chọn ảnh mặt hàng!");
            }

            if (!ModelState.IsValid)
            {
                return View("Edit", product);
            }

            if (uploadPhoto != null && uploadPhoto.Length > 0)
            {
                // Sử dụng form với phương thức POST và enctype="multipart/form-data" để cho phép tải lên tệp.
                // Sử dụng Directory.GetCurrentDirectory() để lấy đường dẫn gốc của ứng dụng
                // và kết hợp với wwwroot/images/employees để xác định thư mục lưu trữ tệp.
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Product");
                if (!string.IsNullOrEmpty(product.Photo))
                {
                    var filePathDel = Path.Combine(uploadsFolder, product.Photo);
                    if (System.IO.File.Exists(filePathDel))
                    {
                        System.IO.File.Delete(filePathDel);
                    }
                }
                // Tạo tên tệp duy nhất bằng cách sử dụng Guid.NewGuid().ToString().
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + uploadPhoto.FileName;
                // Kết hợp đường dẫn thư mục và tên tệp để tạo đường dẫn đầy đủ
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                // Sử dụng FileStream để lưu trữ tệp vào thư mục đích
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(fileStream);
                }
                // Đặt giá trị cho tham số photo là tên của file ảnh
                product.Photo = uniqueFileName;
            }

            if (product.ProductID == 0)
            {
                ProductDataService.AddProduct(product);
            }
            else if (product.ProductID > 0)
            {
                ProductDataService.UpdateProduct(product);
            }

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id = 0)
        {
            var product = ProductDataService.GetProduct(id);
            if (Request.Method == "POST")
            {
                var PhotoPath = product.Photo;
                //Kiểm tra xem chuỗi có rỗng không
                if (!string.IsNullOrEmpty(PhotoPath))
                {
                    //Lấy đường dẫn thư mục lưu tệp
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Product");
                    //Kết hợp lại để tạo một đường dẫn đầy đủ
                    var filePath = Path.Combine(uploadsFolder, PhotoPath);
                    //kiểm tra xem đường dẫn đó có tồn tại file ảnh không
                    if (System.IO.File.Exists(filePath))
                    {
                        //thực hiện xoá ảnh nếu tồn tại
                        System.IO.File.Delete(filePath);
                    }
                }
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            ViewBag.Title = "Xoá Mặt Hàng";
            if (product == null)
                return RedirectToAction("Index");
            ViewBag.AllowDelete = !ProductDataService.InUsedProduct(id);
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
        public IActionResult SavePhoto(ProductPhoto productPhoto, IFormFile uploadPhoto)
        {
            var data = Request.Form;
            productPhoto = new ProductPhoto()
            {
                ProductID = Convert.ToInt32(data["ProductID"]),
                PhotoId = Convert.ToInt32(data["PhotoID"]),
                Photo = data["Photo"],
                Description = data["Description"],
                DisplayOrder = Convert.ToInt32(data["DisplayOrder"]),
                IsHidden = Convert.ToBoolean(data["IsHidden"])
            };
            ViewBag.Title = productPhoto.PhotoId == 0 ? "Bổ sung ảnh cho mặt hàng" : "Thay đổi ảnh mặt hàng";
            if (string.IsNullOrEmpty(productPhoto.Description))
            {
                ModelState.AddModelError(nameof(productPhoto.Description), "Hãy nhập mô tả ảnh mặt hàng!");
            }
            if (productPhoto.DisplayOrder == 0)
            {
                ModelState.AddModelError(nameof(productPhoto.DisplayOrder), "Hãy nhập thứ tự ảnh mặt hàng!");
            }
            if (uploadPhoto == null && string.IsNullOrEmpty(productPhoto.Photo))
            {
                ModelState.AddModelError(nameof(productPhoto.Photo), "Hãy chọn ảnh mặt hàng!");
            }
            if (ModelState.IsValid == false)
            {
                return View("Photo", productPhoto);
            }
            if (uploadPhoto != null && uploadPhoto.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/Product");
                if (!string.IsNullOrEmpty(productPhoto.Photo))
                {
                    var filePathDel = Path.Combine(uploadsFolder, productPhoto.Photo);
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
                productPhoto.Photo = uniqueFileName;
            }
            if (productPhoto.PhotoId == 0)
            {
                ProductDataService.AddProductPhoto(productPhoto);
            }
            if (productPhoto.PhotoId > 0)
            {
                ProductDataService.UpdateProductPhoto(productPhoto);
            }
            return RedirectToAction("Edit", new { id = productPhoto.ProductID });
        }
        [HttpPost]
        public IActionResult Attribute(ProductAttribute attribute)
        {
            var data = Request.Form;
            if (data != null)
            {
                attribute = new ProductAttribute()
                {
                    AttributeID = Convert.ToInt32(data["AttributeID"]),
                    ProductID = Convert.ToInt32(data["ProductID"]),
                    AttributeName = data["AttributeName"],
                    AttributeValue = data["AttributeValue"],
                    DisplayOrder = Convert.ToInt32(data["DisplayOrder"])
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
