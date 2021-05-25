
using PostgresClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PostgresClient.Helpers
{
    public class HttpService : IHttpService, IDisposable
    {
        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private readonly HttpClient _httpClient;

        private JsonSerializerOptions _jOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<RequestResult<HttpResponseMessage>> SendRequestAsync(HttpMethod method, string endpoint)
        {
            return await SendRequestAsync<HttpResponseMessage>(method, endpoint, null);
        }

        public async Task<RequestResult<T>> SendRequestAsync<T>(HttpMethod method, string endpoint) where T : class
        {
            return await SendRequestAsync<T>(method, endpoint, null);
        }

        public async Task<RequestResult<HttpResponseMessage>> SendRequestAsync(HttpMethod method, string endpoint, object payload)
        {
            return await SendRequestAsync<HttpResponseMessage>(method, endpoint, payload);
        }

        public async Task<RequestResult<T>> SendRequestAsync<T>(HttpMethod method, string endpoint, object payload, bool noAuth = false) where T : class
        {
            var request = new HttpRequestMessage(method, endpoint);
            if(payload != null)
            {
                request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"); 
            }
            try
            {
                var result = await _httpClient.SendAsync(request);
                var textResult = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    if(typeof(T) == typeof(HttpResponseMessage))
                    {
                        return new RequestResult<T>(result as T);
                    }
                    var dataResult = JsonSerializer.Deserialize<T>(textResult, _jOptions);
                    return new RequestResult<T>(dataResult);
                }
                else
                {
                    var message = textResult;

                    if (result.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return new RequestResult<T>(InvalidRequestReasons.BadRequest, message);
                    }
                    else if (result.StatusCode == HttpStatusCode.RequestTimeout)
                    {
                        return new RequestResult<T>(InvalidRequestReasons.TimeOut, message);
                    }
                    else if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        return new RequestResult<T>(InvalidRequestReasons.AccessDenied, message);
                    }
                    else
                    {
                        return new RequestResult<T>(InvalidRequestReasons.Other, message);
                    }
                }
            }
            catch (Exception e)
            {
                return new RequestResult<T>(InvalidRequestReasons.Other, e.ToString());
            }
        }


        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
