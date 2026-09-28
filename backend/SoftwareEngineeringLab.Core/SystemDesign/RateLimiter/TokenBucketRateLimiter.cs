namespace SoftwareEngineeringLab.Core.SystemDesign.RateLimiter;

/// <summary>
/// Token Bucket Rate Limiter with thread-safe precision time tracking.
/// Avoids starvation on rapid sub-second requests by tracking fractional tokens or elapsed fractional seconds.
/// </summary>
public class TokenBucketRateLimiter
{
    private readonly int _capacity;
    private readonly double _refillRatePerSecond;
    private double _tokens;
    private DateTime _lastRefillUtc;
    private readonly object _syncRoot = new();

    public int Capacity => _capacity;
    public double RefillRatePerSecond => _refillRatePerSecond;

    public TokenBucketRateLimiter(int capacity, double refillRatePerSecond)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than zero.");
        if (refillRatePerSecond <= 0)
            throw new ArgumentOutOfRangeException(nameof(refillRatePerSecond), "Refill rate must be greater than zero.");

        _capacity = capacity;
        _refillRatePerSecond = refillRatePerSecond;
        _tokens = capacity;
        _lastRefillUtc = DateTime.UtcNow;
    }

    public double AvailableTokens
    {
        get
        {
            lock (_syncRoot)
            {
                Refill();
                return _tokens;
            }
        }
    }

    /// <summary>
    /// Attempts to consume requested tokens.
    /// Returns true if allowed (tokens available), false if throttled (rate limit exceeded).
    /// </summary>
    public bool TryConsume(int tokensRequested = 1)
    {
        if (tokensRequested <= 0)
            return true;

        lock (_syncRoot)
        {
            Refill();

            if (_tokens >= tokensRequested)
            {
                _tokens -= tokensRequested;
                return true;
            }

            return false;
        }
    }

    private void Refill()
    {
        var now = DateTime.UtcNow;
        var elapsedSeconds = (now - _lastRefillUtc).TotalSeconds;

        if (elapsedSeconds > 0)
        {
            var tokensToAdd = elapsedSeconds * _refillRatePerSecond;
            _tokens = Math.Min(_capacity, _tokens + tokensToAdd);
            _lastRefillUtc = now;
        }
    }
}
