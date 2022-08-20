using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.DoublyLinkedList
{
    public class DoublyLinkedListNode<E>
    {
        public DoublyLinkedListNode<E> Next;
        public DoublyLinkedListNode<E> Prev;
        public E Element;

        public DoublyLinkedListNode(E element)
        {
            this.Element = element;
            this.Next = null;
            this.Prev = null;
        }
    }
}
