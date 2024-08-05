using Dapper;
using Microsoft.Data.SqlClient;
using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SV21T1020171.DataLayers
{
    public class EmployeeDAL
    {

        public string connectionString = "";
        public EmployeeDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public List<Employee> List()
        {
            List<Employee> list = new List<Employee>();
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                //Gabage Collector(GC quét bộ nhớ)
                var sql = @"SELECT * FROM Employees";
                list = connection.Query<Employee>(sql: sql, commandType: System.Data.CommandType.Text).ToList();

                connection.Close();
            }
            return list;

        }
        public Employee Detail(int id)
        {
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                var sql = @"SELECT * FROM Employees WHERE EmployeeID = @id";
                return connection.QuerySingleOrDefault<Employee>(sql, new { id = id });
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deleteProductsSql = "DELETE FROM Products WHERE SupplierID = @SupplierID";
                        connection.Execute(deleteProductsSql, new { SupplierID = id }, transaction);

                        var deleteSupplierSql = "DELETE FROM Suppliers WHERE SupplierID = @Id";
                        connection.Execute(deleteSupplierSql, new { Id = id }, transaction);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public void Create(Supplier supplier)
        {
            Console.WriteLine(supplier.Provice);
            Console.WriteLine("đã in");
            using (var connection = new SqlConnection(connectionString))
            {
                var createProvinces = @"INSERT INTO Provinces (ProvinceName) VALUES (@ProvinceName)";
                connection.ExecuteAsync(createProvinces, new
                {
                    supplier.Provice,
                });

                var createSuppliers = @"INSERT INTO Suppliers (SupplierName, ContactName, Provice, Address, Phone, Email)
                    VALUES (@SupplierName, @ContactName, @Provice, @Address, @Phone, @Email)";
                connection.ExecuteAsync(createSuppliers, new
                {
                    supplier.SupplierName,
                    supplier.ContactName,
                    supplier.Provice,
                    supplier.Address,
                    supplier.Phone,
                    supplier.Email
                });



            }
        }
    }
}
