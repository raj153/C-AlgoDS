using AlGoLiv.ProbSol.DoublyLinkedList;
using AlGoLiv.ProbSol.DS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.Graph
{

    public static class Extensions {

        public static T[] Populate<T>(this T[] nums, T value)
        {
            for (int i = 0; i < nums.Length; i++)
            {
                nums[i] = value;
            }
            return nums;

        }
    }

    public class MatrixOp
    {

        //638: Shopping Offers
        //https://youtu.be/iBwv-2IG-DQ?t=330
        //https://leetcode.com/problems/shopping-offers/

        public static int LowerPriceToPayForGivenNeedsAndOffers(List<int> price, List<List<int>> offers, List<int> needs)
        {
            /*
             * Discard any offer that has a higher price than buying individually
             * always use offers first
             * remaining units for each iterm can be filled individually
             */
            int res = 0;
            
            //1.Using Recursion
            res = LowerPriceToPayForGivenNeedsAndOffersRec(price, offers, needs);
            
            //2.Using Recursion with memoization 
            res = LowerPriceToPayForGivenNeedsAndOffersRecOptimalDP(price, offers, needs, new Dictionary<List<int>, int>());
            return res;
        }

        private static int LowerPriceToPayForGivenNeedsAndOffersRecOptimalDP(List<int> price, List<List<int>> offers, List<int> needs, Dictionary<List<int>, int> map)
        {
            if (map.ContainsKey(needs))
                return map[needs];

            int j = 0, res = TotalPriceForNeedsWithoutOffers(price, needs);
            foreach (List<int> offer in offers)
            {
                List<int> cloNeeds = new List<int>(needs);

                for (j = 0; j < needs.Count; j++)
                {
                    int diff = cloNeeds[j] - offer[j];
                    if (diff < 0) break; //Can't use this offer as need of an item is more

                    cloNeeds[j] = diff;
                }
                if (j == needs.Count)
                {
                    res = Math.Min(res, offer[j] + LowerPriceToPayForGivenNeedsAndOffersRecOptimalDP(price, offers, cloNeeds , map));
                }
            }
            map[needs] = res;
            return res;

        }

        private static int LowerPriceToPayForGivenNeedsAndOffersRec(List<int> price, List<List<int>> offers, List<int> needs)
        {
            int j = 0;
            //Determind cost of buying items per needs array
            int res = TotalPriceForNeedsWithoutOffers(price, needs);

            foreach(List<int> offer in offers)
            {
                List<int> cloNeeds = new List<int>(needs);

                for(j=0; j< needs.Count; j++)
                {
                    int diff = cloNeeds[j] - offer[j];
                    if (diff < 0) break; //Can't use this offer as need of an item is more

                    cloNeeds[j] = diff;
                }
                if (j == needs.Count)
                {
                    res = Math.Min(res, offer[j] + LowerPriceToPayForGivenNeedsAndOffersRec(price,offers, cloNeeds));
                }               
            }
            return res;

        }

        private static int TotalPriceForNeedsWithoutOffers(List<int> price, List<int> needs)
        {
            int sum = 0;

            for(int i=0; i< price.Count; ++i)
            {
                sum += price[i] * needs[i];
            }
            return sum;
        }

        //https://leetcode.com/problems/number-of-provinces/
        //547. Number of Provinces
        public static int FindNumberOfConnectedProvinces(int[,] cities)
        {
            int numOfProvinces=0;
            bool[] visited = new bool[cities.GetLength(1)];
            Queue<int> q = new Queue<int>();
            for(int i=0; i<cities.GetLength(0); ++i)
            {
                if (!visited[i])
                {
                    q.Enqueue(i);
                    /*DFS
                    Time complexity : O(n^2).The complete matrix of size n^2n 2 is traversed.
                    Space complexity : O(n)O(n). visitedvisited array of size nn is used.
                    */
                    FindNumberOfConnectedProvincesDfs(cities, visited, i);

                    /*
                     * BFS
                     * 
                     */
                    
                    FindNumberOfConnectedProvincesBfs(cities, visited, q);

                    numOfProvinces += 1;
                }
            }
            return numOfProvinces;
        }

        private static void FindNumberOfConnectedProvincesBfs(int[,] cities, bool[] visited, Queue<int> q)
        {
            while (q.Count > 0)
            {

                var i = q.Dequeue();

                visited[i] = true;
                for (int j = 0; j < cities.GetLength(1); ++j)
                {
                    if (cities[i, j] == 1 && !visited[j])
                    {
                        q.Enqueue(j);
                    }

                }


            }
            
        }

        private static void FindNumberOfConnectedProvincesDfs(int[,] cities, bool[] visited, int i)
        {
            for(int j=0; j<cities.GetLength(1); j++)
            {
                if(cities[i, j] ==1 && !visited[j])
                {
                    visited[j] = true;
                    FindNumberOfConnectedProvincesDfs(cities, visited, j);
                }

            }
        }

        //Floyd Warshall algorithm | All pairs shortest path
        //https://www.youtube.com/watch?v=nV_wOZnhbog&list=RDCMUCnxhETjJtTPs37hOZ7vQ88g&index=2
        //https://gist.github.com/SuryaPratapK/19c9acc834711f7e5cd8b6f710b0344f
        ////TIME COMPLEXITY: O(V^3)
        public static void FindAllPairsShortestPath()
        {
            int[,] graph = {
                { 0, 1, 4, int.MaxValue, int.MaxValue, int.MaxValue},
						{ int.MaxValue, 0, 4, 2, 7, int.MaxValue},
						{ int.MaxValue, int.MaxValue, 0, 3, 4, int.MaxValue},
						{ int.MaxValue, int.MaxValue, int.MaxValue, 0, int.MaxValue, 4},
						{ int.MaxValue, int.MaxValue, int.MaxValue, 3, 0, int.MaxValue},
						{ int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue, 5, 0}
            };
            FloydWarshall(graph);

        }

        private static void FloydWarshall(int[,] graph)
        {
            int[,] dist = new int[graph.GetLength(0), graph.GetLength(1)];

            //Assign all values of graph to allPairs_SP
            for (int i = 0; i < graph.GetLength(0); ++i)
                for (int j = 0; j < graph.GetLength(1); ++j)
                    dist[i, j] = graph[i, j];

            //Find all pairs shortest path by trying all possible paths
            for(int k=0; k< graph.GetLength(0); k++) //Try all intermediate nodes
            {
                for (int i = 0; i < graph.GetLength(0); ++i) //Try for all possible starting position
                    for (int j = 0; j < graph.GetLength(1); ++j) //Try for all possible ending position
                    {
                        if (dist[i, k] == int.MaxValue || dist[k, j] == int.MaxValue) //SKIP if K is unreachable from i or j is unreachable from k
                            continue;
                        else if (dist[i, k] + dist[k, j] < dist[i, j]) //Check if new distance is shorter via vertex K
                            dist[i, j] = dist[j, k] + dist[k, j];

                    }




            }
            //Check for negative edge weight cycle
            for (int i = 0; i < graph.GetLength(0); ++i)
                if (dist[i,i] < 0)
                {
                   Console.WriteLine( "Negative edge weight cycle is present\n");
                    return;
                }


            //Print Shortest Path Graph
            //(Values printed as INT_MAX defines there is no path)
            for (int i = 1; i < graph.GetLength(0); ++i)
            {
                for (int j = 0; j < graph.GetLength(0); ++j)
                   Console.WriteLine("{0}  ->  {1} . distance is ",  i, j, dist[i,j]);
            }


        }

        //Dijkstra algorithm | Single source shortest path algorithm
        //Algo: https://www.youtube.com/watch?v=Sj5Z-jaE2x0&list=RDCMUCnxhETjJtTPs37hOZ7vQ88g&index=2
        //Implementation: https://www.youtube.com/watch?v=t2d-XYuPfg0&list=RDCMUCnxhETjJtTPs37hOZ7vQ88g&start_radio=1&rv=t2d-XYuPfg0&t=7
        //https://gist.github.com/SuryaPratapK/531ec1fd8efdaeb0c098b89a7a1a9d3e
        /*
         * //TIME COMPLEXITY: O(V^2) - Below one
           //TIME COMPLEXITY: (using Min-Heap + Adjacency_List): O(ElogV)
         */
        public static void SingleSourceShortestPathCoseDijkstra()
        {
            //User Adjaceny List to reduce runtime complexity provided edges as input
            int[,] adjMatrix = { {0, 1, 4, 0,0,0},
                             {1, 0, 4, 2,7,0},
                             {4, 4, 0, 3,5,0}, 
                             {0, 2, 3, 0,4,6},
                             {0, 7, 5, 4,0,7 }, 
                             {0, 0, 0, 6,7,0 } };

            Dijkstra(adjMatrix);
        }

        private static void Dijkstra(int[,] adjMatrix)
        {
            int[] parent = new int[adjMatrix.GetLength(0)]; //Stores Shortest Path Structure
            int[] value = new int[adjMatrix.GetLength(0)].Populate(int.MaxValue); //Keeps shortest path values to each vertex from source
            bool[] processed = new bool[adjMatrix.GetLength(0)]; //TRUE->Vertex is processed

            //Assuming start point as Node-0
            parent[0] = -1; //Start node has no parent
            value[0] = 0; //start node has value=0 to get picked 1st

            //Include (V-1) edges to cover all V-vertices
            for (int i=0; i< adjMatrix.GetLength(0)-1; ++i)
            {
                //Select best Vertex by applying greedy method
                int u = SelectMinVertex(value, processed);
                processed[u] = true;    //Include new Vertex in shortest Path Graph

                //Relax adjacent vertices (not yet included in shortest path graph)
                for(int j=0; j< adjMatrix.GetLength(1); ++j)
                {
                    /* 3 conditions to relax:-
                      1.Edge is present from U to j.
                      2.Vertex j is not included in shortest path graph
                      3.Edge weight is smaller than current edge weight
                    */

                    if(adjMatrix[u,j]!=0 && !processed[j] && value[u] != int.MaxValue
                        && (value[u]+adjMatrix[u,j] < value[j]))
                    {
                        value[j] = value[u] + adjMatrix[u, j];
                        parent[j] = u;
                    }

                }

            }
            //Print Shortest Path Graph
            for(int i=1; i<adjMatrix.GetLength(0); i++)
            {
                Console.WriteLine("U->V: {0}->{1} with weight {2}", parent[i], i, value[i]);
            }

        }

        private static int SelectMinVertex(int[] value, bool[] processed)
        {
            //Use Max-Heap
            int min = int.MaxValue;
            int vertex=0;

            for (int i=0; i<value.Length; i++)
            {
                if(!processed[i] && value[i] < min)
                {
                    vertex = i;
                    min = value[i];
                }
            }

            return vertex;
        }


        //https://www.youtube.com/watch?v=60RbWlDFsmI
        //Cheapest Flights price Within K Stops using DFS + Pruning | Find Min cost path to reach from src to dest within K stops
        //***Applicable Algo's***
        //Dijkastra Algo - Vpower2 (v square) - can't handle negative edge weight cycles | Floyds Warshall - V3(V Cube) AKA all pairs shortest path
        //Bellmon Ford - V3(V Cube) - handles negative edge weight cycles bt in this case, cost is always positive and so not useful
        //BFS with optional Heap | DFS + Pruning | DFS + Memorization
        public static int FindCheapestPriceWithKStops(int n, int[,] flights, int src, int dest, int k)
        {
            Dictionary<int, List<int>> adjList = new Dictionary<int, List<int>>();
            int[,] costMatrix = new int[n,n];

            for(int i=0; i< flights.GetLength(0); i++)
            {
                if (!adjList.ContainsKey(flights[i, 0]))
                    adjList[flights[i, 0]] = new List<int>();

                adjList[flights[i, 0]].Add(flights[i, 1]);
                costMatrix[flights[i, 0], flights[i, 1]] = flights[i, 2];

            }
            bool[] visited = new bool[n];
            
            int totalPathCost = int.MaxValue;

            FindCheapestPriceWithKStopsDfsPrune(adjList, costMatrix, src, dest, k, ref totalPathCost, 0, visited);

            if (totalPathCost == int.MaxValue)
                return -1;
            
            return totalPathCost;
        }

        private static void FindCheapestPriceWithKStopsDfsPrune(Dictionary<int, List<int>> adjList, int[,] costMatrix, int src, int dest, int k, ref int totalPathCost, int currentPathCost, bool[] visited)
        {
            if (k < -1)
                return;

            if (src == dest)
            {
                totalPathCost = Math.Min(totalPathCost, currentPathCost);
            }
            visited[src] = true;

            for(int i=0; i< adjList[src].Count; i++)
            {
                if((!visited[adjList[src][i]]) && (currentPathCost+costMatrix[src, adjList[src][i]] <= totalPathCost))
                    FindCheapestPriceWithKStopsDfsPrune(adjList, costMatrix, adjList[src][i], dest, k-1, ref totalPathCost, currentPathCost+ costMatrix[src, adjList[src][i]], visited);

            }
            visited[src] = false;
        }


        //https://leetcode.com/problems/course-schedule/solution/
        //CanFinishAllCourses Scheduled
        private class GNode
        {
            public int inDegrees = 0;
            public List<int> outNodes = new List<int>();
        }
        public static bool CanFinishCourses(int numCourses, int[,] preReq)
        {
            if(preReq.Length ==0)
                return true; // no cycle could be formed in empty graph.

            Dictionary<int, GNode> graph = new Dictionary<int, GNode>();

            //Build Graph first - Adjacency List
            for(int i=0; i< preReq.GetLength(0); i++)
            {
                GNode prevCourse = GetCreatedGNode(graph, preReq[i,1]);
                GNode nxtCourse = GetCreatedGNode(graph, preReq[i,0]);

                prevCourse.outNodes.Add(preReq[i,0]);
                nxtCourse.inDegrees+=1;


            }
            // We start from courses that have no prerequisites/dependencies
            int totalDependencies = preReq.GetLength(0);
            
            Queue<int> noDepCourseQ = new Queue<int>();
            foreach(int key in graph.Keys)
            {
                if (graph[key].inDegrees == 0)
                    noDepCourseQ.Enqueue(key);
            }
            int removedEdges = 0;

            while(noDepCourseQ.Count() > 0)
            {
                int course = noDepCourseQ.Dequeue();

                foreach(int nxtCourse in graph[course].outNodes)
                {
                    GNode childNode = graph[nxtCourse];
                    childNode.inDegrees-=1;
                    removedEdges += 1;

                    if (childNode.inDegrees == 0)
                        noDepCourseQ.Enqueue(nxtCourse);
                }

            }
            if (removedEdges != totalDependencies)
                // if there are still some edges left, then there exist some cycles
                // Due to the dead-lock (dependencies), we cannot remove the cyclic edges
                return false;
            else
                return true;


        }

        private static GNode GetCreatedGNode(Dictionary<int, GNode> graph, int course)
        {
            
            if (!graph.ContainsKey(course))
            {
                graph[course] = new GNode();
            }
            return graph[course];
        }

        //https://leetcode.com/problems/course-schedule-ii/solution/
        //https://www.youtube.com/watch?v=qe_pQCh09yU&list=RDCMUCnxhETjJtTPs37hOZ7vQ88g&index=2
        //210. Course Schedule II
        //Topological ordering/sort using node InDegree
        public static int[] FindCourseOrder(int numCourses, int[,] preReq)
        {
            //bool isPossible = false;
            Dictionary<int, List<int>> adjList = new Dictionary<int, List<int>>();

            int[] inDegree = new int[numCourses];

            int[] topologicalOrder = new int[numCourses];

            // Create the adjacency list representation of the graph
            for(int i=0; i<preReq.GetLength(0); i++)
            {
                int dest = preReq[i, 0];
                int src = preReq[i, 1];

                if (adjList.ContainsKey(src))
                    adjList[src] = new List<int>();

                adjList[src].Add(dest);

                // Record in-degree of each vertex
                inDegree[dest] += 1;
            }
            if (IsCycleExists(adjList, numCourses))
                return new int[0];

            //***DFS************
            DS.Stack<int> stack = new DS.Stack<int>(numCourses);
            bool[] visited = new bool[numCourses];

            //Apply DFS and and find topological sort
            for (int i = 0; i < numCourses; i++)
                if (!visited[i])
                    FindCourseOrderDfs(adjList, i, visited, stack);

            int s = 0;
            while (!stack.IsEmpty())
            {
                topologicalOrder[s++] = stack.Peek();
                stack.Pop();
            }
            //***BFS************
            // Add all vertices with 0 in-degree to the queue
            Queue<int> q = new Queue<int>();
            for (int i = 0; i < numCourses; i++)
                if (inDegree[i] == 0)
                    q.Enqueue(i);
            
            int i1 = 0;
            // Process until the Q becomes empty
            while (q.Count > 0)
            {
                int node = q.Dequeue();

                topologicalOrder[i1++] = node;

                if (adjList.ContainsKey(node))
                {
                    foreach (int i2 in adjList[node])
                    {
                        inDegree[i2]--;

                        // If in-degree of a neighbor becomes 0, add it to the Q
                        if (inDegree[i2] == 0)
                            q.Enqueue(i2);
                    }
                }

            }
            // Check to see if topological sort is possible or not.
            if (i1 == numCourses) return topologicalOrder;

            return new int[0];
        }

        private static void FindCourseOrderDfs(Dictionary<int, List<int>> adjList, int v, bool[] visited, DS.Stack<int> stack)
        {
            visited[v] = true;

            for (int i = 0; i < adjList[v].Count; i++)
                if (!visited[adjList[v][i]])
                    FindCourseOrderDfs(adjList, adjList[v][i], visited, stack);

            stack.Push(v);

            
        }

        private static bool IsCycleExists(Dictionary<int, List<int>> adjList, int numCourses)
        {
            // 0 - Not visited/ 1- visited / 2 -visited & processed
            int[] visited = new int[numCourses];

            for(int i=0; i<numCourses; i++)
            {
                if (visited[i]==0)
                    if (HasCycleDfs(adjList, visited, i))
                        return true;
            }
            return false;
        }

        private static bool HasCycleDfs(Dictionary<int, List<int>> adjList, int[] visited, int v)
        {
            if (visited[v] == 1)
                return true;
            if (visited[v] == 2)
                return false;

            visited[v] = 1; // Mark current node as visited

            for (int i = 0; i < adjList[v].Count; i++)
                if (HasCycleDfs(adjList, visited, adjList[v][i]))
                    return true;
            
            visited[v] = 2; // Mark current node as processed
            return false;

        }

        //https://youtu.be/nNGSZdx6F3M?list=RDCMUChQRyFNgb7lbfzoacC5hk_A&t=736
        public static List<List<string>> NQueens(int n)
        {
            char[,] board = new char[n, n];
            for(int i=0; i<n; i++)
            {
                for(int j=0; j<n; j++)
                {
                    board[i, j] = '.';
                }
            }
            List<List<string>> result = new List<List<string>>();
            DfsNQueens(board, result, 0);
            return result;

        }
        private static void DfsNQueens(char[,] board, List<List<string>> result, int colIndex)
        {
            if(colIndex == board.GetLength(0))
            {
                result.Add(Construct(board));
                return;
            }
            
            for (int i=0; i< board.GetLength(0); i++)
            {
                //Console.WriteLine("{0}-{1}-{2}", i, colIndex, string.Join("", Construct(board).ToArray()));

                if (Validate(board, i, colIndex))
                {
                    board[i, colIndex] = 'Q';
                    DfsNQueens(board, result, colIndex+1);
                    board[i, colIndex] = '.';
                }
            }
        }

        private static bool Validate(char[,] board, int rowIndex, int colIndex)
        {
            for(int i=0; i<board.GetLength(0); i++)
            {
                for(int j=0; j< colIndex; j++)
                {
                    if (board[i, j] == 'Q' && (rowIndex + j == colIndex + i || rowIndex + colIndex == i + j || rowIndex == i))
                        return false;
                }
            }
            return true;

        }

        private static List<string> Construct(char[,] board)
        {
            List<string> res = new List<string>();
            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                    res.Add("["+i+","+j+"]"+board[i, j].ToString());

            return res;
        }

        //https://www.youtube.com/watch?v=5dJSZLmDsxk&list=PLBZBJbE_rGRVnpitdvpdY9952IsKMDuev&index=7
        //Count Negative Integers in Row/Column-Wise Sorted Matrix
        public static int CountNegativeInRowColSortedMatric(int[,] matrix)
        {
            int res = 0;

            int i = 0, j = matrix.GetLength(1) - 1;

            while (j >= 0 && i < matrix.GetLength(0))
            {
                if (matrix[i, j] < 0)
                {
                    res += j + 1;
                    i += 1;
                }
                else
                    j -= 1;
            }

            return res;
        }

        //https://www.youtube.com/watch?v=eaYX0Ee0Kcg&list=PLBZBJbE_rGRVnpitdvpdY9952IsKMDuev&index=5
        //K closest points to origin
        public static int[,] KClosestPointsToOrigin(int[,] coordinates, int k)
        {
            var map = new Dictionary<int, int[]>();
            Heap heap = new Heap();

            for (int i = 0; i < coordinates.GetLength(0); i++)
            {
                int x = coordinates[i, 0];
                int y = coordinates[i, 1];

                int key = x * x + y * y;
                if (!map.ContainsKey(key))
                    map[key] = new int[] { x, y };

                map[key] = new int[] { x, y };
            }

            foreach (int key in map.Keys)
            {
                heap.Push(key);
                if (heap.Size > k)
                    heap.Pop();

            }
            int[,] result = new int[k, 2];
            while (k > 0)
            {
                var a = heap.Pop();
                k--;
                result[k, 0] = map[a][0];
                result[k, 1] = map[a][1];

            }

            return result;
        }

        //https://www.youtube.com/watch?v=8nlmcgy7vso&list=PLtQWXpf5JNGJrA4oZNuF8pRfdxRq3XVm9
        public int MaxPathSumGold(int[,] grid)
        {
            if (grid == null || grid.Length == 0) return 0;
            int max=0;
            int m = grid.GetLength(0);
            int n = grid.GetLength(1);
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (grid[i,j] > 0)
                    {
                        int gold = FindMaxGold(grid, i, j,m,n, new bool[m,n]);
                        Console.WriteLine(gold);
                        max = Math.Max(gold, max);
                    }
                }
            }
            return max;

        
        }

        private int FindMaxGold(int[,] grid, int i, int j, int m, int n, bool[,] visited)
        {
            if (i < 0 || i >= grid.GetLength(0) || j < 0 || j >= grid.GetLength(1) || grid[i,j] == 0 || visited[i,j]) return 0;

            visited[i, j] = true;

            int left = FindMaxGold(grid, i, j - 1, m,  n, visited);
            int right = FindMaxGold(grid, i, j + 1, m, n, visited);
            int up = FindMaxGold(grid, i - 1, j, m, n,visited);
            int down = FindMaxGold(grid, i + 1, j,m,n, visited);

            visited[i, j] = false;
            return Math.Max(left, Math.Max(right, Math.Max(up, down)))+ grid[i,j];

        }

        //https://www.youtube.com/watch?v=c1ZxUOHlulo&list=PLtQWXpf5JNGJrA4oZNuF8pRfdxRq3XVm9&index=2
        public int NumberOfDistinctIslands(int[][] grid)
        {
            
            if (grid == null || grid.Length == 0) return 0;
            HashSet<string> set = new HashSet<string>();

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i][j] == 1)
                    {
                        //START - X
                        // Outofbounds or Water - O
                        string path = ComputePath(grid, i, j,  "X");
                        set.Add(path);
                    }
                }
            }

            return set.Count();

        }

        private string ComputePath(int[][] grid, int i, int j, string direction)
        {
            if (i < 0 || i >= grid.GetLength(0) || j < 0 || j >= grid.GetLength(1) || grid[i][j] == 0) return "O";

            grid[i][j] = 0;

            string left = ComputePath(grid, i, j - 1, "L");
            string right = ComputePath(grid, i, j +1, "R");
            string up = ComputePath(grid, i-1, j, "U");
            string down= ComputePath(grid, i+1, j , "D");


            return direction + left + right + up + down;
        }

        public int NumberOfIslands(int[][] arr)
        {
            if (arr == null || arr.Length ==0) return 0;
            int numberOfIslands = 0;

            for(int i=0; i<arr.GetLength(0); i++)
            {
                for (int j=0; j<arr.GetLength(1); j++)
                {
                    if(arr[i][j] == 1)
                    {
                        numberOfIslands++;
                        DfsIslands(arr, i, j);
                    }
                }
            }
            return numberOfIslands;
        }
        private void DfsIslands(int[][] grid, int i , int j)
        {
            if (i < 0 || i >= grid.GetLength(0) || j < 0 || j >= grid.GetLength(1) || grid[i][j] != 1) return;

            grid[i][j] = 0;

            DfsIslands(grid, i - 1, j);
            DfsIslands(grid, i, j + 1);
            DfsIslands(grid, i, j - 1);
            DfsIslands(grid, i + 1, j);

        }
        //https://leetcode.com/problems/flood-fill/
        public int[,] FloodFill(int[,] image, int sr, int sc, int newClr)
        {
            int rowCnt = image.GetLength(0);
            int colCnt = image.GetLength(1);

            if (sr < 0 || sr >= rowCnt || sc < 0 || sc >= colCnt)
                throw new IndexOutOfRangeException();
            if (image.Length <= 1) throw new Exception("Insuffient Image");

            if (newClr == image[sr, sc]) return image;

            
            int srcClr = image[sr, sc];

            Dfs(image, sr, sc, rowCnt, colCnt, srcClr, newClr);
            return image;
        }
        void Dfs(int [,] image, int sr, int sc, int rowCnt, int colCnt, int srcClr, int newClr)
        {
            if (sr < 0 || sr >= rowCnt || sc < 0 || sc >= colCnt) return;
            else if (srcClr != image[sr, sc]) return;

            image[sr, sc] = newClr;

            Dfs(image, sr + 1, sc, rowCnt, colCnt, srcClr, newClr);
            Dfs(image, sr - 1, sc, rowCnt, colCnt, srcClr, newClr);
            Dfs(image, sr, sc + 1, rowCnt, colCnt, srcClr, newClr);
            Dfs(image, sr, sc - 1, rowCnt, colCnt, srcClr, newClr);



        }

    }
}
