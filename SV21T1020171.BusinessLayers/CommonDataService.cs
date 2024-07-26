using System;
using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;

namespace SV21T1020171.BusinessLayers
{
    public static class CommonDataService
    {
        private static readonly CustomerDAL customerDB;
        private static readonly SupplierDAL supplierDAL;
        //Constructor static khoong dc co tham so
        static CommonDataService()
        {
            string connnectionString = @"server=.;
                                        user id=sa;
                                        password=sa;
                                        database=LiteCommerceDB;
                                        TrustServerCertificate=true";
            customerDB = new CustomerDAL(connnectionString);
            supplierDAL = new SupplierDAL(connnectionString);
        }
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers()
        {

            return customerDB.List();
        }
        public static List<Supplier> ListOfSuppliers() {
            return supplierDAL.List();
        }
    }
}
