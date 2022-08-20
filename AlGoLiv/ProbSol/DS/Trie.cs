using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.DS
{
    public class TrieArray
    {
        Node _root;

        public TrieArray()
        {
            _root = new Node(' ');
        }

        public void InsertWord(string word)
        {
            var cur = _root;
            foreach(char c in word)
            {
                var key = c - 'a';
                if (cur.Children[key] == null)
                    cur.Children[key] = new Node(c);

                cur = cur.Children[key];
            }
            cur.IsWord = true;

        }
        public bool StartsWith(string prefix)
        {
            return GetNode(prefix) != null;
        }
        public bool Search(string word)
        {
            Node node = GetNode(word);
            return node != null && node.IsWord; //return if word in trie
        }
        private Node GetNode(string word)
        {
            var cur = _root;
            foreach(char c in word)
            {
                var key = c - 'a';
                if (cur.Children[key] == null) return null;

                cur = cur.Children[key];
            }
            return cur;
        }

        public class Node
        {
            char _c;
            Node[] _children;
            public bool IsWord;

            public Node[] Children
            {
                get
                {
                    return _children;
                }
            }
            public Node(char c)
            {
                _c = c;
                _children = new Node[26];

            }


        }

    }
    public class Trie
    {
        private readonly Dictionary<char, Trie> _node;
        public bool IsValidWord = false;
        //private readonly i
        public Dictionary<char, Trie> Node
        {
            get
            {
                return _node;
            }
        }
        public Trie()
        {
            _node = new Dictionary<char, Trie>();
        }

        public void BuildTrie(string[] words)
        {
            foreach(string str in words)
            {
                GenerateTrie(str);
            }
        }
        private void GenerateTrie(string word)
        {
            var curNode = _node;
            int counter = 0;
            foreach (char c in word)
            {
                counter++;
                if (curNode.ContainsKey(c))
                {
                    curNode = curNode[c]._node;
                    continue;
                }
                else
                {
                    curNode[c] = new Trie();
                    if(counter == word.Length) {
                        curNode[c].IsValidWord = true;
                    }
                    curNode = curNode[c]._node;
                }
                
            }
            
            
        }
        public bool IsPrefixAvailable(string str)
        {
            var curNode = _node;
            int counter = 0;   
            foreach(char c in str)
            {
                if (curNode.ContainsKey(str[c]))
                {
                    curNode = curNode[str[c]]._node;                   

                }
                else
                {
                    return false;
                }
            }
            return true;
        }


    }
}
