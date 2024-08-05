using Dapper;
using SV21T1020171.DomainModels;
using System.Data;



namespace SV21T1020171.DataLayers.SQLServer
{
    public class SupplierDAL : _BaseDAL, ICommonDAL<Supplier>
    {
        private string connectionString { get; set; } = "";
        public SupplierDAL(string connectionString) : base(connectionString) {
            _connectionString = connectionString;
        }
        public int Add(Supplier data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO Suppliers(SupplierName,ContactName,Province,Address,Phone,Email)
                            VALUES(@SupplierName,@ContactName,@Province,@Address,@Phone,@Email);
                            SELECT @@IDENTITY";
                var parameters = new
                {
                    SupplierName = data.SupplierName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();

            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            using (var connection= OpenConnection()) {
                var sql = @"SELECT Count(*) FROM Suppliers WHERE SupplierName Like @searchValue or ContactName Like @searchValue";
                var parameters = new
                {
                    searchValue = $"%{searchValue}%",
                };
                count = connection.ExecuteScalar<int>(sql:sql,param: parameters,commandType: CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection()) {
                var sql = @"DELETE FROM Suppliers WHERE SupplierID=@SupplierID";
                var parameters = new
                {
                    SupplierID = id,
                };
                result = connection.Execute(sql:sql,param : parameters,commandType: CommandType.Text) >0;
                connection.Close() ;
            }
            return result;
        }

        public Supplier? Get(int id)
        {
            Supplier? data = null;
            using (var connection = OpenConnection()) {
                var sql = @"SELECT * FROM Suppliers WHERE SupplierID=@SupplierID";
                var parameters = new
                {
                    SupplierID = id,
                };
                data = connection.QueryFirstOrDefault<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();           
            }
            return data;
        }

        public bool IsUsed(int id)
        {
            bool result = false;
            using (var connection = OpenConnection()) {
                var sql = @"IF EXISTS (SELECT * FROM Products WHERE SupplierID=@SupplierID  ) 
                                  SELECT 1
                             ELSE 
                                  SELECT 0 ";
                var parameters = new
                {
                    SupplierID = id,
                };
                result = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text) >0;
                connection.Close();
            }
            return result;
        }

        public IList<Supplier> List(int page = 1, int pagesize = 10, string searchValue = "")
        {
           List<Supplier>data = new List<Supplier>();
            using (var connection = OpenConnection()) {
                var sql = @"SELECT *
                                    FROM(
	                                    SELECT *,
	                                    ROW_NUMBER()over (order by SupplierName) as RowNumber
	                                    FROM Suppliers
	                                    WHERE SupplierName like @searchValue or ContactName like @searchValue
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
                data = (List<Supplier>)connection.Query<Supplier>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return data;
        }
        public bool Update(Supplier data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Suppliers SET 
                          SupplierName = @SupplierName,
                          ContactName =@ContactName,
                          Province=@Province,
                          Address=@Address,
                          Phone = @Phone,
                          Email=@Email
                           WHERE SupplierID=@SupplierID";
                var parameters = new
                {
                    SupplierName = data.SupplierName ?? "",
                    ContactName = data.ContactName ?? "",
                    Province = data.Province ?? "",
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    SupplierId=data.SupplierID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;

        }

    }
}
