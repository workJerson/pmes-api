using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace pmes.core.Helpers
{
    public interface IHttpClientHelper
    {
        public Task<HttpResponseMessage> PostAsync(string endpointUrl, dynamic data, string xApiKey, bool isCamelCase = false);
        public Task<HttpResponseMessage> PutAsync(string endpointUrl, dynamic data, string xApiKey);
        public Task<HttpResponseMessage> GetAsync(string endpointUrl, string xApiKey);
    }

    public class HttpClientHelper(HttpClient httpClient, IHttpContextAccessor httpContextAccessor) : IHttpClientHelper
    {
        private static readonly ILogger logger = Log.ForContext(typeof(HttpClientHelper));

        public async Task<HttpResponseMessage> GetAsync(string endpointUrl, string xApiKey)
        {
            logger.Information("GetAsync Request url: {0} xApiKey: {1}", endpointUrl, xApiKey);

            if (!string.IsNullOrEmpty(xApiKey))
                if (!httpClient.DefaultRequestHeaders.Contains("x-api-key"))
                    httpClient.DefaultRequestHeaders.Add("x-api-key", xApiKey);
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(httpContextAccessor.HttpContext.Session.GetString("scheme"), httpContextAccessor.HttpContext.Session.GetString("token"));

            var response = await httpClient.GetAsync(endpointUrl);

            logger.Information("GetAsync Response url: {0} xApiKey: {1} response: {2}", endpointUrl, xApiKey, response.Content.ReadAsStringAsync());

            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string endpointUrl, dynamic data, string xApiKey, bool isCamelCase = false)
        {
            if (!string.IsNullOrEmpty(xApiKey))
                if (!httpClient.DefaultRequestHeaders.Contains("x-api-key"))
                    httpClient.DefaultRequestHeaders.Add("x-api-key", xApiKey);

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(httpContextAccessor.HttpContext.Session.GetString("scheme"), httpContextAccessor.HttpContext.Session.GetString("token"));

            var json = JsonConvert.SerializeObject(data);

            if (isCamelCase) // check if payload properties need to be converted to camelcase
            {
                json = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            }


            logger.Information("PostAsync Request url: {0} payload: {1} xApiKey: {2}", endpointUrl, json, xApiKey);

            var stringConent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(endpointUrl, stringConent);

            logger.Information("PostAsync Response url: {0} payload: {1} xApiKey: {2} response: {3}", endpointUrl, json, xApiKey, response.Content.ReadAsStringAsync());

            return response;
        }

        public async Task<HttpResponseMessage> PutAsync(string endpointUrl, dynamic data, string xApiKey)
        {

            if (!string.IsNullOrEmpty(xApiKey))
                if (!httpClient.DefaultRequestHeaders.Contains("x-api-key"))
                    httpClient.DefaultRequestHeaders.Add("x-api-key", xApiKey);

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(httpContextAccessor.HttpContext.Session.GetString("scheme"), httpContextAccessor.HttpContext.Session.GetString("token"));

            var json = JsonConvert.SerializeObject(data);

            logger.Information("PutAsync Request url: {0} payload: {1} xApiKey: {2}", endpointUrl, json, xApiKey);

            var stringConent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(endpointUrl, stringConent);

            logger.Information("PutAsync Response url: {0} payload: {1} xApiKey: {2} response: {3}", endpointUrl, json, xApiKey, response.Content.ReadAsStringAsync());

            return response;
        }
    }
}
