using Dapper;
using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                var sql = @"
                    INSERT INTO Employees(FullName, BirthDate, [Address], Phone, Email, Photo, IsWorking) 
                    VALUES (@FullName, @BirthDate, @Address, @Phone, @Email, @Photo, @IsWorking);

                    SELECT @@IDENTITY
                ";
                var param = new
                {
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Photo = data.Photo ?? "",
                    IsWorking = data.IsWorking
                };
                id = connection.ExecuteScalar<int>(sql, param, commandType: System.Data.CommandType.Text);
                connection.Close();
            };
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"
                    SELECT count(*) 
                    FROM Employees
                    WHERE FullName LIKE @searchValue
                ";
                var param = new
                {
                    searchValue = $"%{searchValue}%"
                };
                count = connection.ExecuteScalar<int>(sql, param, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {

            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"
                    DELETE FROM Employees
                    WHERE EmployeeID = @EmployeeID
                ";
                var param = new
                {
                    EmployeeID = id
                };
                result = connection.Execute(sql, param, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Employee? Get(int id)
        {
            Employee? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"
                    SELECT * 
                    FROM Employees
                    WHERE EmployeeId = @EmployeeId 
                ";
                var param = new
                {
                    EmployeeId = id
                };
                data = connection.QueryFirstOrDefault<Employee>(sql, param, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"
                    SELECT COUNT(*) 
                    FROM Orders 
                    WHERE EmployeeID = @EmployeeID
                ";
                var param = new
                {
                    EmployeeID = id
                };
                result = connection.ExecuteScalar<int>(sql, param, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public IList<Employee> List(int page = 1, int pagesize = 10, string searchValue = "")
        {
            List<Employee> result = new List<Employee>();
            using (var connection = OpenConnection())
            {
                var sql = @"
                            SELECT * 
                            FROM (
		                            SELECT *, 
			                            ROW_NUMBER() OVER (ORDER BY FullName) AS RowNumber
		                            FROM Employees
		                            WHERE FullName LIKE @searchValue 
	                            ) AS t
                            WHERE @pageSize = 0
	                            OR (RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize)
                            ORDER BY RowNumber";
                var param = new
                {
                    page = page,
                    pageSize = pagesize,
                    searchValue = $"%{searchValue}%"
                };
                result = connection.Query<Employee>(sql, param, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return result;
        }

        public bool Update(Employee data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"
                        UPDATE Employees
                        SET FullName = @FullName,
	                        BirthDate = @BirthDate,
	                        Address =  @Address,
	                        Phone =  @Phone,
	                        Email = @Email,
	                        IsWorking = @IsWorking,
	                        Photo = @Photo
                        WHERE EmployeeID = @EmployeeID
                       ";
                var param = new
                {
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    IsWorking = data.IsWorking,
                    Photo = data.Photo ?? "",
                    EmployeeID = data.EmployeeID
                };
                result = connection.Execute(sql, param, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
