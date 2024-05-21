using System_EMS_1._0.Data;

namespace System_EMS_1._0.Services
{
    public interface IApiService
    {
        Task<ResponLogin> Login(string userName, string passWord);
        Task<ResponLogout> Logout(string userId);
        Task<ResponseApi> GetDepartment(string token);
        Task<ResponSaveDepartment> SaveDepartment(string name, string desc, string sort,string token);
        Task<ResponLogout> UpdateDepartment(string id,string name, string desc, string sort, string token);
        Task <ResponseApi> GetGroup(string token);
        Task<ResponLogout> SaveGroup(string name,string desc,string token);
        Task<ResponLogout> UpdateGroup(string id,string name, string desc, string token);
        Task<ResponseApi> GetUser(string token);
        Task<ResponLogout> SaveUser(string name, string pass, string display, string email, string phone, string de, string token);
        Task<ResponLogout> UpdateUser(string id, string display, string phone, string email, string deid,string token);
        Task<ResponseApi> ListRoles(string token);
        Task<ResponseApi> ListRolesUser(string id,string token);
        Task<ResponLogout> GrantRolesUser(string type,string id, string rolename, string token);
        Task<ResponseApi> GetUsersInGroup(string id,string token);
        Task<ResponLogout> Add_RemoveUserInGroup(string type,string idgr,string iduser,string token);
        Task<ResponseApi> ListRolesGroup(string groupid, string token);
        Task<ResponLogout> GrantRolesGroup(string type, string id, string rolename, string token);
        Task<ResponLogout> ChangePassword(string username, string passold, string passnew, string token);
        Task<ResponLogout> ChangeStatus(string userid, bool status,string token);
        Task<ResponseApi> ResetPass(string userid, string token);
        Task<ResponseApi> GetRotation( int deid,string token);
        Task<ResponseApi> SaveRotation(int deid,string departid,string token);
        Task<ResponseApi> GetDeviceDepartment(string deid,string token);
        Task<ResponseApi> SaveRepairDevice(string token);
        Task<ResponseApi> UpdateRepairDevice(string id, int deviceid, string desc, double cost, string type, string token);
        Task<ResponseApi> DetailsRepairDevice(string id, string token);

        Task<ResponseApi> ApprovedRepairDevice(string id,string status,string token);

        Task<ResponseApi> DetailsRepair(string id,string token);
        Task<ResponseApi> StatusRepair (string status,string token);
        Task<ResponseApi> GetListDevice(string token);

        Task<ResponseApi> DetailsDevice(string deviceid, string token);
        Task<ResponseApi> ApiConfig(string type, string token);
    }
}
