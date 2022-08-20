using AlGoLiv.ProbSol.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.Logic
{
    public class Boggle
    {

        char[,] boogle = { 
            {'c', 'y', 'z', 'e'}, 
            {'v', 'a', 'x', 'b'}, 
            {'x', 'b', 't', 'u'}, 
            {'i', 'a', 'n', 'k'} 
        };

        int[,] neighborDelta = {
            {-1,-1 },
            {-1,0 },
            {-1,1 },
            {0, -1},
            {0,1 },
            {1, -1 },
            {1,0 },
            {1,1 }
        };

        int rLen, cLen = 0;
        public Boggle()
        {
            rLen = boogle.GetLength(0);
            cLen = boogle.GetLength(1);
        }

        public void PlayBoggle(Trie trie)
        {
            for(int i=0; i<rLen; i++)
            {
                for(int j=0; j<cLen; j++)
                {
                    dfs(i, j, new Dictionary<string, bool>(),"", trie.Node);
                }
            }
            
        }
        void dfs(int r, int c, Dictionary<string,bool>  visited, string nowWord, Dictionary<char, Trie> node)
        {
            if (visited.ContainsKey(r + "-" + c)) return;

            char letter = boogle[r, c]; 

            visited[r + "-" + c] = true;    
            
            if (node.ContainsKey(letter))
            {
                nowWord += letter;
                if (node[letter].IsValidWord)
                    Console.WriteLine(nowWord);
                //var neighbors = GetNeighbors(r, c);

                for (int i = 0; i < neighborDelta.GetLength(0); i++)
                {
                    var newr = r + neighborDelta[i, 0];
                    var newc = c + neighborDelta[i, 1];
                    if(newr >= 0 && newr < rLen && newc >= 0 & newc < cLen)
                        dfs(newr, newc, new Dictionary<string, bool>(), nowWord, node[letter].Node);
                }

            }

        }
        int[,] GetNeighbors(int cr, int cc)
        {
            //NOT WORKING
            int[,] neigh = { };
            for(int i=0; i< neighborDelta.GetLength(0); i++)
            {
                int r = cr + neighborDelta[i, 0];
                int c = cr + neighborDelta[i, 1];

                if (r >= 0 && r <= rLen && c >= 0 && c <= cLen)
                {
                    neigh[neigh.GetLength(0), 0] = r;
                    neigh[neigh.GetLength(0), 1] = c;
                }
            }
            return neigh;
        }

    }
}
