using SoftwareEngineeringLab.Core.SystemDesign.RateLimiter;
using Xunit;

namespace SoftwareEngineeringLab.Tests.SystemDesign;

public class TokenBucketRateLimiterTests
{
    [Fact]
    public void TryConsume_AllowsUpToCapacity()
    {
        var limiter = new TokenBucketRateLimiter(capacity: 5, refillRatePerSecond: 1);

        for (int i = 0; i < 5; i++)
        {
            Assert.True(limiter.TryConsume(1));
        }

        // 6th request with empty bucket should be rejected immediately
        Assert.False(limiter.TryConsume(1));
    }

    [Fact]
    public async Task TryConsume_RefillsTokensOverTimeWithoutStarvation()
    {
        // 10 capacity, 10 tokens per second -> 1 token every 100ms
        var limiter = new TokenBucketRateLimiter(capacity: 10, refillRatePerSecond: 10);

        // Exhaust all tokens
        for (int i = 0; i < 10; i++)
        {
            Assert.True(limiter.TryConsume(1));
        }
        Assert.False(limiter.TryConsume(1));

        // Rapid checks within 150ms should not freeze accumulation (starvation test)
        for (int i = 0; i < 5; i++)
        {
            await Task.Delay(30);
            limiter.TryConsume(1); // Rapid polling
        }

        // Wait an additional 200ms to guarantee at least 1-2 tokens accumulated
        await Task.Delay(200);

        Assert.True(limiter.TryConsume(1));
    }
}
