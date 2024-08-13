﻿using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020171.DataLayers
{
    public interface IProductDAL
    {/// <summary>
    /// Tìm kiếm và lấy mặt hàng dưới dạng phân trang
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <param name="searchValue"></param>
    /// <param name="categoryID"></param>
    /// <param name="supplierID"></param>
    /// <param name="minPrice"></param>
    /// <param name="maxPrice"></param>
    /// <returns></returns>
        IList<Product>List(int page=1, int pageSize=0,string searchValue="",
            int categoryID=0,int supplierID=0,decimal minPrice=0,decimal maxPrice=0);
        /// <summary>
        /// Đếm số lượng mặt hàng tìm kiếm được
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="categoryID"></param>
        /// <param name="supplierID"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <returns></returns>
        int Count(string searchValue = "", int categoryID = 0, 
        int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);
        /// <summary>
        /// Lấy thông tin mặt hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        Product? Get(int productID);
        /// <summary>
        /// Bổ sung mặt hàng mới (hàm trả về mã của mặt hàng được bổ sung)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(Product data);
        /// <summary>
        /// Cập nhật thông tin mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Product data);
        /// <summary>
        /// Xoá mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Delete(int productID);
        /// <summary>
        /// Kiếm trả xem mặt hàng có liên quan hay không?
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        bool InUsed(int productID);
        /// <summary>
        /// Lấy danh sách ảnh của mặt hàng (Sắp xếp theo thứ tự của DisplayOrder)
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<ProductPhoto> ListPhotos(int productID);
        /// <summary>
        /// Lấy thông tin 1 ảnh dựa vào ID
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        ProductPhoto GetPhoto(long productID);
        /// <summary>
        /// Bổ sung một ảnh cho mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        long AddPhoto(ProductPhoto data);
        /// <summary>
        /// Cập nhật ảnh của mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool UpdatePhoto(ProductPhoto data);
        /// <summary>
        /// Xoá ảnh mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool DeletePhoto(long productID);
        /// <summary>
        /// Lấy danh sách các thuộc tính của mặt hàng,sắp xếp theo thứ tự của DisplayOrder
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<ProductAttribute> ListAttributes(int productID);
        /// <summary>
        /// Lấy thông tin thuộc tính dựa trên mã thuộc tính
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        ProductAttribute? GetAttribute(long attributeID);
        /// <summary>
        /// Bổ sung thuộc tính cho mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        long AddAttribute(ProductAttribute data);
        /// <summary>
        /// Cập nhật thông tin mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool UpdateAttribute(ProductAttribute data);
        /// <summary>
        /// Xoá thuộc tính 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool DeleteAttribute(long attributeID);
    }
}
