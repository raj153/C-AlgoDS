using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.DoublyLinkedList
{

    
    public class ListBasic 
    {
        //https://youtu.be/OFr16YdsBEQ?t=1240
        //Floyd's Cycle finding
        public static DoublyLinkedListNode<T> FindCycleNode<T>(DoublyLinkedListNode<T> head)
        {
            if (head == null || head.Next == null) return null;

            DoublyLinkedListNode<T> walker = head;
            DoublyLinkedListNode<T> runner = head;
            bool isCycle = false;

            //First part of Floyd's: Is there a cycle exists?
            while (walker != null && runner != null)
            {
                walker = walker.Next;

                if (runner.Next == null) return null;

                runner = runner.Next.Next;

                if (walker == runner)
                {
                    isCycle = true;
                    break;
                }



            }
            if (!isCycle) return null;

            walker = head;

            while (walker != runner)
            {
                walker = walker.Next;
                runner = runner.Next;

            }
            return walker;
        }




        public class IntComparer : IComparer<int>
        {
            public int Compare(int x, int y)
            {
                return x.CompareTo(y);
            }
        }
        //https://youtu.be/OFr16YdsBEQ?t=606
        public static DoublyLinkedListNode<T> PartitionList<T>(DoublyLinkedListNode<T> head, T x, IComparer<T> comparer)
        {
            DoublyLinkedListNode<T> beforeHead = new DoublyLinkedListNode<T>(default(T));
            DoublyLinkedListNode<T> beforeRunner = beforeHead;

            DoublyLinkedListNode <T> afterHead = new DoublyLinkedListNode<T>(default(T));
            DoublyLinkedListNode<T> afterRunner = afterHead;

            while(head != null){
                if(comparer.Compare(head.Element, x) <0)
                {
                    beforeRunner.Next = head;
                    beforeRunner = beforeRunner.Next;

                }
                else
                {
                    afterRunner.Next = head;
                    afterRunner = afterRunner.Next;
                }
                head = head.Next;
            }
            afterRunner.Next = null;
            beforeRunner.Next = afterHead.Next;
            return beforeHead.Next;

        }
        //https://youtu.be/OFr16YdsBEQ?t=606
        public static DoublyLinkedListNode<T> SwapNodesInPair<T>(DoublyLinkedListNode<T> head)
        {
            DoublyLinkedListNode<T> dummy = new DoublyLinkedListNode<T>(default(T));

            dummy.Next = head;

            DoublyLinkedListNode<T> runner = dummy;
        
            while(runner.Next != null && runner.Next.Next != null)
            {
                var r1 = runner.Next;
                var r2 = runner.Next.Next;

                runner.Next = r2;
                runner.Next.Next = r1;
                r2.Next = r1;

                runner = runner.Next.Next;

            }
            return dummy.Next;

        }
        //https://youtu.be/OFr16YdsBEQ?t=386
        public static DoublyLinkedListNode<T> RemoveNthNodeFromEnd<T>(DoublyLinkedListNode<T> head, int n)
        {
            DoublyLinkedListNode<T> dummy = new DoublyLinkedListNode<T>(default(T));

            dummy.Next = head;

            DoublyLinkedListNode<T> runner = dummy;
            DoublyLinkedListNode<T> walker = dummy;

            //Advances runner so that distance beween walker and runner is n+1 to get walker right before N element
            for (int i = 0; i < n+1; i++)
                runner = runner.Next;

            while(runner != null)
            {
                runner = runner.Next;
                walker = walker.Next;
            }
            walker.Next = walker.Next.Next;
            
            return dummy.Next;

        }
    }
}
