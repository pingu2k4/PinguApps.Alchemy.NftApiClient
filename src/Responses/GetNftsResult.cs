using PinguApps.Alchemy.NftApiClient.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PinguApps.Alchemy.NftApiClient.Responses
{
    /// <summary>
    /// Response when calling GetNfts
    /// </summary>
    /// <param name="Nfts">List of Nft's belonging to the account specified</param>
    /// <param name="PageKey">UUID for pagination - returned if there are more NFTs to fetch. Max NFTs per page = 100.</param>
    /// <param name="TotalCount">Total number of NFTs owned by the given address.</param>
    /// <param name="BlockHash">The canonical head block hash of when your request was received.</param>
    public record GetNftsResult(
        [property: JsonPropertyName("ownedNfts")] IReadOnlyList<Nft> Nfts,
        [property: JsonPropertyName("pageKey")] string? PageKey,
        [property: JsonPropertyName("totalCount")] int TotalCount,
        [property: JsonPropertyName("blockHash")] string BlockHash);

    /// <summary>
    /// An Nft
    /// </summary>
    /// <param name="Contract">Contract for the returned Nft</param>
    /// <param name="Token">Information about the token</param>
    /// <param name="Balance">Token balance</param>
    /// <param name="Title">Name of the Nft asset</param>
    /// <param name="Description">Brief human-readable description</param>
    /// <param name="TokenUri">Uri's specifying metadata for the Nft</param>
    /// <param name="Media">Metadata for media representing the Nft</param>
    /// <param name="Metadata">Relevant metadata for this Nft. This is useful for viewing image url, traits, etc. without having to follow the metadata url in tokenUri to parse manually</param>
    /// <param name="TimeLastUpdated">ISO timestamp of the last cache refresh for the information returned in the metadata field</param>
    /// <param name="ContractMetadata">Relevant metadata for this Nft contract</param>
    /// <param name="Error">A string describing a particular reason that we were unable to fetch complete metadata for the NFT</param>
    public record Nft(
        [property: JsonPropertyName("contract")] ContractDetails Contract,
        [property: JsonPropertyName("id")] Token Token,
        [property: JsonPropertyName("balance")] string Balance,
        [property: JsonPropertyName("title")] string? Title,
        [property: JsonPropertyName("description")] string? Description,
        [property: JsonPropertyName("tokenUri")] TokenUri? TokenUri,
        [property: JsonPropertyName("media")] IReadOnlyList<Media>? Media,
        [property: JsonPropertyName("metadata")] Metadata? Metadata,
        [property: JsonPropertyName("timeLastUpdated")] DateTime TimeLastUpdated,
        [property: JsonPropertyName("contractMetadata")] ContractMetadata ContractMetadata,
        [property: JsonPropertyName("error")] string? Error);

    /// <summary>
    /// Details about the contract
    /// </summary>
    /// <param name="Address">Address of the Nft contract</param>
    public record ContractDetails(
        [property: JsonPropertyName("address")] string Address);

    /// <summary>
    /// Information about the token
    /// </summary>
    /// <param name="TokenId">The ID of the token. Can be in hex or decimal format</param>
    /// <param name="TokenMetadata">Metadata desribing the token</param>
    public record Token(
        [property: JsonPropertyName("tokenId")] string TokenId,
        [property: JsonPropertyName("tokenMetadata")] TokenMetadata? TokenMetadata);

    /// <summary>
    /// Information about the token
    /// </summary>
    /// <param name="TokenType"><c>ERC721</c> or <c>ERC1155</c></param>
    public record TokenMetadata(
        [property: JsonPropertyName("tokenType")] TokenType TokenType);

    /// <summary>
    /// Uri's specifying metadata for the Nft
    /// </summary>
    /// <param name="Raw">Uri representing the location of the NFT's original metadata blob. This is a backup for you to parse when the metadata field is not automatically populated</param>
    /// <param name="Gateway">Public gateway uri for the raw uri above</param>
    public record TokenUri(
        [property: JsonPropertyName("raw")] string Raw,
        [property: JsonPropertyName("gateway")] string Gateway);

    /// <summary>
    /// Metadata for media representing the Nft
    /// </summary>
    /// <param name="Raw">Uri representing the location of the NFT's original metadata blob. This is a backup for you to parse when the metadata field is not automatically populated</param>
    /// <param name="Gateway">Public gateway uri for the raw uri above</param>
    /// <param name="Thumbnail">URL for a resized thumbnail of the NFT media asset</param>
    /// <param name="Format">The media format (jpg, gif, png, etc.) of the gateway and thumbnail assets</param>
    /// <param name="Size">The size of the media asset in bytes</param>
    public record Media(
        [property: JsonPropertyName("raw")] string Raw,
        [property: JsonPropertyName("gateway")] string Gateway,
        [property: JsonPropertyName("thumbnail")] string? Thumbnail,
        [property: JsonPropertyName("format")] string? Format,
        [property: JsonPropertyName("size")] long? Size);

    /// <summary>
    /// Relevant metadata for this Nft. This is useful for viewing image url, traits, etc. without having to follow the metadata url in tokenUri to parse manually
    /// </summary>
    /// <param name="Name">Name of the Nft asset</param>
    /// <param name="Description">Human-readable description of the Nft asset. (Markdown is supported/rendered on OpenSea and other Nft platforms)</param>
    /// <param name="Image">URL to the Nft asset image. Can be standard URLs pointing to images on conventional servers, IPFS, or Arweave. Most types of images (SVGs, PNGs, JPEGs, etc.) are supported by Nft marketplaces.</param>
    /// <param name="BackgroundColor">Background color of the Nft item. Usually must be defined as a six-character hexadecimal.</param>
    /// <param name="ExternalUrl">The image URL that appears alongside the asset image on NFT platforms.</param>
    /// <param name="Attributes">Traits/attributes/characteristics for each Nft asset.</param>
    public record Metadata(
        [property: JsonPropertyName("name")] string? Name,
        [property: JsonPropertyName("description")] string? Description,
        [property: JsonPropertyName("image")] string? Image,
        [property: JsonPropertyName("background_color")] string? BackgroundColor,
        [property: JsonPropertyName("external_url")] string? ExternalUrl,
        [property: JsonPropertyName("attributes")] IReadOnlyList<Attribute>? Attributes);

    /// <summary>
    /// Traits/attributes/characteristics for the Nft
    /// </summary>
    [JsonConverter(typeof(AttributeConverter))]
    public record Attribute(
        [property: JsonPropertyName("trait_type")] string TraitType,
        [property: JsonPropertyName("display_type")] string DisplayType);

    /// <inheritdoc/>
    public record StringAttribute(
        [property: JsonPropertyName("value")] string? Value,
        string TraitType,
        string DisplayType) : Attribute(TraitType, DisplayType);

    /// <inheritdoc/>
    public record DecimalAttribute(
        [property: JsonPropertyName("value")] decimal? Value,
        string TraitType,
        string DisplayType) : Attribute(TraitType, DisplayType);

    /// <summary>
    /// Relevant metadata for this Nft contract
    /// </summary>
    /// <param name="Name">Nft contract name.</param>
    /// <param name="Symbol">Nft contract symbol abbreviation.</param>
    /// <param name="TotalSupply">Total number of Nft's in a given Nft collection.</param>
    /// <param name="TokenType"><c>ERC721</c> or <c>ERC1155</c></param>
    public record ContractMetadata(
        [property: JsonPropertyName("name")] string? Name,
        [property: JsonPropertyName("symbol")] string? Symbol,
        [property: JsonPropertyName("totalSupply")] string? TotalSupply,
        [property: JsonPropertyName("tokenType")] TokenType TokenType);
}
