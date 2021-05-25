using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostgresClient.Models
{
    public enum InvalidRequestReasons
    {
        None,
        AccessDenied,
        BadRequest,
        TimeOut,
        Other
    }

    public class RequestResult<T> where T : class
    {
        /// <summary>
        /// Returns a valid RequestResult
        /// </summary>
        /// <param name="data">Payload to be passed back</param>
        public RequestResult(T data)
        {
            IsValid = true;
            Data = data;
            InvalidRequestResult = InvalidRequestReasons.None;
        }

        /// <summary>
        /// Returns an invalud RequestResult with corresponding reason
        /// </summary>
        /// <param name="reason">Why did it fail, authentication failures will be redirected to the login page</param>
        public RequestResult(InvalidRequestReasons reason, string message)
        {
            IsValid = false;
            Data = null;
            InvalidRequestResult = reason;
            ErrorMessage = message;
        }

        public bool IsValid { get; private set; }
        public T Data { get; private set; }
        public InvalidRequestReasons InvalidRequestResult { get; private set; }
        public string ErrorMessage { get; private set; }
    }
}
