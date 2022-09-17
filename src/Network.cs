using System;
using System.ComponentModel;

namespace PinguApps.Alchemy.NftApiClient
{
    public enum Network
    {
        [Description("eth-mainnet")]
        EthereumMainnet,
        [Description("eth-rinkeby")]
        [Obsolete]
        EthereumRinkeby,
        [Description("eth-goerli")]
        EthereumGoerli,
        [Description("polygon-mainnet")]
        PolygonMainnet,
        [Description("polygon-mumbai")]
        PolygonMumbai
    }
}
