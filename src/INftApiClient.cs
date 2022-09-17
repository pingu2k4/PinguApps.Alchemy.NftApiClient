using PinguApps.Alchemy.NftApiClient.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PinguApps.Alchemy.NftApiClient
{
    public interface INftApiClient
    {
        Task<ApiResult<GetNftsResult>> GetNfts(string owner, string? pageKey = null, int? pageSize = null, IEnumerable<string>? contractAddresses = null, bool? withMetadata = null, int? tokenUriTimeoutInMs = null, IEnumerable<string>? filters = null);
    }
}