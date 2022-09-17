using PinguApps.Alchemy.NftApiClient.Extensions;
using PinguApps.Alchemy.NftApiClient.Responses;
using Polly;
using Polly.Wrap;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PinguApps.Alchemy.NftApiClient
{
    public sealed class NftApiClient : INftApiClient
    {
        /// <summary>
        /// Unless otherwise specified, the initial delay to use when backing off due either to transient errors or rate limits
        /// </summary>
        public const int DefaultBackoffInitialDelayMs = 1000;
        /// <summary>
        /// Unless otherwise specified, the multiplier to the delay to use when backing off due either to transient errors or rate limits
        /// </summary>
        public const float DefaultBackoffMultiplier = 1.5f;
        /// <summary>
        /// Unless otherwise specified, the maximum time before timing out a request
        /// </summary>
        public const int DefaultBackoffTimeoutMs = 30 * 1000;
        /// <summary>
        /// Unless otherwise specified, the maximum number of attempts to use when backing off due either to transient errors or rate limits
        /// </summary>
        public const int DefaultBackoffMaxAttempts = 5;

        private readonly IInternalApi _internalApi;
        private readonly BackoffSettings? _backoffSettings;

        /// <summary>
        /// Instantiates a client for the Nft API
        /// </summary>
        /// <param name="endpoint">Your Alchemy endpoint, including API key</param>
        /// <param name="backoffSettings">Optionally, settings to override the default behaviour for backing off due either to transient errors or rate limits</param>
        public NftApiClient(string endpoint, BackoffSettings? backoffSettings = null)
        {
            if (!endpoint.Contains("nft/v2"))
            {
                endpoint = endpoint.Replace("/v2", "/nft/v2");
            }

            _internalApi = RestService.For<IInternalApi>(endpoint);
            _backoffSettings = backoffSettings;
        }

        /// <summary>
        /// Instantiates a client for the Nft API
        /// </summary>
        /// <param name="network">The network you are connecting to</param>
        /// <param name="apiKey">Your Alchemy API key</param>
        /// <param name="backoffSettings">Optionally, settings to override the default behaviour for backing off due either to transient errors or rate limits</param>
        public NftApiClient(Network network, string apiKey, BackoffSettings? backoffSettings = null)
        {
            var endpoint = $"https://{network.GetDescription()}.g.alchemy.com/nft/v2/{apiKey}";

            _internalApi = RestService.For<IInternalApi>(endpoint);
            _backoffSettings = backoffSettings;
        }

        private AsyncPolicyWrap<T> BuildPolicy<T>() where T : IApiResponse
        {
            var timeoutPolicy = Policy.TimeoutAsync<T>(_backoffSettings?.BackoffTimeoutMs ?? DefaultBackoffTimeoutMs);

            var retryPolicy = Policy.Handle<HttpRequestException>()
                         .OrResult<T>(x => (int) x.StatusCode >= 500 || x.StatusCode == HttpStatusCode.RequestTimeout || (int) x.StatusCode == 429)
                         .WaitAndRetryAsync(_backoffSettings?.BackoffMaxAttempts ?? DefaultBackoffMaxAttempts,
                                            x => TimeSpan.FromMilliseconds(Math.Pow(_backoffSettings?.BackoffMultiplier ?? DefaultBackoffMultiplier, x - 1)
                                                                                    * _backoffSettings?.BackoffInitialDelayMs ?? DefaultBackoffInitialDelayMs));

            return Policy.WrapAsync(timeoutPolicy, retryPolicy);
        }

        private Task<T> WrapRequestInRetry<T>(Func<Task<T>> request) where T : IApiResponse
        {
            return BuildPolicy<T>().ExecuteAsync(request);
        }

        /// <summary>
        /// Gets all NFTs currently owned by a given address.
        /// </summary>
        /// <param name="owner">Address for NFT owner (can be in ENS format!).</param>
        /// <param name="pageKey">Cursor key for pagination. If more results are available, a pageKey will be returned in the response. Pass back that pageKey as a param to fetch the next 100 NFTs.</param>
        /// <param name="pageSize">Number of NFTs to be returned per page. Defaults to 100. Max is 100. NOTE: Only supported on Ethereum Mainnet and Goerli, Polygon Mainnet and Mumbai, Arbitrum Mainnet and Goerli, and Optimism Mainnet and Goerli.</param>
        /// <param name="contractAddresses">Array of contract addresses to filter the responses with. Max limit 20 contracts.</param>
        /// <param name="withMetadata">If set to <c>true</c>, returns NFT metadata. Setting this to <c>false</c> will reduce payload size and may result in a faster API call. Defaults to false.</param>
        /// <param name="tokenUriTimeoutInMs">No set timeout by default - When metadata is requested, this parameter is the timeout (in milliseconds) for the website hosting the metadata to respond. If you want to only access the cache and not live fetch any metadata for cache misses then set this value to 0.</param>
        /// <param name="filters">Array of filters (as ENUMS) that will be applied to the query. NFTs that match one or more of these filters will be excluded from the response.</param>
        /// <returns><see cref="ApiResult{GetNftsResult}"/></returns>
        public async Task<ApiResult<GetNftsResult>> GetNfts(string owner, string? pageKey = null, int? pageSize = null, IEnumerable<string>? contractAddresses = null,
                                  bool? withMetadata = null, int? tokenUriTimeoutInMs = null, IEnumerable<string>? filters = null)
        {
            try
            {
                var result = await WrapRequestInRetry(() =>
                    _internalApi.GetNfts(owner, pageKey, pageSize, contractAddresses, withMetadata, tokenUriTimeoutInMs, filters));

                return Helpers.GetResult(result);
            }
            catch (Exception e)
            {
                return Helpers.GetCaughtExceptionResponse<GetNftsResult>(e);
            }
        }
    }
}
