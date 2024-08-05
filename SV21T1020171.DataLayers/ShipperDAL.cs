using Dapper;
using Microsoft.Data.SqlClient;
using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020171.DataLayers
{
    public class ShipperDAL
    {
        private string connectionString = "";
        public ShipperDAL(string connectionString) {
            this.connectionString = connectionString;
        }

        public List<Shipper> List()
        {
            List<Shipper>list = new List<Shipper>();
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                connection.Open();
                var sql = @"SELECT * FROM Shippers";
                list =connection.Query<Shipper>(sql:sql , commandType:System.Data.CommandType.Text).ToList();

                connection.Close();
            }
            return list;
        }
        public int Create(Shipper shipper)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = @"INSERT INTO Shippers (ShipperName, Phone)
                            VALUES (@ShipperName, @Phone);
                            SELECT CAST(SCOPE_IDENTITY() as int)";
                return connection.QuerySingle<int>(sql, new { shipper.ShipperName, shipper.Phone });
            }
        }
        public Shipper Detail(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = @"SELECT * FROM Shippers WHERE ShipperID = @Id";
                return connection.QuerySingleOrDefault<Shipper>(sql, new { Id = id });
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var sql = @"DELETE FROM Shippers WHERE ShipperID = @Id";
                var rowsAffected = connection.Execute(sql, new { Id = id });
                return rowsAffected > 0;
            }
        }

    }
}
