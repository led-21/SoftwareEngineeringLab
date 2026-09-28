using System.Security.Cryptography;
using System.Text;

namespace SoftwareEngineeringLab.Core.SystemDesign.UrlShortener;

/// <summary>
/// In-memory URL Shortener demonstration with Base62 encoding and collision handling.
/// </summary>
public class UrlShortenerService
{
    private const string Base62Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private readonly Dictionary<string, string> _urlToCode = new();
    private readonly Dictionary<string, string> _codeToUrl = new();
    private readonly object _syncRoot = new();

    public string ShortenUrl(string originalUrl)
    {
        if (string.IsNullOrWhiteSpace(originalUrl))
            throw new ArgumentException("URL cannot be empty.", nameof(originalUrl));

        lock (_syncRoot)
        {
            if (_urlToCode.TryGetValue(originalUrl, out var existingCode))
                return existingCode;

            string shortCode = GenerateUniqueCode(originalUrl);
            _urlToCode[originalUrl] = shortCode;
            _codeToUrl[shortCode] = originalUrl;

            return shortCode;
        }
    }

    public string? GetOriginalUrl(string shortCode)
    {
        if (string.IsNullOrWhiteSpace(shortCode))
            return null;

        lock (_syncRoot)
        {
            return _codeToUrl.GetValueOrDefault(shortCode);
        }
    }

    private string GenerateUniqueCode(string originalUrl)
    {
        // Use SHA256 of url + salt to derive 7-character base62 code with collision fallback
        int attempt = 0;
        while (true)
        {
            string payload = attempt == 0 ? originalUrl : $"{originalUrl}_{attempt}";
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
            ulong number = BitConverter.ToUInt64(hash, 0);

            var sb = new StringBuilder(7);
            for (int i = 0; i < 7; i++)
            {
                sb.Append(Base62Alphabet[(int)(number % 62)]);
                number /= 62;
            }

            string candidate = sb.ToString();
            if (!_codeToUrl.ContainsKey(candidate))
                return candidate;

            attempt++;
        }
    }
}
