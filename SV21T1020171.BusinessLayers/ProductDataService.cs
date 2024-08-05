using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020171.BusinessLayers
{
    public class ProductDataService
    {
        private static readonly ProductDAL productDAL;
        static ProductDataService()
        {
            string connnectionString = @"server=.;
                                        user id=sa;
                                        password=sa;
                                        database=LiteCommerceDB;
                                        TrustServerCertificate=true";
            productDAL = new ProductDAL(connnectionString);
        }
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public static List<Product> List()
        {

            return productDAL.List();
        }
    }
}