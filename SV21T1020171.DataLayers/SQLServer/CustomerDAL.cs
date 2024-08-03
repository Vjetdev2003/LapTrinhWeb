using Dapper;
using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SV21T1020171.DataLayers.SQLServer
{
    public class CustomerDAL : _BaseDAL, ICommonDAL<Customer>
    {
        public CustomerDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Customer data)
        {
            int id = 0;
            using (var connection =OpenConnection())
            {
                var sql = @"INSERT INTO Customers(CustomerName,ContactName,Province,Address,Phone,Email,IsLocked)
                        VALUES(@CustomerName,@ContactName,@Province,@Address,@Phone,@Email,@IsLocked);
                            SELECT @@IDENTITY";
                var parameters = new {
                    CustomerName=data.CustomerName ?? "",
                    ContactName=data.ContactName ?? "",
                    Province=data.Province ?? "",
                    Address=data.Address ?? "",
                    Phone= data.Phone ?? "",
                    Email= data.Email ?? "",
                    IsLocked= data.IsLocked,
                };
                id = connection.ExecuteScalar<int>(sql:sql,param:parameters,commandType:CommandType.Text);
                connection.Close();
            
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
           int count = 0;
            using (var connection = OpenConnection()) {
                var sql = @"select count(*)
                            from Customers
                             where CustomerName like @searchValue or ContactName like @searchValue";
                var parameters = new
                {
                    searchValue = $"%{searchValue}%"
                };
                count = connection.ExecuteScalar<int>(sql:sql, param:parameters,commandType:CommandType.Text);
                connection.Close();
            }
            return count;

        }

        public bool Delete(int id)
        {
            bool result =false;

            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM Customers WHERE CustomerId=@CustomerId";
                var parameters = new {CustomerId=id };
                result= connection.Execute(sql:sql,param:parameters,commandType:CommandType.Text)>0;
            };
            return result;
        }

        public Customer? Get(int id)
        {
           Customer? data = null;
            using (var connection = OpenConnection()) {

                var sql = @"SELECT * FROM Customers WHERE CustomerId=@CustomerId";
                var parametes = new
                {
                    CustomerId = id
                };
                data = connection.QueryFirstOrDefault<Customer>(sql:sql,param:parametes,commandType:CommandType.Text);
                connection.Close ();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {

                var sql = @"IF EXISTS (SELECT * FROM Customers WHERE CustomerId = @CustomerId)
                                 SELECT 1
                             ELSE 
                                  SELECT 0 ";
                var parametes = new
                {
                    CustomerId = id
                };
                result = connection.ExecuteScalar<bool>(sql:sql,param:parametes,commandType:CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public IList<Customer> List(int page = 1, int pagesize = 10, string searchValue = "")
        {
            List<Customer> data = new List<Customer>();
            using (var connection = OpenConnection()) {
                var sql = @"SELECT *
                                    FROM(
	                                    SELECT *,
	                                    ROW_NUMBER()over (order by CustomerName) as RowNumber
	                                    FROM Customers
	                                    WHERE CustomerName like @searchValue or ContactName like @searchValue
	                                    ) as t
                                    where @pageSize=0
                                      or RowNumber between (@page-1) * @pageSize +1 and (@page *@pageSize)
                                    ";
                var parameters = new
                {
                    page = page,///tên tham số trong câu lệnh SQL Page đầu tiên 
                    pagesize = pagesize,
                    searchValue = $"%{searchValue}%"
                };
                data = connection.Query<Customer>(sql:sql, param:parameters,commandType:CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Customer data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Customers SET 
                    CustomerName=@CustomerName,
                    ContactName =@ContactName,
                    Province =@Province,
                    Address =@Address,
                    Phone =@Phone,
                    Email=@Email,
                    IsLocked=@IsLocked
                    WHERE CustomerId=@CustomerId";
                var parameters = new
                {
                    CustomerName = data.CustomerName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    IsLocked = data.IsLocked,
                };
                data = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close ();
            }
            return result;

        }
    }
}
