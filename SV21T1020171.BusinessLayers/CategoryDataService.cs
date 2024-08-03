using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV21T1020171.BusinessLayers
{
    public static class CategoryDataService
    {
        private static CategoryDAL categoryDb;
        static CategoryDataService()
        {
            string connnectionString = @"server=.;
                                        user id=sa;
                                        password=sa;
                                        database=LiteCommerceDB;
                                        TrustServerCertificate=true";
            categoryDb = new CategoryDAL(connnectionString);
        }

       public static List<Category> ListofCategory()
        {
            return categoryDb.List();
        }
        public static Category Detail(int id)
        {
           return categoryDb.Detail(id);
        }
        public static void Delete(int id) { 
             categoryDb.Delete(id);
        }
    }
}
