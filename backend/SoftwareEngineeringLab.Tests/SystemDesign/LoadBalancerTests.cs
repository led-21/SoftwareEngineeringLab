using SoftwareEngineeringLab.Core.SystemDesign.LoadBalancer;
using Xunit;

namespace SoftwareEngineeringLab.Tests.SystemDesign;

public class LoadBalancerTests
{
    [Fact]
    public void GetNextServer_DistributesRoundRobin()
    {
        var lb = new RoundRobinLoadBalancer(new[] { "server-1", "server-2", "server-3" });

        Assert.Equal("server-1", lb.GetNextServer());
        Assert.Equal("server-2", lb.GetNextServer());
        Assert.Equal("server-3", lb.GetNextServer());
        Assert.Equal("server-1", lb.GetNextServer()); // Wraps around
    }

    [Fact]
    public void AddAndRemoveServer_UpdatesPoolGracefully()
    {
        var lb = new RoundRobinLoadBalancer(new[] { "s1", "s2" });
        lb.AddServer("s3");

        Assert.Equal(3, lb.Servers.Count);

        lb.RemoveServer("s2");
        Assert.Equal(2, lb.Servers.Count);
        Assert.DoesNotContain("s2", lb.Servers);
    }
}
