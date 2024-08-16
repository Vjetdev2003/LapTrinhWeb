using SV21T1020171.DataLayers.SQLServer;

using SV21T1020171.BusinessLayers;
using SV21T1020171.DataLayers;
using SV21T1020171.DomainModels;
using SV21T1020171.DataLayers.SQLServer;

namespace SV21T1020171.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL employeeAccountDB;

        static UserAccountService()
        {
            employeeAccountDB = new EmployeeAccountDAL(Configuration.ConnectionString);
        }

        public static UserAccount? Authorize(string userName, string password)
        {
            return employeeAccountDB.Authorize(userName, password);
        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return employeeAccountDB.ChangePassword(userName, oldPassword, newPassword);
        }

    }
}
