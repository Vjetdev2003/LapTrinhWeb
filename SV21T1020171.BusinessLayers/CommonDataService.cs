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
        static readonly ICommonDAL<Product> productDB;
        static readonly ICommonDAL<Supplier> supplierDB;
        static readonly ICommonDAL<Category> categoryDB;
        static readonly ICommonDAL<Shipper> shipperDB;
        static readonly ICommonDAL<Employee> employeeDB;



        static CommonDataService()
        {
            provinceDB = new DataLayers.SQLServer.ProvinceDAL(Configuration.ConnectionString);
            customerDB = new DataLayers.SQLServer.CustomerDAL(Configuration.ConnectionString);
            productDB = new DataLayers.SQLServer.ProductDAL(Configuration.ConnectionString);
            supplierDB = new DataLayers.SQLServer.SupplierDAL(Configuration.ConnectionString);
            categoryDB = new DataLayers.SQLServer.CategoryDAL(Configuration.ConnectionString);
            shipperDB =new DataLayers.SQLServer.ShipperDAL(Configuration.ConnectionString);
            employeeDB=new DataLayers.SQLServer.EmployeeDAL(Configuration.ConnectionString);



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
        public static List<Customer> ListofCustomer(string searchValue = "")
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
         return customerDB.IsUsed(id);
        }
        /// <summary>
        /// Danh sách sản phẩm
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Product>ListofProducts(out int rowCount , int page=1,int pageSize =0,string searchValue= "")
        {
            rowCount=productDB.Count(searchValue);
            return productDB.List(page, pageSize, searchValue).ToList();
        }

        public static List<Supplier>ListofSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize,searchValue).ToList();
        }
        public static List<Supplier>ListofSupplier (string searchValue = "")
        {
            return supplierDB.List(1, 0, searchValue).ToList();
        }
        public static Supplier? GetSupplier(int id) {
            if (id <= 0)
                return null;
            return supplierDB.Get(id);
        }
      
        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }
        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }
        public static bool DeleteSupplier(int id)
        {
            return supplierDB.Delete(id);
        }
        public static bool IsUsedSupplier(int id)
        {
            return supplierDB.IsUsed(id);
        }

        /// <summary>
        /// Category
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListofCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }
        public static List<Category> ListofCategory(string searchValue = "")
        {
            return categoryDB.List(1, 0, searchValue).ToList();
        }
        public static Category? GetCategory(int id)
        {
            if (id <= 0)
                return null;
            return categoryDB.Get(id);
        }

        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }
        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }
        public static bool DeleteCategory(int id)
        {
            return categoryDB.Delete(id);
        }
        public static bool IsUsedCategory(int id)
        {
            return categoryDB.IsUsed(id);
        }
        /// <summary>
        /// Shipper
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Shipper> ListofShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }
        public static List<Shipper> ListofShipper(string searchValue = "")
        {
            return shipperDB.List(1, 0, searchValue).ToList();
        }
        public static Shipper? GetShipper(int id)
        {
            if (id <= 0)
                return null;
            return shipperDB.Get(id);
        }

        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }
        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }
        public static bool DeleteShipper(int id)
        {
            return shipperDB.Delete(id);
        }
        public static bool IsUsedShipper(int id)
        {
            return shipperDB.IsUsed(id);
        }
        /// <summary>
        /// Employee
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Employee> ListofEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }
        public static List<Employee> ListofEmployee(string searchValue = "")
        {
            return employeeDB.List(1, 0, searchValue).ToList();
        }
        public static Employee? GetEmployee(int id)
        {
            if (id <= 0)
                return null;
            return employeeDB.Get(id);
        }

        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }
        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }
        public static bool DeleteEmployee(int id)
        {
            return employeeDB.Delete(id);
        }
        public static bool IsUsedEmployee(int id)
        {
            return employeeDB.IsUsed(id);
        }
    }
}
//Lớp static là gì 
//Constructor trong lớp static có đặc điểm gì ? Dc gọi khi nào
