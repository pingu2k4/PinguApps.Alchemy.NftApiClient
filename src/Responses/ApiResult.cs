using System.Net;

namespace PinguApps.Alchemy.NftApiClient.Responses
{
    public class ApiResult<T> : ApiResult
    {
        public ApiResult(bool success, HttpStatusCode statusCode) : base(success, statusCode)
        {

        }

        public T? Result { get; set; }
    }

    public class ApiResult
    {
        public ApiResult(bool success, HttpStatusCode statusCode)
        {
            Success = success;
            StatusCode = statusCode;
        }

        public bool Success { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
