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
namespace System_EMS_1._0.Services
{
    public class ApiService : IApiService
    {

        private readonly HttpClient _httpClient;
        private readonly ISessionStorageService _sessionStorageService;
        private readonly ApiSettings apiSettings;

        public ApiService(HttpClient http, ISessionStorageService sessionStorageService, IOptions<ApiSettings> apiSettingsOptions)
        {
            _httpClient = http;
            _sessionStorageService = sessionStorageService;
            apiSettings = apiSettingsOptions.Value;
        }
        public async Task<ResponseApi> SaveUser(UserModel user,string password, string token)
        {
            try
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
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/user", content);
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
        public async Task<ResponseApi> GetDepartment(string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/department");
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
        public async Task<ResponseApi> GetGroup(string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/groups");
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
        public async Task<ResponseApi> GetUser(string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/user");
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
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/user/logout");
                request.Headers.Add("userid", userId);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                ResponseApi? result = await response.Content.ReadFromJsonAsync<ResponseApi>();
                if (result != null && result.Code == 200)
                {
                    await _sessionStorageService.RemoveItemAsync("us");
                }
                else
                {

                }

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
        public async Task<ResponseApi> SaveDepartment(DepartmentModel department, string token)
        {
            try
            {
                var request = new
                {
                    departmentname = department.Departmentname,
                    departmentdescription = department.Departmentdescription,
                    departmentsortname = department.Departmentsortname,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/department", content);
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
        public async Task<ResponseApi> SaveGroup(GroupModel group, string token)
        {
            try
            {
                var request = new
                {
                    groupname = group.Groupname,
                    groupdescription = group.Groupdescription,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/groups", content);
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
        public async Task<ResponseApi> UpdateDepartment(string id, string name, string desc, string sort, string token)
        {
            try
            {
                var request = new
                {
                    departmentid = id,
                    departmentname = name,
                    departmentdescription = desc,
                    departmentsortname = sort,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/department", content);
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
        public async Task<ResponseApi> UpdateGroup(string id, string name,string desc, string token)
        {
            try
            {
                var request = new
                {
                    groupid = id,
                    groupname = name,
                    groupdescription = desc,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/groups", content);
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
        public async Task<ResponseApi> UpdateUser(string id, string display, string email, string phone, string deid, string token)
        {
            try
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
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/user", content);
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
        public async Task<ResponseApi> ListRoles(string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/roles");
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
        public async Task<ResponseApi> ListRolesUser(string? id, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/roles/user");
                request.Headers.Add("userid", id);
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
        public async Task<ResponseApi> GrantRolesUser(string type, string id, string rolename, string token)
        {
            try
            {
                var request = new
                {
                    type = type,
                    userid = id,
                    rolename = rolename,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/roles/user", content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ResponseApi>();
                if (result == null)
                {
                    throw new Exception("Failed to deserialize the response content.");
                }
                return result;

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
        public async Task<ResponseApi> GetUsersInGroup(string id, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/groups/user");
                request.Headers.Add("token", token);
                request.Headers.Add("groupid", id);
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
        public async Task<ResponseApi> Add_RemoveUserInGroup(string type, string idgr, string iduser, string token)
        {

            try
            {
                var request = new
                {
                    type = type,
                    groupid = idgr,
                    userId = iduser,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/groups/user", content);
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
        public async Task<ResponseApi> ListRolesGroup(string groupid, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/Roles/Group");
                request.Headers.Add("token", token);
                request.Headers.Add("groupid", groupid);
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
        public async Task<ResponseApi> GrantRolesGroup(string type, string id, string rolename, string token)
        {
            try
            {
                var request = new
                {
                    type = type,
                    groupid = id,
                    rolename = rolename,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/Roles/Group", content);
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
        public async Task<ResponseApi> ChangePassword(string username, string passold, string passnew, string token)
        {
            try
            {
                var request = new
                {
                    username = username,
                    oldpassword = passold,
                    newpassword = passnew,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/User/change_pass", content);
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
        public async Task<ResponseApi> ChangeStatus(string userid, bool status, string token)
        {
            try
            {
                var request = new
                {
                    userid = userid,
                    status = status,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/User/status", content);
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
        public async Task<ResponseApi> ResetPass(string userid, string token)
        {
            try
            {
                var request = new
                {
                    userid = userid,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/user/reset_pass", content);
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
        public async Task<ResponseApi> GetRotation(int deid, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices/rotation");
                request.Headers.Add("token", token);
                request.Headers.Add("deviceid", deid.ToString());
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
        public async Task<ResponseApi> SaveRotation(int deid, string departid, string token)
        {
            try
            {
                var request = new
                {
                    deviceid = deid,
                    departmentid = departid,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices/rotation", content);
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
        public async Task<ResponseApi> GetDeviceDepartment(string deid, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices/department");
                request.Headers.Add("token", token);
                request.Headers.Add("departmentid", deid);
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
            try
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
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices/repair", content);
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
        public async Task<ResponseApi> DetailsRepairDevice(string id, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices/repair/details");
                request.Headers.Add("token", token);
                request.Headers.Add("repairid", id);
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
        public async Task<ResponseApi> ApprovedRepairDevice(string id, string status, string token)
        {
            try
            {
                var request = new
                {
                    token = token,
                    repairId = id,
                    status = status
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices/repair/status", content);
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
        public async Task<ResponseApi> DetailsRepair(string id, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices/repair/details");
                request.Headers.Add("token", token);
                request.Headers.Add("repairid", id);
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
        public async Task<ResponseApi> StatusRepair(string status, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices/repair");
                request.Headers.Add("status", status);
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
        public async Task<ResponseApi> GetListDevice(string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices");
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
        public async Task<ResponseApi> DetailsDevice(string deviceid, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices/details");
                request.Headers.Add("deviceid", deviceid);
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
        public async Task<ResponseApi> ApiConfig(string type, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/config");
                request.Headers.Add("type", type);
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
        public async Task<ResponseApi> SaveDevice(DeviceModel device, string token)
        {
            try
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
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices", content);
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
        public async Task<ResponseApi> UpdateDevice(DeviceModel device, string token)
        {
            try
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
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/devices", content);
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
    }
}

