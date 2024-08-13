using Dapper;
using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020171.DataLayers.SQLServer
{
    public class EmployeeDAL : _BaseDAL, ICommonDAL<Employee>
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Employee data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @" INSERT INTO Employees(FullName,BirthDate,Address,Phone,Email,Photo,IsWorking)
                              VALUES(@FullName,@BirthDate,@Address,@Phone,@Email,@Photo,@IsWorking);
                         SELECT @@IDENTITY";
                var parameters = new
                {
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address= data.Address??"",
                    Phone = data.Phone ?? "",
                    Email=  data.Email ??"",
                    Photo= data.Photo ??"",
                    IsWorking =data.IsWorking
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*)
                            from Employees
                             where FullName like @searchValue ";
                var parameters = new
                {
                    searchValue = $"%{searchValue}%"
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM Employees WHERE EmployeeID=@EmployeeID";
                var parameters = new
                {
                    EmployeeID = id,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Employee? Get(int id)
        {
            Employee? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT * FROM Employees WHERE EmployeeID=@EmployeeID";
                var parameters = new
                {
                    EmployeeID = id,
                };
                data = connection.QueryFirstOrDefault<Employee?>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {

                var sql = @"IF EXISTS (SELECT * FROM Employees WHERE EmployeeID = @EmployeeID)
                                 SELECT 1
                             ELSE 
                                  SELECT 0 ";
                var parametes = new
                {
                    EmployeeID = id
                };
                result = connection.ExecuteScalar<int>(sql: sql, param: parametes, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public IList<Employee> List(int page = 1, int pagesize = 10, string searchValue = "")
        {
            List<Employee> data = new List<Employee>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT *
                                    FROM(
	                                    SELECT *,
	                                    ROW_NUMBER()over (order by FullName) as RowNumber
	                                    FROM Employees
	                                    WHERE FullName like @searchValue 
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
                data = (List<Employee>)connection.Query<Employee>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool Update(Employee data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Employees SET 
                            FullName=@FullName,
                            BirthDate=@BirthDate,
                            Address=@Address,
                            Phone=@Phone,
                            Email=@Email,
                            Photo=@Photo,
                            IsWorking=@IsWorking
                            WHERE EmployeeID=@EmployeeID";
                var parameters = new
                {
                    FullName=data.FullName ??"",
                    BirthDate = data.BirthDate,
                    Address=data.Address??"",
                    Phone =data.Phone??"",
                    Email=data.Email ??"",
                    Photo=data.Photo ??"",
                    IsWorking=data.IsWorking,
                    EmployeeID=data.EmployeeID,
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }
    }
}
