namespace PinguApps.Alchemy.NftApiClient
{
    /// <summary>
    /// Settings that define how to backoff whilst receiving transient errors or rate limits with requests
    /// </summary>
    public sealed class BackoffSettings
    {
        /// <summary>
        /// The initial delay to use when backing off due either to transient errors or rate limits
        /// </summary>
        public int BackoffInitialDelayMs { get; set; } = NftApiClient.DefaultBackoffInitialDelayMs;

        /// <summary>
        /// The multiplier to the delay to use when backing off due either to transient errors or rate limits
        /// </summary>
        public float BackoffMultiplier { get; set; } = NftApiClient.DefaultBackoffMultiplier;

        /// <summary>
        /// The maximum time before timing out a request
        /// </summary>
        public int BackoffTimeoutMs { get; set; } = NftApiClient.DefaultBackoffTimeoutMs;

        /// <summary>
        /// The maximum number of attempts to use when backing off due either to transient errors or rate limits
        /// </summary>
        public int BackoffMaxAttempts { get; set; } = NftApiClient.DefaultBackoffMaxAttempts;
    }
}
