
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PostgresClient.Models
{
    public interface IHttpService : IDisposable
    {
        Task<RequestResult<HttpResponseMessage>> SendRequestAsync(HttpMethod method, string endpoint);
        Task<RequestResult<HttpResponseMessage>> SendRequestAsync(HttpMethod method, string endpoint, object payload);
        Task<RequestResult<T>> SendRequestAsync<T>(HttpMethod method, string endpoint) where T : class;
        Task<RequestResult<T>> SendRequestAsync<T>(HttpMethod method, string endpoint, object payload, bool noAuth = false) where T : class;
    }
}