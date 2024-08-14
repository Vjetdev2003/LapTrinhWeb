using Dapper;
using SV21T1020171.DomainModels;
using System;
using System.Data;

namespace SV21T1020171.DataLayers.SQLServer
{
    public class CategoryDAL :_BaseDAL,ICommonDAL<Category> 
    {
        public CategoryDAL(string connectionString):base(connectionString) { 
        
        }

        public int Add(Category data)
        {
            int id = 0;
            using (var connection = OpenConnection()) {
                var sql = @" INSERT INTO Categories(CategoryName,Description)
                              VALUES(@CategoryName,@Description);
                         SELECT @@IDENTITY";
                var parameters = new
                {
                    CategoryName = data.CategoryName ??"",
                    Description = data.Description??"",
                };
                id = connection.ExecuteScalar<int>(sql:sql,param:parameters,commandType:CommandType.Text);
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
                            from Categories
                             where CategoryName like @searchValue ";
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
            using (var connection = OpenConnection()) {
                var sql = @"DELETE FROM Categories WHERE CategoryID=@CategoryID";
                var parameters = new
                {
                    CategoryId = id,
                };
                result = connection.Execute(sql:sql,param:parameters, commandType:CommandType.Text)>0;
                connection.Close();
            }
            return result;
        }

        public Category? Get(int id)
        {   
            Category? data = null;
            using (var connection = OpenConnection()) {
                var sql = @"SELECT * FROM Categories WHERE CategoryID=@CategoryID";
                var parameters = new
                {
                    CategoryId = id,
                };
                data= connection.QueryFirstOrDefault<Category?>(sql:sql,param:parameters,commandType: CommandType.Text);
                connection.Close() ;
            }
            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"IF EXISTS (SELECT * FROM Products WHERE CategoryID=@CategoryID)
                           SELECT 1 
                            ESLE
                            SELECT 0";
                var parameters = new
                {
                    CategoryID = id,
                };
                result = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public IList<Category> List(int page = 1, int pagesize = 10, string searchValue = "")
        {
            List<Category> data = new List<Category>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT *
                                    FROM(
	                                    SELECT *,
	                                    ROW_NUMBER()over (order by CategoryName) as RowNumber
	                                    FROM Categories
	                                    WHERE CategoryName like @searchValue 
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
                data = connection.Query<Category>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Category data)
        {
            bool result= false;
            using (var connection = OpenConnection()) {
                var sql = @"UPDATE Categories SET 
                            CategoryName=@CategoryName,
                            Description=@Description
                            WHERE CategoryID=@CategoryID";
                var parameters = new
                {
                    CategoryName = data.CategoryName ?? "",
                    Description = data.Description ?? "",
                    CategoryID=data.CategoryId
                };
                result = connection.ExecuteScalar<bool>(sql:sql,param:parameters,commandType: CommandType.Text);
                connection.Close ();
            }
            return result;
        }
    }
}
