using Blazored.SessionStorage;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System_EMS_1._0.Data;

namespace System_EMS_1._0.Services
{


    public class Api : IApi
    {
        private readonly ApiSettings apiSettings;
        private readonly HttpClient _httpClient;

        public Api(HttpClient http, IOptions<ApiSettings> apiSettingsOptions)
        {
            _httpClient = http;
            apiSettings = apiSettingsOptions.Value;

        }

        public async Task<ResponseApi> Get(string api, Dictionary<string, string> header)
        {
            string http = $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/" + api;
            var request = new HttpRequestMessage(HttpMethod.Get, http);
            if (header != null)
            {
                foreach (var item in header)
                {
                    request.Headers.Add(item.Key, item.Value);
                }
            }
            try
            {
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


        public async Task<ResponseApi> Post(string api, object value)
        {

            string http = $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/" + api;
            var jsonContent = JsonConvert.SerializeObject(value);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(http, content);
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
        public async Task<ResponseApi> Put(string api, object value)
        {

            string http = $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/" + api;
            var jsonContent = JsonConvert.SerializeObject(value);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PutAsync(http, content);
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

        public async Task<ResponseApi> Delete(string api, object value)
        {
            var jsonContent = JsonConvert.SerializeObject(value);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            string http = $"{apiSettings.BaseUrl}:{apiSettings.PortUrl}/api/" + api;
            var request = new HttpRequestMessage(HttpMethod.Delete, http);
            request.Content = content;



            try
            {
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






    }
}
