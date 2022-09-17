using PinguApps.Alchemy.NftApiClient.Responses;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PinguApps.Alchemy.NftApiClient
{
    internal static class Helpers
    {
        internal static ApiResult<T> GetResult<T>(IApiResponse<T> response)
        {
            if (response.IsSuccessStatusCode && response.Content is not null)
            {
                return new ApiResult<T>(true, response.StatusCode)
                {
                    Result = response.Content
                };
            }
            else
            {
                return GetErrorResponse(response);
            }
        }

        internal static ApiResult<T> GetErrorResponse<T>(IApiResponse<T> response)
        {
            if (response.Error is null)
            {
                return new ApiResult<T>(false, response.StatusCode)
                {
                    ErrorMessage = "Unknown error encountered."
                };
            }
            else
            {
                return new ApiResult<T>(false, response.StatusCode)
                {
                    ErrorMessage = response.Error.Content
                };
            }
        }

        internal static ApiResult<T> GetErrorResponse<T, U>(IApiResponse<U> response)
        {
            if (response.Error is null)
            {
                return new ApiResult<T>(false, response.StatusCode)
                {
                    ErrorMessage = "Unknown error encountered."
                };
            }
            else
            {
                return new ApiResult<T>(false, response.StatusCode)
                {
                    ErrorMessage = response.Error.Content
                };
            }
        }

        internal static ApiResult<T> GetCaughtExceptionResponse<T>(Exception e)
        {
            return new ApiResult<T>(false, HttpStatusCode.InternalServerError)
            {
                ErrorMessage = e.Message
            };
        }
    }
}
