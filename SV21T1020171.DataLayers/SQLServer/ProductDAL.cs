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
                string sql = @"insert into Products(ProductName, ProductDescription, SupplierID, CategoryID, Unit, Price, Photo, IsSelling)
                                values (@ProductName, @ProductDescription, @SupplierID, @CategoryID, @Unit, @Price, @Photo, @IsSelling);
                                select @@Identity;";
                var parameters = new
                {
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
            return id;
        }

        public long AddAttribute(ProductAttribute data)
        {
            long id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO ProductAttributes(ProductID, AttributeName, AttributeValue,DisplayOrder)
                            VALUES(@ProductID, @AttributeName,@AttributeValue,@DisplayOrder);
                            SELECT @@Identity";
                var parameters = new
                {
                    ProductID=data.ProductID,
                    AttributeName = data.AttributeName??"",
                    AttributeValue = data.AttributeValue??"",
                    DisplayOrder = data.DisplayOrder
                };
                id= connection.ExecuteScalar<long>(sql: sql, param: parameters, commandType: CommandType.Text);

                connection.Close(); 
            }
            return id;
        }

        public long AddPhoto(ProductPhoto data)
        {
            long id = 0;
            
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO ProductPhotos(ProductID, Photo,Description,DisplayOrder,IsHidden)
                            VALUES(@ProductID, @Photo,@Description,@DisplayOrder,@IsHidden);
                           select @@Identity";
                var parameters = new
                {
                    ProductID = data.ProductID,
                    Photo=  data.Photo ??"",
                    Description =  data.Description??"",
                    DisplayOrder = data.DisplayOrder,
                    IsHidden =  data.IsHidden
                };
                id = connection.ExecuteScalar<long>(sql: sql, param: parameters, commandType: CommandType.Text);
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
                    ProductId = productID
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

        public bool DeletePhoto(long photoID)
        {

            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = "DELETE FROM ProductPhotos WHERE PhotoID = @PhotoID";
                var parameters = new { photoID= photoID };
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

        public ProductPhoto? GetPhoto(long productID)
        {
            ProductPhoto? data = null;
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
            var result = false;
            using (var connection = OpenConnection())
            {
                string sql = @"IF EXISTS (
                                        SELECT * 
                                        FROM OrderDetails 
                                        WHERE ProductID = @ProductID
                                    ) OR EXISTS (
                                        SELECT * 
                                        FROM ProductPhotos 
                                        WHERE ProductID = @ProductID
                                    ) OR EXISTS (
                                        SELECT * 
                                        FROM ProductAttributes 
                                        WHERE ProductID = @ProductID
                                    )
                                        SELECT 1
                                    ELSE
                                        SELECT 0;";
                var param = new { ProductID = productID };
                int count = connection.ExecuteScalar<int>(sql, param, commandType: CommandType.Text);
                connection.Close();
                result = count > 0;
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
                    "SupplierID=@SupplierID," +
                    "CategoryID=@CategoryID," +
                    "Unit=@Unit," +
                    "Price=@Price," +
                    "Photo=@Photo," +
                    "IsSelling=@IsSelling " +
                    "WHERE ProductID=@ProductID";
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
                    ProductId=data.ProductID
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
                                AttributeValue = @AttributeValue,
                                DisplayOrder = @DisplayOrder
                             where ProductID = @ProductID and AttributeID = @AttributeID";
                var parameters = new
                {
                    ProductID= data.ProductID,
                    AttributeID = data.AttributeID,
                    AttributeName = data.AttributeName,
                    DisplayOrder = data.DisplayOrder,
                    AttributeValue = data.AttributeValue
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
                            SET Photo = @Photo,
                                Description=@Description,
                                DisplayOrder=@DisplayOrder,
                                IsHidden=@IsHidden
                            where ProductID = @ProductID and PhotoID = @PhotoID";
                var parameters = new
                {
                    ProductID= data.ProductID,
                    Description= data.Description,
                    DisplayOrder= data.DisplayOrder,
                    IsHidden= data.IsHidden,
                    PhotoId=data.PhotoId,
                    Photo=data.Photo
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text)>0;
                connection.Close();  
            }
            return result;
        }
    }
}
