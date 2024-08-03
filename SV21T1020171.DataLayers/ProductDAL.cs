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
    public class ProductDAL
    {
        private string connectionString = "";
        public ProductDAL(string connnectionString)
        {
            connectionString = connnectionString;
        }

        public List<Product> List()
        {
            List<Product> list = new List<Product>();
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                var sql = @"SELECT * FROM Products";
                list = connection.Query<Product>(sql: sql, commandType: System.Data.CommandType.Text).ToList();
            }

            return list;
        }
    }
}