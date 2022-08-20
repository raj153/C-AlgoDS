using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.Lists
{
    public class MergeKSortedLists
    {
        //https://www.youtube.com/watch?v=BBt9FB5Yt0M
        public ListNode<int> Merge(ListNode<int>[] listNodes)
        {
            
            if (listNodes == null || listNodes.Length == 0) return null;

            return DivideAndConquer(listNodes, 0, listNodes.Length - 1);

        }

        private ListNode<int> DivideAndConquer(ListNode<int>[] listNodes, int start, int end)
        {
            if (start == end) return listNodes[start];

           
            int mid = start + (end - start) / 2; //To avoid overflow when start and end are big enough for int

            ListNode<int> left = DivideAndConquer(listNodes, start, mid);
            ListNode<int> right = DivideAndConquer(listNodes, mid + 1, end);

            return MergeLists(left, right);
        }

        private ListNode<int> MergeLists(ListNode<int> l1, ListNode<int> l2)
        {
            ListNode<int> result = new ListNode<int>(-1);
            ListNode<int> cur = result;

            while(l1 !=null || l2 != null)
            {
                if (l1 == null)
                {
                    cur._next = l2;
                    l2 = l2._next;
                }
                else if (l2 == null)
                {
                    cur._next = l1;
                    l1 = l1._next;

                }else if (l1._next.val < l2._next.val)
                {
                    cur._next = l1._next;
                    l1 = l1._next;
                }else
                {
                    cur._next = l2._next;
                    l2 = l2._next;
                }
                cur = cur._next;

            }
            return result._next;
        }
    }
}
