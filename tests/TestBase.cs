using PinguApps.Alchemy.NftApiClient.Responses;
using Shouldly;

namespace PinguApps.Alchemy.NftApiClient.Tests;
public abstract class TestBase
{
	protected const string FullWalletAddress = "0xAff1BF27f0aE7ce8AC0b7d3Ec7638933eDE0194E";
	protected const string ErrorWalletAddressTooShort = "0xAff1BF27f0aE7ce8AC0b7d3Ec7638933eDE0194";
	protected const string ErrorWalletAddressBadCharacter = "0xAff1BF27f0aE7ce8AC0b7d3Ec7638933eDE0194Z";
	protected const string NearlyEmptyWalletAddress = "0x576F9215250777b005EcCbF5b2eca5E6638a86cB";
	protected const string EnsWalletAddress = "pingu.eth";
	protected const string BlankWalletAddress = "0xDeAd9215250777b005EcCbF5b2eca5E6638aB33F";


    protected readonly NftApiClient _client;

	public TestBase()
	{
		_client = new NftApiClient(Network.EthereumMainnet, "demo");
	}

    public void ValidateResponse<T>(ApiResult<T> result, bool expectSuccess = true) where T : class
    {
        result.Success.ShouldBe(expectSuccess);

        if (result.Success)
        {
            result.Result.ShouldNotBeNull();
        }
        else
        {
            result.ErrorMessage.ShouldNotBeNull();
        }
    }
}
