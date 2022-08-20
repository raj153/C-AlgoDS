using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.Tree
{
    public class TreeNode
    {
        private int _value;
        public int Value { get { return _value; } }
        public TreeNode Left { get; set; }

        public TreeNode Right { get; set; }

        public TreeNode(int value)
        {
            _value = value;
        }
    }
}
