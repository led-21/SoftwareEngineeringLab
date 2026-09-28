using SoftwareEngineeringLab.Core.SystemDesign.UrlShortener;
using Xunit;

namespace SoftwareEngineeringLab.Tests.SystemDesign;

public class UrlShortenerTests
{
    [Fact]
    public void ShortenUrl_GeneratesConsistentCodeAndResolvesOriginal()
    {
        var sut = new UrlShortenerService();
        string longUrl = "https://github.com/microsoft/dotnet";

        string code1 = sut.ShortenUrl(longUrl);
        string code2 = sut.ShortenUrl(longUrl);

        // Same URL should yield the same code
        Assert.Equal(code1, code2);
        Assert.Equal(7, code1.Length);

        // Resolving code should return original URL
        string? original = sut.GetOriginalUrl(code1);
        Assert.Equal(longUrl, original);
    }

    [Fact]
    public void GetOriginalUrl_WhenCodeNotFound_ReturnsNull()
    {
        var sut = new UrlShortenerService();
        Assert.Null(sut.GetOriginalUrl("nonexistent"));
    }
}
