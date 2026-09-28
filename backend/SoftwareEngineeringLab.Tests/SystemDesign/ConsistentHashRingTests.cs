using SoftwareEngineeringLab.Core.SystemDesign.ConsistentHashing;
using Xunit;

namespace SoftwareEngineeringLab.Tests.SystemDesign;

public class ConsistentHashRingTests
{
    [Fact]
    public void GetNode_ReturnsDeterministicNodeForKey()
    {
        var ring = new ConsistentHashRing(virtualNodesPerServer: 30);
        ring.AddNode("node-alpha");
        ring.AddNode("node-beta");
        ring.AddNode("node-gamma");

        string? assigned1 = ring.GetNode("session:user:12345");
        string? assigned2 = ring.GetNode("session:user:12345");

        Assert.NotNull(assigned1);
        Assert.Equal(assigned1, assigned2);
    }

    [Fact]
    public void RemoveNode_ReassignsKeysClockwise()
    {
        var ring = new ConsistentHashRing(virtualNodesPerServer: 20);
        ring.AddNode("node-a");
        ring.AddNode("node-b");

        string? initial = ring.GetNode("test-key");
        Assert.NotNull(initial);

        // Remove node-a
        ring.RemoveNode("node-a");

        // Now all keys must map to remaining node-b
        Assert.Equal("node-b", ring.GetNode("test-key"));
    }
}
