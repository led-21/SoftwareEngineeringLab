namespace SoftwareEngineeringLab.Core.Algorithms.Trees;

/// <summary>
/// Classic binary tree interview algorithms.
/// </summary>
public class TreeProblems
{
    /// <summary>
    /// Maximum Depth of Binary Tree.
    /// Time Complexity: O(n)
    /// Space Complexity: O(h) where h is the tree height
    /// </summary>
    public int MaxDepth(TreeNode? root)
    {
        if (root == null)
            return 0;

        return 1 + Math.Max(MaxDepth(root.Left), MaxDepth(root.Right));
    }

    /// <summary>
    /// Same Tree: Check if two binary trees are structurally identical and have the same values.
    /// Time Complexity: O(n)
    /// Space Complexity: O(h)
    /// </summary>
    public bool IsSameTree(TreeNode? p, TreeNode? q)
    {
        if (p == null && q == null)
            return true;
        if (p == null || q == null)
            return false;

        return p.Val == q.Val && IsSameTree(p.Left, q.Left) && IsSameTree(p.Right, q.Right);
    }

    /// <summary>
    /// Invert Binary Tree: Swap left and right children recursively.
    /// Time Complexity: O(n)
    /// Space Complexity: O(h)
    /// </summary>
    public TreeNode? InvertTree(TreeNode? root)
    {
        if (root == null)
            return null;

        var temp = root.Left;
        root.Left = InvertTree(root.Right);
        root.Right = InvertTree(temp);

        return root;
    }

    /// <summary>
    /// Inorder Traversal (Left, Root, Right).
    /// Time Complexity: O(n)
    /// Space Complexity: O(h)
    /// </summary>
    public IList<int> InorderTraversal(TreeNode? root)
    {
        var result = new List<int>();
        InorderHelper(root, result);
        return result;
    }

    private void InorderHelper(TreeNode? node, List<int> result)
    {
        if (node == null)
            return;

        InorderHelper(node.Left, result);
        result.Add(node.Val);
        InorderHelper(node.Right, result);
    }

    /// <summary>
    /// Validate Binary Search Tree: Ensures left subtree &lt; node &lt; right subtree.
    /// Time Complexity: O(n)
    /// Space Complexity: O(h)
    /// </summary>
    public bool IsValidBST(TreeNode? root)
    {
        return IsValidBSTHelper(root, long.MinValue, long.MaxValue);
    }

    private bool IsValidBSTHelper(TreeNode? node, long minVal, long maxVal)
    {
        if (node == null)
            return true;

        if (node.Val <= minVal || node.Val >= maxVal)
            return false;

        return IsValidBSTHelper(node.Left, minVal, node.Val) &&
               IsValidBSTHelper(node.Right, node.Val, maxVal);
    }

    /// <summary>
    /// Lowest Common Ancestor in a Binary Tree.
    /// Time Complexity: O(n)
    /// Space Complexity: O(h)
    /// </summary>
    public TreeNode? LowestCommonAncestor(TreeNode? root, TreeNode? p, TreeNode? q)
    {
        if (root == null || root == p || root == q)
            return root;

        var left = LowestCommonAncestor(root.Left, p, q);
        var right = LowestCommonAncestor(root.Right, p, q);

        if (left != null && right != null)
            return root;

        return left ?? right;
    }
}
