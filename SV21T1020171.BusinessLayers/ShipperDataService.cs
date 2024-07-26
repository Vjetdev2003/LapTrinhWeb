using System;
using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;

namespace SV21T1020171.BusinessLayers
{
    public static class ShipperDataService
    {
        private static readonly ShipperDAL shipperDB;
        //Constructor static khoong dc co tham so
        static ShipperDataService()
        {
            string connnectionString = @"server=.;
                                        user id=sa;
                                        password=sa;
                                        database=LiteCommerceDB;
                                        TrustServerCertificate=true";
            shipperDB = new ShipperDAL(connnectionString);
        }
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public static List<Shipper> ListOfShipper()
        {

            return shipperDB.List();
        }
        public static int CreateShipper(Shipper shipper)
        {
            return shipperDB.Create(shipper);
        }
        public static Shipper Detail(int id)
        {
            return shipperDB.Detail(id);
        }
        public static bool DeleteShipper(int id)
        {
            return shipperDB.Delete(id);
        }

    }
}
