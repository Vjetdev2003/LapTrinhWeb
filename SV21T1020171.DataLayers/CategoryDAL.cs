using Dapper;
using Microsoft.Data.SqlClient;
using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020171.DataLayers
{
    public class CategoryDAL
    {
        private string connectionString="";
        public CategoryDAL(string connnectionString) { 
            connectionString = connnectionString;
        }

        public List<Category> List() { 
            List<Category> list = new List<Category>();
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                var sql = @"SELECT * FROM Categories";
                list = connection.Query<Category>(sql : sql , commandType:System.Data.CommandType.Text).ToList();
            }

                return list;
        }
        public Category Detail(int id)
        {
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                var sql = @"SELECT * FROM Categories WHERE CategoryID = @id";
                return connection.QuerySingleOrDefault<Category>(sql, new { id = id });
            }
        }
        public void Delete(int id) {
            using (var connection = new SqlConnection()) {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var deleteProductSql = "DELETE FROM Products WHERE CategoryID = @CategoryId";
                        connection.Execute(deleteProductSql, new { CategoryID = id }, transaction);

                        var deleteCategorySql = "DELETE FROM Categories WHERE CategoryID = @Id";
                        connection.Execute(deleteCategorySql, new { Id = id }, transaction);

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
