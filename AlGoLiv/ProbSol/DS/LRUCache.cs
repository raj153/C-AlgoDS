using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.DS
{
    public class LRUCache<k, v>
    {
        protected int maxCacheSize;
        private Dictionary<k, LinkedListNode> map = null;

        private LinkedListNode head, tail = null;
        public LRUCache(int maxSize)
        {
            map = new Dictionary<k, LinkedListNode>();
            maxCacheSize = maxSize;
        }

        private void RemoveKey(k key)
        {
            if (!map.ContainsKey(key)) return;
            var node = map[key];

            removeFromLinkedList(node);
            
            map.Remove(key);

        }

        private void removeFromLinkedList(LinkedListNode node)
        {
            if (node.prev != null)
                node.prev.nxt = node.nxt;
            if (node.nxt != null)
                node.nxt.prev = node.prev;
            if (node == head) head = node.nxt;
            if (node == tail) tail = node.prev;
        }

        public void Insert(k key, v value)
        {
            RemoveKey(key);

            if (map.Count() >= maxCacheSize && tail != null)
                RemoveKey(tail.Key);

            LinkedListNode node = new LinkedListNode(key, value);
            InsertAtFront(node);
            map[key] = node;
            
        }
        public v GetValue(k key) {
            if (map.ContainsKey(key))
            {
                var node = map[key];
                if (node != head)
                {
                    removeFromLinkedList(node);
                    InsertAtFront(node);
                }
                return node.Value;
            }
            return default;
        }
        private void InsertAtFront(LinkedListNode node)
        {
            if (head == null) { head = node; tail = node; return; }
            head.prev = node;
            node.nxt = head;
            head = node;
        }
            
        private class LinkedListNode
        { 
        
            public LinkedListNode nxt, prev;

            public k  Key { get;  }
            public v Value { get;  }
            public LinkedListNode(k key, v value )
            {
                Key = key;
                Value = value;
            }
        }
    }
}
