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
    public class ProductDAL : _BaseDAL,ICommonDAL<Product>
    {
        public ProductDAL(string connectionString) : base(connectionString) {
        }

        public int Add(Product data)
        {
            throw new NotImplementedException();
        }

        public int Count(string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Product? Get(int id)
        {
            throw new NotImplementedException();
        }

        public bool IsUsed(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Product> List(int page = 1, int pagesize = 10, string searchValue = "")
        {
            throw new NotImplementedException();
        }

        public bool Update(Product data)
        {
            throw new NotImplementedException();
        }

        public bool Update(Supplier data)
        {
            throw new NotImplementedException();
        }
    }
}
