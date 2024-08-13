using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020171.DataLayers
{
    /// <summary>
    /// Đinh nghĩa  các phép dữ liệu chung
    /// T: Generic
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommonDAL<T>where T : class
    {/// <summary>
    /// Tìm kiếm và lấy dữ liệu dưới dạng phân trang
    /// </summary>
    /// <param name="page">Trang cần hiển thị</param>
    /// <param name="pagesize">Số dòng hiển thị trên mỗi trang(bằng 0 nếu không phân trang)</param>
    /// <param name="searchValue">Giá trị tìm kiếm(chuỗi rỗng nếu không tìm kiếm)</param>
    /// <returns></returns>
        IList<T> List(int page = 1, int pagesize = 10, string searchValue = "");
        /// <summary>
        /// 
        /// Đếm số dòng dữ liệu tìm được
        /// </summary>
        /// <param name="searchValue">Giá trị tìm kiếm ( chuỗi rỗng nếu không tìm được)</param>
        /// <returns></returns>
        int Count(string searchValue = "");

        /// <summary>
        /// Lấy dòng dữ liệu dựa trên mã( id)
        /// </summary>
        /// <param name="id">Mã của dự liệu cần tìm</param>
        /// <returns></returns>
        T? Get(int id);

        /// <summary>
        /// Bổ sung dữ liệu vào bảng .Hàm trả về ID(mã) của dữ liệu được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);

        /// <summary>
        /// Câp nhật dữ liệu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);
        /// <summary>
        /// Xoá 1 dòng dữ liệu dựa vào id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// Kiếm tra xem dòng dữ liệu có mã là id hiện có dữ liệu liên quan 
        /// ở bảng khác hay không?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool InUsed(int id);
        
    }
}
