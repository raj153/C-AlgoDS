using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.Tree
{
    public class TreeBacktracking
    {

        //https://youtu.be/gUgT0W25jCM?t=1035
        //Binary Tree Max  Path Sum
        public static int BinaryTreeMaxPathSum(TreeNode root)
        {
            if (root == null) return 0;
            int max = int.MinValue; ;

            BinaryTreeMaxPathSumDfs(root, ref max);
            return max;
        }

        private static int BinaryTreeMaxPathSumDfs(TreeNode root, ref int max)
        {

            if (root == null) return 0;
            int left = Math.Max(0, BinaryTreeMaxPathSumDfs(root.Left, ref max));
            int right = Math.Max(0, BinaryTreeMaxPathSumDfs(root.Right, ref max));

            max = Math.Max(max, left+ right + root.Value);

            return Math.Max(left,right)+root.Value;
        }

        //https://youtu.be/gUgT0W25jCM?t=561
        //Binary Tree Longest Consecutive Sequence
        //int result =0;
        public static int LongestConsecutiveSequence(TreeNode root)
        {
            if (root == null) return 0;
            int result =0;

            LCSDfs(root, ref result);
            return result;
        }

        private static int LCSDfs(TreeNode node, ref int result)
        {
            if (node == null) return 0;

            int left = LCSDfs(node.Left, ref result);
            int right = LCSDfs(node.Right, ref result);

            int max = 1;

            if (node.Left == null || node.Left.Value == node.Value + 1)
            {
                max = Math.Max(left + 1, max);
            }
            if (node.Right == null || node.Right.Value == node.Value + 1)
                max = Math.Max(right + 1, max);

            result = Math.Max(result, max);

            return max;

        }
        //https://youtu.be/gUgT0W25jCM?t=250
        //Path to Leaf equal to sum / all root node to leaf path / sum root to leaf numbers
        public static List<List<int>> LeafPathSum(TreeNode treeNode, int sum)
        {
            if (treeNode == null) return null;

            List<List<int>> result = new List<List<int>>();
            LeafPathSumDfs(treeNode, sum, new List<int>() , result);

            return result;

        }

        private static void LeafPathSumDfs(TreeNode treeNode, int sum, List<int> cur, List<List<int>> result)
        {
            if (treeNode == null) return;

            cur.Add(treeNode.Value);
            
            //Only at leaf node level
            if(treeNode.Left == null && treeNode.Right== null && treeNode.Value == sum) { result.Add(cur); }

            LeafPathSumDfs(treeNode.Left, sum - treeNode.Value, cur, result);
            LeafPathSumDfs(treeNode.Right, sum - treeNode.Value, cur, result);
            cur.RemoveAt(cur.Count - 1);

        }
    }
}
