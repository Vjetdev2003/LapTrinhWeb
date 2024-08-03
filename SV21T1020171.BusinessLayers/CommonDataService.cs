using Azure.Core;
using SV21T1020171.DomainModels;
using SV21T1020171.DataLayers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020171.BusinessLayers
{
    public class CommonDataService
    {
        static readonly ICommonDAL<Province> provinceDB;
        static readonly ICommonDAL<Customer> customerDB;


        static CommonDataService()
        {
            provinceDB = new DataLayers.SQLServer.ProvinceDAL(Configuration.ConnectionString);
            customerDB = new DataLayers.SQLServer.CustomerDAL(Configuration.ConnectionString);

        }
        public static List<Province> ListofProvinces() { 
            return provinceDB.List().ToList();
        }
        /// <summary>
        /// Danh sách khách hàng,( tìm kiếm ,  phân trang)
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListofCustomers(out int rowCount, int page = 1, int pageSize = 0,string searchValue="") {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
        public static List<Customer> ListofCustomers(string searchValue = "")
        {
            return customerDB.List(1,0,searchValue).ToList();
        }
        public static int Count(string searchValue = "")
        {
            return customerDB.Count();
        }

        /// <summary>
        /// Lấy thông tin của 1 khách hàng dựa vào mã khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer? GetCustomer(int id) {
            if(id <= 0)
                return null;
            return customerDB.Get(id);
        }
        /// <summary>
        /// Bổ sung 1 khách hàng mới .Hàm trả về id của khách hàng được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer data) {
         
            return customerDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin của khách hàng 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }
        /// <summary>
        /// Xoá khách hàng dựa vào id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int id) {
            return customerDB.Delete(id);
        }

        /// <summary>
        /// kiểm tra mã khách hàng có dữ liệu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCustomer(int id) { 
         return customerDB.InUsed(id);
        }
    }
}
//Lớp static là gì 
//Constructor trong lớp static có đặc điểm gì ? Dc gọi khi nào
