namespace Sentry.Internal
{
    internal record DiscardReason : Enumeration
    {
        public static DiscardReason BeforeSend = new("before_send");
        public static DiscardReason CacheOverflow = new("cache_overflow");
        public static DiscardReason EventProcessor = new("event_processor");
        public static DiscardReason NetworkError = new("network_error");
        public static DiscardReason QueueOverflow = new("queue_overflow");
        public static DiscardReason RateLimitBackoff = new("ratelimit_backoff");
        public static DiscardReason SampleRate = new("sample_rate");

        private DiscardReason(string value) : base(value)
        {
        }

        public DiscardReasonWithCategory WithCategory(DataCategory category) => new(this, category);
    }
}
