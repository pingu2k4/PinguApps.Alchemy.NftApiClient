using PinguApps.Alchemy.NftApiClient.Responses;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PinguApps.Alchemy.NftApiClient
{
    internal interface IInternalApi
    {
        // /getNFTs?owner=owner&pageKey=pageky&pageSize=1&contractAddresses[]=asd&contractAddresses[]=asd&withMetadata=true&tokenUriTimeoutInMs=100&filters[]=SPAM
        [Get("/getNFTs")]
        Task<IApiResponse<GetNftsResult>> GetNfts(string owner,
                                           string? pageKey,
                                           int? pageSize, 
                                           [Query(CollectionFormat.Multi), AliasAs("contractAddresses[]")] IEnumerable<string>? contractAddresses,
                                           bool? withMetadata,
                                           int? tokenUriTimeoutInMs,
                                           [Query(CollectionFormat.Multi), AliasAs("filters[]")] IEnumerable<string>? filters);
    }
}
