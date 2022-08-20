using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.Tree
{
    /*
     * https://leetcode.com/problems/all-nodes-distance-k-in-binary-tree/solution/
     * Given the root of a binary tree, the value of a target node target, and an integer k, 
     * return an array of the values of all nodes that have a distance k from the target node.
     */
    public class DistanceFromTarget
    {
        Dictionary<TreeNode, TreeNode> parent;
        
        public List<int> DistanceKFromTargetT(TreeNode root, TreeNode target, int k)
        {
            parent = new Dictionary<TreeNode, TreeNode>();

            dfs(root, null);

            Queue<TreeNode> queue = new Queue<TreeNode>();
            queue.Enqueue(target);

            HashSet<TreeNode> visited = new HashSet<TreeNode>();
            
            
            int dist = 0;
            while (queue.Count() > 0)
            {
                int size = queue.Count();
                if(dist == k)
                {
                    List<int> ans = new List<int>();
                    foreach(TreeNode node1 in queue)
                    {
                        ans.Add(node1.Value);
                    }
                    return ans;
                }
                for(int i=0; i<size; i++) {
                    TreeNode node = queue.Dequeue();
                    visited.Add(node);
                    if (node.Left != null &&!visited.Contains(node.Left))
                    {
                        visited.Add(node.Left);
                        queue.Enqueue(node.Left);
                    }

                    if (node.Right !=null && !visited.Contains(node.Right)) {
                        visited.Add(node.Right);
                        queue.Enqueue(node.Right);
                    }
                    TreeNode par = parent[node];
                    if (par != null && !visited.Contains(par))
                    {
                        visited.Add(par);
                        queue.Enqueue(par);

                    }
                    
                }
                dist++;
            }
             
            return new List<int>();
        }
        
        private void dfs(TreeNode node, TreeNode par)
        {
            if(node !=null)
            {
                parent[node] = par;

                dfs(node.Left, node);
                dfs(node.Right, node);
            }

        }
    }
}
