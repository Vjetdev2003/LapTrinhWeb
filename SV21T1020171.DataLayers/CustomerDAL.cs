using System;
using Dapper;
using SV21T1020171.DomainModels;
using Microsoft.Data.SqlClient;

namespace SV21T1020171.DataLayers
{
    public class CustomerDAL
    {
        private string connectionString = "";
        /// <summary>
        /// Constructor(hàm tạo) 
        /// :Hàm dc gọi khi tạo ra đối tượng (object) là thể hiện(instance) 1 lớp 
        /// </summary>  
        /// <param name="connectionString"></param>
        public CustomerDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }
        //Phương thức
        public List<Customer> List()
        {
            List<Customer> list = new List<Customer>();
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                //Gabage Collector(GC quét bộ nhớ)
                var sql = @"SELECT * FROM Customers";
                list = connection.Query<Customer>(sql: sql, commandType: System.Data.CommandType.Text).ToList();

                connection.Close();
            }
            return list;
        }
        public Customer CustomerDetail(int id)
        {
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                var sql = @"SELECT * FROM Customers WHERE CustomerId = @id";
                return connection.QuerySingleOrDefault<Customer>(sql, new { id = id });
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
                        var deleteOrderSql = "DELETE FROM Orders WHERE CustomerID = @CustomerID";
                        connection.Execute(deleteOrderSql, new { CustomerID = id }, transaction);

                        var deleteCustomerSql = "DELETE FROM Customers WHERE CustomerID = @Id";
                        connection.Execute(deleteCustomerSql, new { Id = id }, transaction);

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
    }
}
