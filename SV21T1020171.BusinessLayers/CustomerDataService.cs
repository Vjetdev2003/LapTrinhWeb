using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020171.BusinessLayers
{
    public class CustomerDataService
    {
        private static readonly CustomerDAL customerDB;
        static CustomerDataService()
        {
            string connnectionString = @"server=.;
                                        user id=sa;
                                        password=sa;
                                        database=LiteCommerceDB;
                                        TrustServerCertificate=true";
            customerDB = new CustomerDAL(connnectionString);
        }
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers()
        {

            return customerDB.List();
        }
        public static Customer CustomerDetail(int id)
        {
            return customerDB.CustomerDetail(id);
        }
        public static void Delete(int id)
        {
            customerDB.Delete(id);
        }

    }
}
