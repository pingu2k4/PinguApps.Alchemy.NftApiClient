using PinguApps.Alchemy.NftApiClient.Responses;
using Shouldly;

namespace PinguApps.Alchemy.NftApiClient.Tests;
public class GetNftsTests : TestBase
{
    const string MvhqContractAddress = "0x2809a8737477a534df65c4b4cae43d0365e52035";
    const string NftnerdsContractAddress = "0x495f947276749ce646f68ac8c248420045cb7b5e";

    [Theory]
    [MemberData(nameof(SpecifyJustAccountData))]
    public async Task SpecifyJustAccount(string account, bool expectSuccess, bool expectPageKey)
    {
        var result = await _client.GetNfts(account);

        ValidateResponse(result, expectSuccess);

        if (result.Success)
        {
            if (expectPageKey)
            {
                result.Result!.PageKey.ShouldNotBeNull();
            }
            else
            {
                result.Result!.PageKey.ShouldBeNull();
            }
        }
    }

    [Fact]
    public async Task PageSize()
    {
        var lowPageSize = await _client.GetNfts(FullWalletAddress, pageSize: 2);
        var oversizedPageSize = await _client.GetNfts(FullWalletAddress, pageSize: 10000);

        ValidateResponse(lowPageSize);
        ValidateResponse(oversizedPageSize);

        lowPageSize.Result!.Nfts.Count.ShouldBe(2);
        oversizedPageSize.Result!.Nfts.Count.ShouldBe(100);
    }

    [Fact]
    public async Task ContractAddressFiltering()
    {
        var result = await _client.GetNfts(FullWalletAddress, contractAddresses: new List<string> { MvhqContractAddress, NftnerdsContractAddress });

        ValidateResponse(result);

        foreach (var nft in result.Result!.Nfts)
        {
            nft.Contract.Address.ShouldBeOneOf(MvhqContractAddress, NftnerdsContractAddress);
        }
    }

    public static IEnumerable<object[]> SpecifyJustAccountData => new List<object[]>
    {
        new object[] { FullWalletAddress, true, true },
        new object[] { ErrorWalletAddressTooShort, false, false },
        new object[] { ErrorWalletAddressBadCharacter, false, false },
        new object[] { NearlyEmptyWalletAddress, true, false },
        new object[] { EnsWalletAddress, true, true },
        new object[] { BlankWalletAddress, true, false }
    };
}
