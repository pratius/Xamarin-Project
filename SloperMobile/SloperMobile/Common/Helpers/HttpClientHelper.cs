﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SloperMobile.Common.Helpers
{
    public class HttpClientHelper
    {
        protected readonly string _endpoint;
        protected readonly string _accesstoken;

        public HttpClientHelper(string endpoint, string accesstoken)
        {
            _endpoint = endpoint;
            _accesstoken = accesstoken;
        }

        public async Task<T> Post<T>(string jsonobject)
        {
            using (var httpClient = new HttpClient())
            {
                var endpoint = _endpoint;
                var accessToken = _accesstoken;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (accessToken != "")
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                var response = await httpClient.PostAsync(endpoint, new StringContent(jsonobject, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task<List<T>> Get<T>()
        {
            using (var httpClient = new HttpClient())
            {
                var endpoint = _endpoint;
                var accessToken = _accesstoken;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (accessToken != "")
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<List<T>>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task<T> Get<T>(Dictionary<string, string> query)
        {
            using (var httpClient = new HttpClient())
            {
                var endpoint = _endpoint;
                var accessToken = _accesstoken;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (accessToken != "")
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                if (query != null)
                {
                    var querystr = CreateQueryString(query);
                    endpoint += "?" + querystr;
                }
                var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                //var contents = await response.Content.ReadAsStringAsync();
                //return JsonConvert.DeserializeObject<T>(contents);
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task<string> GetJsonString<T>(Dictionary<string, string> query)
        {
            using (var httpClient = new HttpClient())
            {
                var endpoint = _endpoint;
                var accessToken = _accesstoken;
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                if (accessToken != "")
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                }
                if (query != null)
                {
                    var querystr = CreateQueryString(query);
                    endpoint += "?" + querystr;
                }
                var response = await httpClient.GetAsync(endpoint, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
                //var contents = await response.Content.ReadAsStringAsync();
                //return JsonConvert.DeserializeObject<T>(contents);
                return response.Content.ReadAsStringAsync().Result.ToString();
            }
        }

        public static string CreateQueryString(IDictionary<string, string> dict)
        {
            var list = new List<string>();
            foreach (var item in dict)
            {
                list.Add(item.Key + "=" + item.Value.ToString());
            }
            return string.Join("&", list);
        }
        protected HttpClient NewHttpClient()
        {
            return new HttpClient();
        }
    }
}
