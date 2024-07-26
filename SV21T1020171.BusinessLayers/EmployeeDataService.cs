using System;
using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;

namespace SV21T1020171.BusinessLayers
{
    public static class EmployeeDataService
    {
        private static readonly EmployeeDAL employeeDB;
   
        //Constructor static khoong dc co tham so
        static EmployeeDataService()
        {
            string connnectionString = @"server=.;
                                        user id=sa;
                                        password=sa;
                                        database=LiteCommerceDB;
                                        TrustServerCertificate=true";
            employeeDB = new EmployeeDAL(connnectionString);
           
        }
        /// <summary>
        /// Lấy danh sách khách hàng
        /// </summary>
        /// <returns></returns>
        public static List<Employee> ListOfEmployee()
        {

            return employeeDB.List();
        }
        public static Employee Detail(int id)
        {
            return employeeDB.Detail(id);
        }
    }
}
