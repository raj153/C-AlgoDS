using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.DoublyLinkedList
{
    public class DoublyLinkedList<E>
    {

        DoublyLinkedListNode<E> dummyHead;
        DoublyLinkedListNode<E> dummyTail;

        public DoublyLinkedList()
        {
            // We can instantiate these by null, since we are never gonna use val for these dummyNodes
            dummyHead = new DoublyLinkedListNode<E>(default(E));
            dummyTail = new DoublyLinkedListNode<E>(default(E));

            // Also Initially there are no items
            // so just join dummyHead and Tail, we can add items in between them easily.
            dummyHead.Next = dummyTail;
            dummyTail.Prev = dummyHead;
        }

        /**
         * Method to detach a random node from the doubly linked list. The node itself will not be removed from the memory.
         * Just that it will be removed from the list and becomes orphaned.
         *
         * @param node Node to be detached.
         */
        public void detachNode(DoublyLinkedListNode<E> node)
        {
            // Just Simply modifying the pointers.
            if (node != null)
            {
                node.Prev.Next = node.Next;
                node.Next.Prev = node.Prev;
            }
        }

        /**
         * Helper method to add a node at the end of the list.
         *
         * @param node Node to be added.
         */
        public void addNodeAtLast(DoublyLinkedListNode<E> node)
        {
            DoublyLinkedListNode<E> tailPrev = dummyTail.Prev;
            tailPrev.Next = node;
            node.Next = dummyTail;
            dummyTail.Prev = node;
            node.Prev = tailPrev;
        }

        /**
         * Helper method to add an element at the end.
         *
         * @param element Element to be added.
         * @return Reference to new node created for the element.
         */
        public DoublyLinkedListNode<E> addElementAtLast(E element)
        {
            if (element == null)
            {
                throw new InvalidElementException();
            }
            DoublyLinkedListNode<E> newNode = new DoublyLinkedListNode<E>(element);
            addNodeAtLast(newNode);
            return newNode;
        }

        public bool isItemPresent()
        {
            return dummyHead.Next != dummyTail;
        }

        public DoublyLinkedListNode<E> getFirstNode()
        {
            DoublyLinkedListNode<E> item = null;
            if (!isItemPresent())
            {
                return null;
            }
            return dummyHead.Next;
        }

        [Serializable]
        internal class InvalidElementException : Exception
        {
            public InvalidElementException()
            {
            }

            public InvalidElementException(string message) : base(message)
            {
            }

            public InvalidElementException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected InvalidElementException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        public DoublyLinkedListNode<E> getLastNode()
        {
            DoublyLinkedListNode<E> item = null;
            if (!isItemPresent())
            {
                return null;
            }
            return dummyTail.Prev;
        }
    }
}
