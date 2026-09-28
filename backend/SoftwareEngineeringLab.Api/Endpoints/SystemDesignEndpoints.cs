using SoftwareEngineeringLab.Core.SystemDesign.ConsistentHashing;
using SoftwareEngineeringLab.Core.SystemDesign.LoadBalancer;
using SoftwareEngineeringLab.Core.SystemDesign.LruCache;
using SoftwareEngineeringLab.Core.SystemDesign.RateLimiter;
using SoftwareEngineeringLab.Core.SystemDesign.UrlShortener;

namespace SoftwareEngineeringLab.Api.Endpoints;

public record ShortenUrlRequest(string Url);
public record RateLimitConsumeRequest(int Tokens = 1);
public record LruPutRequest(string Key, string Value);
public record ConsistentHashKeyRequest(string Key);

public static class SystemDesignEndpoints
{
    public static void MapSystemDesignEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/system-design").WithTags("SystemDesign");

        // Singletons for persistent in-memory playground demos
        var rateLimiter = new TokenBucketRateLimiter(capacity: 10, refillRatePerSecond: 2);
        var urlShortener = new UrlShortenerService();
        var lruCache = new LruCache<string, string>(capacity: 4);
        var loadBalancer = new RoundRobinLoadBalancer(new[] { "server-us-east-1", "server-us-west-2", "server-eu-central-1" });
        var consistentRing = new ConsistentHashRing(virtualNodesPerServer: 30);
        consistentRing.AddNode("node-cache-01");
        consistentRing.AddNode("node-cache-02");
        consistentRing.AddNode("node-cache-03");

        // Rate Limiter
        group.MapGet("/rate-limiter/status", () => Results.Ok(new
        {
            capacity = rateLimiter.Capacity,
            refillRatePerSecond = rateLimiter.RefillRatePerSecond,
            availableTokens = Math.Round(rateLimiter.AvailableTokens, 2)
        }));

        group.MapPost("/rate-limiter/consume", (RateLimitConsumeRequest req) =>
        {
            bool allowed = rateLimiter.TryConsume(req.Tokens);
            return Results.Ok(new
            {
                allowed,
                tokensRequested = req.Tokens,
                remainingTokens = Math.Round(rateLimiter.AvailableTokens, 2),
                statusCode = allowed ? 200 : 429
            });
        });

        // URL Shortener
        group.MapPost("/url-shortener/shorten", (ShortenUrlRequest req) =>
        {
            if (string.IsNullOrWhiteSpace(req.Url))
                return Results.BadRequest(new { error = "Url cannot be empty." });

            string code = urlShortener.ShortenUrl(req.Url);
            return Results.Ok(new
            {
                originalUrl = req.Url,
                shortCode = code,
                shortUrl = $"https://sel.dev/{code}"
            });
        });

        group.MapGet("/url-shortener/{code}", (string code) =>
        {
            string? original = urlShortener.GetOriginalUrl(code);
            return original != null
                ? Results.Ok(new { shortCode = code, originalUrl = original })
                : Results.NotFound(new { error = $"Short code '{code}' not found." });
        });

        // LRU Cache
        group.MapGet("/lru-cache", () => Results.Ok(new
        {
            capacity = lruCache.Capacity,
            count = lruCache.Count,
            entries = lruCache.GetOrderedEntries().Select(e => new { key = e.Key, value = e.Value })
        }));

        group.MapPost("/lru-cache", (LruPutRequest req) =>
        {
            if (string.IsNullOrWhiteSpace(req.Key))
                return Results.BadRequest(new { error = "Key cannot be empty." });

            lruCache.Put(req.Key, req.Value);
            return Results.Ok(new
            {
                message = $"Key '{req.Key}' stored/updated.",
                entries = lruCache.GetOrderedEntries().Select(e => new { key = e.Key, value = e.Value })
            });
        });

        group.MapGet("/lru-cache/{key}", (string key) =>
        {
            if (lruCache.TryGet(key, out string? value))
            {
                return Results.Ok(new
                {
                    key,
                    value,
                    entries = lruCache.GetOrderedEntries().Select(e => new { key = e.Key, value = e.Value })
                });
            }
            return Results.NotFound(new { error = $"Key '{key}' not in cache." });
        });

        // Load Balancer
        group.MapGet("/load-balancer/next", () =>
        {
            string? server = loadBalancer.GetNextServer();
            return Results.Ok(new
            {
                server,
                activeServers = loadBalancer.Servers
            });
        });

        // Consistent Hashing
        group.MapPost("/consistent-hashing/route", (ConsistentHashKeyRequest req) =>
        {
            if (string.IsNullOrWhiteSpace(req.Key))
                return Results.BadRequest(new { error = "Key cannot be empty." });

            string? assignedNode = consistentRing.GetNode(req.Key);
            return Results.Ok(new
            {
                key = req.Key,
                assignedNode,
                totalRingSlots = consistentRing.TotalRingSlots,
                virtualNodesPerServer = consistentRing.VirtualNodesPerServer
            });
        });
    }
}
