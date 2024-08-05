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
    public class ShipperDAL : _BaseDAL, ICommonDAL<Shipper>
    {
        public ShipperDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Shipper data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @" INSERT INTO Shippers(ShipperName,Phone)
                              VALUES(@ShipperName,@Phone);
                         SELECT @@IDENTITY";
                var parameters = new
                {
                    ShipperName = data.ShipperName ?? "",
                    Phone = data.Phone ?? "",
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
                            from Shippers
                             where ShipperName like @searchValue ";
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
                var sql = @"DELETE FROM Shippers WHERE ShipperID=@ShipperID";
                var parameters = new
                {
                    ShipperID = id,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Shipper? Get(int id)
        {
            Shipper? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT * FROM Shippers WHERE ShipperID=@ShipperID";
                var parameters = new
                {
                    ShipperID = id,
                };
                data = connection.QueryFirstOrDefault<Shipper?>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool IsUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {

                var sql = @"IF EXISTS (SELECT * FROM Shippers WHERE ShipperID = @ShipperID)
                                 SELECT 1
                             ELSE 
                                  SELECT 0 ";
                var parametes = new
                {
                    ShipperID = id
                };
                result = connection.ExecuteScalar<int>(sql: sql, param: parametes, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public IList<Shipper> List(int page = 1, int pagesize = 10, string searchValue = "")
        {
            List<Shipper> data = new List<Shipper>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT *
                                    FROM(
	                                    SELECT *,
	                                    ROW_NUMBER()over (order by ShipperName) as RowNumber
	                                    FROM Shippers
	                                    WHERE ShipperName like @searchValue 
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
                data = connection.Query<Shipper>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Shipper data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Shippers SET 
                            ShipperName=@ShipperName,
                            Phone=@Phone
                            WHERE ShipperID=@ShipperID";
                var parameters = new
                {
                    ShipperName = data.ShipperName ?? "",
                    Phone = data.Phone ?? "",
                    ShipperID = data.ShipperID
                };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return result;
        }
    }
}
