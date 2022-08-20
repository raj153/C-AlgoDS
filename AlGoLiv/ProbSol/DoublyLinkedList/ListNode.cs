using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.Lists
{
    public class ListNode<k>
    {
        public k val;
        public ListNode<k> _next;
        public ListNode(k _val)
        {
            val = _val;
            _next = null;
        } 

        
    }
}
