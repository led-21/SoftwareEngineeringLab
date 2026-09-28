using SoftwareEngineeringLab.Core.Algorithms.Trees;
using Xunit;

namespace SoftwareEngineeringLab.Tests.Algorithms;

public class TreeProblemsTests
{
    private readonly TreeProblems _sut = new();

    [Fact]
    public void MaxDepth_ReturnsCorrectDepth()
    {
        //       3
        //      / \
        //     9   20
        //        /  \
        //       15   7
        var root = new TreeNode(3,
            new TreeNode(9),
            new TreeNode(20, new TreeNode(15), new TreeNode(7)));

        Assert.Equal(3, _sut.MaxDepth(root));
    }

    [Fact]
    public void IsValidBST_ValidatesOrderingInvariants()
    {
        // Valid BST: 2 is root, 1 is left, 3 is right
        var valid = new TreeNode(2, new TreeNode(1), new TreeNode(3));
        Assert.True(_sut.IsValidBST(valid));

        // Invalid BST: 5 is root, 1 is left, 4 is right (but right has 3 and 6)
        var invalid = new TreeNode(5,
            new TreeNode(1),
            new TreeNode(4, new TreeNode(3), new TreeNode(6)));
        Assert.False(_sut.IsValidBST(invalid));
    }

    [Fact]
    public void InvertTree_SwapsLeftAndRightSubtrees()
    {
        var root = new TreeNode(4,
            new TreeNode(2, new TreeNode(1), new TreeNode(3)),
            new TreeNode(7, new TreeNode(6), new TreeNode(9)));

        var inverted = _sut.InvertTree(root);

        Assert.Equal(7, inverted?.Left?.Val);
        Assert.Equal(2, inverted?.Right?.Val);
    }
}
