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
    public class ProductDAL : _BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Product data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO Products(ProductName,ProductDescription,SupplierID,CategoryID,Unit,Price,Photo,IsSelling)
                        VALUES(@ProductName,@ProductDescription,@SupplierID,@CategoryID,@Unit,@Price,@Photo,@IsSelling);
                            SELECT @@IDENTITY";
                var parameters = new
                {
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling,
                    
                    
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();

            }
            return id;
        }

        public long AddAttribute(ProductAttribute data)
        {
            long id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO ProductAttributes(ProductID, AttributeName, AttributeValue)
                            VALUES(@ProductID, @AttributeName, @AttributeValue);
                            SELECT CAST(SCOPE_IDENTITY() as long)";
                var parameters = new
                {
                    data.ProductID,
                    data.AttributeName,
                    data.AttributeValue
                };
                int tempId = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);

                // Chuyển đổi kiểu int sang long
                id = (long)tempId;
                connection.Close(); 
            }
            return id;
        }

        public long AddPhoto(ProductPhoto data)
        {
            long id = 0;
            
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO ProductPhotos(ProductID, Photo)
                            VALUES(@ProductID, @Photo);
                           SELECT CAST(SCOPE_IDENTITY() AS bigint)";
                var parameters = new
                {
                    data.ProductID,
                    data.Photo
                };
                int tempId = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);

                // Chuyển đổi kiểu int sang long
                id = (long)tempId;
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT Count(*)
                            FROM Products
                            WHERE (@searchValue = N'' OR ProductName LIKE @searchValue)
                              AND (@categoryID = 0 OR CategoryID = @categoryID)
                              AND (@supplierID = 0 OR SupplierID = @supplierID)
                              AND (Price >= @minPrice AND (@maxPrice <= 0 OR Price <= @maxPrice))";
                var parameters = new
                {
                    searchValue = $"%{searchValue}%",
                    categoryID,
                    supplierID,
                    minPrice,
                    maxPrice
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return count;
        }

       

        public bool Delete(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"DELETE FROM Products WHERE ProductID=@ProductID";
                var parameters = new
                {
                   productID = productID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }
            return result;
        }

        public bool DeleteAttribute(long attributeID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = "DELETE FROM ProductAttributes WHERE AttributeID = @AttributeID";
                var parameters = new { AttributeID =attributeID};
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeletePhoto(long productID)
        {

            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = "DELETE FROM ProductPhotos WHERE PhotoID = @PhotoID";
                var parameters = new { productID=productID };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Product? Get(int productID)
        {
            Product? data = null;
            using (var connection = OpenConnection()) {
                var sql = "SELECT * FROM Products WHERE ProductID=@ProductID";
                var parameters = new
                {
                    productID = productID
                };
                data = connection.QueryFirstOrDefault<Product>(sql:sql, param: parameters, commandType: CommandType.Text);
            }
            return data;
        }

      

        public ProductAttribute? GetAttribute(long attributeID)
        {
            ProductAttribute? data = null;
            using (var connection = OpenConnection())
            {
                var sql = "SELECT * FROM ProductAttributes WHERE AttributeID = @AttributeID";
                var parameters = new { AttributeID = attributeID };
                data = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();

            }
            return data;
        }

        public ProductPhoto GetPhoto(long productID)
        {
            ProductPhoto data = null;
            using (var connection = OpenConnection())
            {
                var sql = "SELECT * FROM ProductPhotos WHERE PhotoID = @PhotoID";
                var parameters = new { PhotoID = productID };
                data= connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();   
            }
            return data;
        }

        public bool InUsed(int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {

                var sql = @"IF EXISTS (SELECT * FROM Products WHERE ProductId = @ProductId)
                                 SELECT 1
                             ELSE 
                                  SELECT 0 ";
                var parameters = new
                {
                    ProductID= productID,
                };
                result = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> data = new List<Product>();
            using (var connection = OpenConnection())
            {
                var sql = @"SELECT *
                            FROM (
                                SELECT *, ROW_NUMBER() OVER(ORDER BY ProductName) AS RowNumber
                                FROM Products 
                                WHERE (@searchValue = N'' OR ProductName LIKE @searchValue)
                                  AND (@categoryID = 0 OR CategoryID = @categoryID)
                                  AND (@supplierID = 0 OR SupplierID = @supplierID)
                                  AND (Price >= @minPrice AND (@maxPrice <= 0 OR Price <= @maxPrice))
                            ) AS t
                            WHERE (@pageSize = 0) OR (RowNumber BETWEEN (@page - 1) * @pageSize + 1 AND @page * @pageSize)";
                var parameters = new
                {
                    searchValue = $"%{searchValue}%",
                    categoryID,
                    supplierID,
                    minPrice,
                    maxPrice,
                    page,
                    pageSize
                };
                data = connection.Query<Product>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            return data;
        }

        public IList<ProductAttribute> ListAttributes(int productID)
        {
            IList < ProductAttribute >data = new List<ProductAttribute>();
            using (var connection = OpenConnection())
            {
                var sql = "SELECT * FROM ProductAttributes WHERE ProductID = @ProductID";
                var parameters = new { ProductID = productID };
                 data = connection.Query<ProductAttribute>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
            }
            return data;
        }

        public IList<ProductPhoto> ListPhotos(int productID)
        {
            IList<ProductPhoto> data= new List<ProductPhoto>();
            using (var connection = OpenConnection())
            {
                var sql = "SELECT * FROM ProductPhotos WHERE ProductID = @ProductID";
                var parameters = new { ProductID = productID };
                data = connection.Query<ProductPhoto>(sql: sql, param: parameters, commandType: CommandType.Text).ToList();
                connection.Close();
                
            }
            return data;
        }

        public bool Update(Product data)
        {
            bool result = false;
            using (var connection = OpenConnection()) {
                var sql = "UPDATE Products SET " +
                    " ProductName = @ProductName," +
                    "ProductDescription=@ProductDescription," +
                    "Unit=@Unit," +
                    "Price=@Price," +
                    "Photo=@Photo," +
                    "IsSelling=@IsSelling " +
                    "WHERE ProductID=@ProductID";
                var parameters = new
                {
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling,
                    ProductId=data.ProductId
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text)>0;
            }
            return result;
        }

        public bool UpdateAttribute(ProductAttribute data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE ProductAttributes
                            SET AttributeName = @AttributeName,
                                AttributeValue = @AttributeValue
                            WHERE AttributeID = @AttributeID";
                var parameters = new
                {
                    data.AttributeID,
                    data.AttributeName,
                    data.AttributeValue
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text)>0;
                connection.Close();
            }
            return result;
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result= false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE ProductPhotos
                            SET Photo = @Photo
                            WHERE PhotoID = @PhotoID";
                var parameters = new
                {
                    data.PhotoId,
                    data.Photo
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text)>0;
                connection.Close();  
            }
            return result;
        }
    }
}
