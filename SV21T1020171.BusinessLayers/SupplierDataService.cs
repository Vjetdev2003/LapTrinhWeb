using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020171.BusinessLayers
{
    public class SupplierDataService
    {
        private static readonly SupplierDAL supplierDAL;
        static SupplierDataService()
        {
            string connnectionString = @"server=.;
                                        user id=sa;
                                        password=sa;
                                        database=LiteCommerceDB;
                                        TrustServerCertificate=true";
            supplierDAL = new SupplierDAL(connnectionString);
        }
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers()
        {

            return supplierDAL.List();
        }
        public static void Create(Supplier supplier)
        {
             supplierDAL.Create(supplier);
        }
        public static Supplier SupplierDetail(int id)
        {
            return supplierDAL.SupplierDetail(id);
        }
        public static void Delete(int id)
        {
            supplierDAL.Delete(id);
        }
        public static List<Provinces> GetProvinces() { 
            return supplierDAL.GetProvinces();
        }
        public static void AddProvice(string provinces)
        {
             supplierDAL.AddProvince(provinces);
        }
    }
}