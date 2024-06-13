using System_EMS_1._0.Data;
using Newtonsoft.Json;
using Blazored.SessionStorage;
using System_EMS_1._0.Pages;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Xml.Linq;
using System.Numerics;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http;
using Microsoft.VisualBasic;
using Radzen;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
namespace System_EMS_1._0.Services
{
    public interface IDeviceService
    {
        Task<ResponLogin> Login(string userName, string passWord);
        Task<ResponseApi> Logout(string userId);
        Task<ResponseApi> GetDepartment(string token);
        Task<ResponseApi> SaveDepartment(DepartmentModel departmentModel, string token);
        Task<ResponseApi> UpdateDepartment(string id, string name, string desc, string sort, string token);
        Task<ResponseApi> GetGroup(string token);
        Task<ResponseApi> SaveGroup(GroupModel group, string token);
        Task<ResponseApi> UpdateGroup(string id, string name, string desc, string token);
        Task<ResponseApi> GetUser(string token);
        Task<ResponseApi> SaveUser(UserModel user, string password, string token);
        Task<ResponseApi> UpdateUser(string id, string display, string email, string phone, string deid, string token);
        Task<ResponseApi> ListRoles(string token);
        Task<ResponseApi> ListRolesUser(string? id, string token);
        Task<ResponseApi> GrantRolesUser(string type, string id, string rolename, string token);
        Task<ResponseApi> GetUsersInGroup(string id, string token);
        Task<ResponseApi> Add_RemoveUserInGroup(string type, string idgr, string iduser, string token);
        Task<ResponseApi> ListRolesGroup(string groupid, string token);
        Task<ResponseApi> GrantRolesGroup(string type, string id, string rolename, string token);
        Task<ResponseApi> ChangePassword(string username, string passold, string passnew, string token);
        Task<ResponseApi> ChangeStatus(string userid, bool status, string token);
        Task<ResponseApi> ResetPass(string userid, string token);
        Task<ResponseApi> GetRotation(int deid, string token);
        Task<ResponseApi> SaveRotation(int deid, string departid, string token);
        Task<ResponseApi> GetDeviceDepartment(string deid, string token);
        Task<ResponseApi> SaveRepairDevice(string token);
        Task<ResponseApi> UpdateRepairDevice(string id, int deviceid, string desc, double cost, string type, string token);
        Task<ResponseApi> DetailsRepairDevice(string id, string token);
        Task<ResponseApi> ApprovedRepairDevice(string id, string status, string token);
        Task<ResponseApi> StatusRepair(string status, string token);
        Task<ResponseApi> GetListDevice(string token);
        Task<ResponseApi> DetailsDevice(string deviceid, string token);
        Task<ResponseApi> ApiConfig(string type, string token);
        Task<ResponseApi> SaveDevice(DeviceModel device, string token);
        Task<ResponseApi> UpdateDevice(DeviceModel device, string token);
        Task<ResponseApi> DeleteDetailsRepairDevice(string repa_id, int deid, string token);
    }
    public class DeviceService : IDeviceService
    {

        private readonly HttpClient _httpClient;
        private readonly ISessionStorageService _sessionStorageService;
        private readonly ApiSettings apiSettings;
        private readonly IApi callapi;

        public DeviceService(HttpClient http, ISessionStorageService sessionStorageService, IOptions<ApiSettings> apiSettingsOptions, IApi api)
        {

            _httpClient = http;
            apiSettings = apiSettingsOptions.Value;

            callapi = api;
            _sessionStorageService = sessionStorageService;

        }
        public async Task<ResponseApi> DetailsRepairDevice(string id, string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("repairid", id);
            header.Add("token", token);
            ResponseApi result = await callapi!.Get("devices/repair/details", header);
            return result;
        }
        public async Task<ResponseApi> DetailsDevice(string deviceid, string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("deviceid", deviceid);
            header.Add("token", token);

            ResponseApi result = await callapi!.Get("devices/details", header);
            return result;
        }
        public async Task<ResponseApi> SaveUser(UserModel user, string password, string token)
        {

            var request = new
            {
                username = user.Username,
                password = password,
                displayname = user.Displayname,
                email = user.Email,
                phonenumber = user.Phonenumber,
                departmentid = user.Departmentid,
                token = token
            };
            ResponseApi result = await callapi!.Post("user", request);
            return result;
        }
        public async Task<ResponseApi> GetDepartment(string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);

            ResponseApi result = await callapi!.Get("department", header);
            return result;
        }
        public async Task<ResponseApi> GetGroup(string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);
            ResponseApi result = await callapi!.Get("groups",header);
            return result;
        }
        public async Task<ResponseApi> GetUser(string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);
            ResponseApi result = await callapi!.Get("user", header);
            return result;
        }
        public async Task<ResponLogin> Login(string userName, string passWord)
        {
            try
            {
                var request = new
                {
                    username = userName,
                    password = passWord
                };

                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/user/login", content);
                response.EnsureSuccessStatusCode();

                ResponLogin? result = await response.Content.ReadFromJsonAsync<ResponLogin>();
                if (result != null)
                {
                    if (result.Code == 200)
                    {
                        var data = new
                        {
                            Token = result.Token,
                            Userid = result.Userid,
                            Displayname = result.Displayname,
                            Username = result.Username
                        };
                        await _sessionStorageService.SetItemAsync("us", data);
                    }
                }
                return result!;
            }
            catch (Exception ex)
            {
                ResponLogin response = new ResponLogin()
                {
                    Message = ex.Message,
                };
                return response;
            }

        }
        public async Task<ResponseApi> Logout(string userId)
        {
            var request = new
            {
                userid = userId
            };
            ResponseApi result = await callapi!.Post("user/logout", request);
            if (result != null && result.Code == 200)
            {
                await _sessionStorageService.RemoveItemAsync("us");
            }
            else
            {

            }
            return result!;

        }
        public async Task<ResponseApi> SaveDepartment(DepartmentModel department, string token)
        {

            var request = new
            {
                departmentname = department.Departmentname,
                departmentdescription = department.Departmentdescription,
                departmentsortname = department.Departmentsortname,
                token = token
            };
            ResponseApi result = await callapi!.Post("department", request);
            return result;
        }
        public async Task<ResponseApi> SaveGroup(GroupModel group, string token)
        {

            var request = new
            {
                groupname = group.Groupname,
                groupdescription = group.Groupdescription,
                token = token
            };
            ResponseApi result = await callapi!.Post("groups", request);
            return result;
        }
        public async Task<ResponseApi> UpdateDepartment(string id, string name, string desc, string sort, string token)
        {

            var request = new
            {
                departmentid = id,
                departmentname = name,
                departmentdescription = desc,
                departmentsortname = sort,
                token = token
            };
            ResponseApi result = await callapi!.Put("department", request);
            return result;
        }
        public async Task<ResponseApi> UpdateGroup(string id, string name, string desc, string token)
        {

            var request = new
            {
                groupid = id,
                groupname = name,
                groupdescription = desc,
                token = token
            };
            ResponseApi result = await callapi!.Put("groups", request);
            return result;
        }
        public async Task<ResponseApi> UpdateUser(string id, string display, string email, string phone, string deid, string token)
        {

            var request = new
            {
                userid = id,
                displayname = display,
                email = email,
                phonenumber = phone,
                departmentid = deid,
                token = token
            };
            ResponseApi result = await callapi!.Put("user", request);
            return result;
        }
        public async Task<ResponseApi> ListRoles(string token)
        {

            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);
            ResponseApi result = await callapi!.Get("roles", header);
            return result;
        }
        public async Task<ResponseApi> ListRolesUser(string? id, string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);
            header.Add("userid", id!);

            ResponseApi result = await callapi!.Get("roles/user",header);
            return result;
        }
        public async Task<ResponseApi> GrantRolesUser(string type, string id, string rolename, string token)
        {

            var request = new
            {
                type = type,
                userid = id,
                rolename = rolename,
                token = token
            };
            ResponseApi result = await callapi!.Put("roles/user", request);
            return result;
        }
        public async Task<ResponseApi> GetUsersInGroup(string id, string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);
            header.Add("groupid", id);

            ResponseApi result = await callapi!.Get("groups/user", header );
            return result;

        }
        public async Task<ResponseApi> Add_RemoveUserInGroup(string type, string idgr, string iduser, string token)
        {

            var request = new
            {
                type = type,
                groupid = idgr,
                userId = iduser,
                token = token
            };
            ResponseApi result = await callapi!.Put("groups/user    ", request);
            return result;
        }
        public async Task<ResponseApi> ListRolesGroup(string groupid, string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);
            header.Add("groupid", groupid);

            ResponseApi result = await callapi!.Get("Roles/Group", header);
            return result;
        }
        public async Task<ResponseApi> GrantRolesGroup(string type, string id, string rolename, string token)
        {

            var request = new
            {
                type = type,
                groupid = id,
                rolename = rolename,
                token = token
            };
            ResponseApi result = await callapi!.Put("Roles/Group", request);
            return result;
        }
        public async Task<ResponseApi> ChangePassword(string username, string passold, string passnew, string token)
        {

            var request = new
            {
                username = username,
                oldpassword = passold,
                newpassword = passnew,
                token = token
            };
            ResponseApi result = await callapi!.Put("User/change_pass", request);
            return result;

        }
        public async Task<ResponseApi> ChangeStatus(string userid, bool status, string token)
        {

            var request = new
            {
                userid = userid,
                status = status,
                token = token
            };
            ResponseApi result = await callapi!.Put("User/status", request);
            return result;
        }
        public async Task<ResponseApi> ResetPass(string userid, string token)
        {

            var request = new
            {
                userid = userid,
                token = token
            };
            ResponseApi result = await callapi!.Put("User/reset_pass", request);
            return result;
        }
        public async Task<ResponseApi> GetRotation(int deid, string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);
            header.Add("deviceid", deid.ToString());


            ResponseApi result = await callapi!.Get("devices/rotation",header);
            return result;
        }
        public async Task<ResponseApi> SaveRotation(int deid, string departid, string token)
        {

            var request = new
            {
                deviceid = deid,
                departmentid = departid,
                token = token
            };
            ResponseApi result = await callapi!.Put("devices/rotation", request);
            return result;

        }
        public async Task<ResponseApi> GetDeviceDepartment(string deid, string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);
            header.Add("departmentid", deid);

            ResponseApi result = await callapi!.Get("devices/department", header);
            return result;
        }
        public async Task<ResponseApi> SaveRepairDevice(string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices/repair");
                request.Headers.Add("token", token);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                ResponseApi? result = await response.Content.ReadFromJsonAsync<ResponseApi>();
                return result!;
            }
            catch (Exception ex)
            {
                ResponseApi response = new ResponseApi()
                {
                    Message = ex.Message,
                };
                return response;
            }

        }
        public async Task<ResponseApi> UpdateRepairDevice(string id, int deviceid, string desc, double cost, string type, string token)
        {

            var request = new
            {
                id = id,
                deviceid = deviceid,
                description = desc,
                cost = cost,
                type = type,
                token = token
            };
            ResponseApi result = await callapi!.Put("devices/repair", request);
            return result;
        }
        public async Task<ResponseApi> ApprovedRepairDevice(string id, string status, string token)
        {

            var request = new
            {
                token = token,
                repairId = id,
                status = status
            };
            ResponseApi result = await callapi!.Put("devices/repair/status", request);
            return result;
        }
        public async Task<ResponseApi> StatusRepair(string status, string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);
            header.Add("status",status);

            ResponseApi result = await callapi!.Get("devices/repair", header);
            return result;
        }
        public async Task<ResponseApi> GetListDevice(string token)
        {

            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);

            ResponseApi result = await callapi!.Get("devices", header);
            return result;
        }
        public async Task<ResponseApi> ApiConfig(string type, string token)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("token", token);
            header.Add("type", type);

            ResponseApi result = await callapi!.Get("config", header);
            return result;
        }
        public async Task<ResponseApi> SaveDevice(DeviceModel device, string token)
        {

            var request = new
            {
                devicename = device.Devicename == null ? "" : device.Devicename,
                devicetype = device.Devicetype,
                productgroup = device.Productgroup,
                startdate = device.Startdate,
                devicemodel = device.Devicemodel == null ? "" : device.Devicemodel,
                serialnumber = device.Serialnumber == null ? "" : device.Serialnumber,
                lr_code = device.Lr_code == null ? "" : device.Lr_code,
                company = device.Company == null ? "" : device.Company,
                national = device.National,
                info = device.Info == null ? "" : device.Info,
                comcode = device.Comcode == null ? "" : device.Comcode,
                maindevice = device.Maindevice,
                limit = device.Limit,
                wastage = device.Wastage,
                buydate = device.Buydate,
                expdate = device.Expdate,
                note = device.Note == null ? "" : device.Note,
                token = token
            };
            ResponseApi result = await callapi!.Post("devices", request);
            return result;
        }
        public async Task<ResponseApi> UpdateDevice(DeviceModel device, string token)
        {
            var request = new
            {
                id = device.Deviceid,
                devicename = device.Devicename,
                devicetype = device.Devicetype,
                productgroup = device.Productgroup,
                startdate = device.Startdate,
                devicemodel = device.Devicemodel,
                serialnumber = device.Serialnumber,
                lr_code = device.Lr_code,
                company = device.Company,
                national = device.National,
                info = device.Info,
                comcode = device.Comcode,
                maindevice = device.Maindevice,
                limit = device.Limit,
                wastage = device.Wastage,
                buydate = device.Buydate,
                expdate = device.Expdate,
                note = device.Note,
                token = token
            };
            ResponseApi result = await callapi!.Put("devices", request);
            return result;

        }

        public async Task<ResponseApi> DeleteDetailsRepairDevice(string repa_id, int deid, string token)
        {
            var request = new
            {
                repairid = repa_id,
                deviceid = deid,
                token = token
            };
            ResponseApi result = await callapi!.Delete("devices/repair/details", request);
            return result;
        }
    }
}

