using System_EMS_1._0.Data;
using Newtonsoft.Json;
using Blazored.SessionStorage;
using System_EMS_1._0.Pages;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Xml.Linq;
using System.Numerics;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
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
        public async Task<ResponLogout> SaveUser(string name, string pass, string display, string email, string phone, string de, string token)
        {
            try
            {
                var request = new
                {
                    username = name,
                    password = pass,
                    displayname = display,
                    email = email,
                    phonenumber = phone,
                    departmentid = de,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/user", content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ResponLogout>();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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
                var result = await response.Content.ReadFromJsonAsync<ResponseApi>();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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
                var result = await response.Content.ReadFromJsonAsync<ResponseApi>();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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
                var result = await response.Content.ReadFromJsonAsync<ResponseApi>();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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

                var result = await response.Content.ReadFromJsonAsync<ResponLogin>();
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
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        public async Task<ResponLogout> Logout(string userId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/user/logout");
                request.Headers.Add("userid", userId);

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ResponLogout>();
                return result;

            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        public async Task<ResponSaveDepartment> SaveDepartment(string name, string desc, string sort, string token)
        {
            try
            {
                var request = new
                {
                    departmentname = name,
                    departmentdescription = desc,
                    departmentsortname = sort,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/department", content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ResponSaveDepartment>();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        public async Task<ResponLogout> SaveGroup(string name, string desc, string token)
        {
            try
            {
                var request = new
                {
                    groupname = name,
                    groupdescription = desc,
                    token = token
                };
                var jsonContent = JsonConvert.SerializeObject(request);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/groups", content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ResponLogout>();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        public async Task<ResponLogout> UpdateDepartment(string id, string name, string desc, string sort, string token)
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
                return await response.Content.ReadFromJsonAsync<ResponLogout>();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        public async Task<ResponLogout> UpdateGroup(string id, string name, string desc, string token)
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
                return await response.Content.ReadFromJsonAsync<ResponLogout>();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        public async Task<ResponLogout> UpdateUser(string id, string display, string email, string  phone, string deid, string token)
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
                var result = await response.Content.ReadFromJsonAsync<ResponLogout>();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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
                var result = await response.Content.ReadFromJsonAsync<ResponseApi>();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        public async Task<ResponseApi> ListRolesUser(string id, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/roles/user");
                request.Headers.Add("userid", id);
                request.Headers.Add("token", token);
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<ResponseApi>();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
        public async Task<ResponLogout> GrantRolesUser(string type, string id, string rolename, string token)
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
                var result = await response.Content.ReadFromJsonAsync<ResponLogout>();
                return result;

            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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
                var result = await response.Content.ReadFromJsonAsync<ResponseApi>();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

        }

        public async Task<ResponLogout> Add_RemoveUserInGroup(string type,string idgr, string iduser, string token)
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
                return await response.Content.ReadFromJsonAsync<ResponLogout>();
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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
                var result = await response.Content.ReadFromJsonAsync<ResponseApi>();
                return result;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<ResponLogout> GrantRolesGroup(string type, string id, string rolename, string token)
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
                var result = await response.Content.ReadFromJsonAsync<ResponLogout>();
                return result;

            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<ResponLogout> ChangePassword(string username, string passold, string passnew, string token)
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
                var result = await response.Content.ReadFromJsonAsync<ResponLogout>();
                return result;

            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<ResponLogout> ChangeStatus(string userid, bool status, string token)
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
                var result = await response.Content.ReadFromJsonAsync<ResponLogout>();
                return result;

            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
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
                var result = await response.Content.ReadFromJsonAsync<ResponseApi>();
                return result;

            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }
    }
}

