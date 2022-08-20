using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlGoLiv.ProbSol.DS;
namespace AlGoLiv.ProbSol.Arrays
{
    public class Misc
    {

        //https://leetcode.com/problems/best-time-to-buy-and-sell-stock/
        //121. Best Time to Buy and Sell Stock - in any ONE day to buy and other day to sell
        public static int  StockBuySellMaxProfit(int[] prices)
        {
            int maxProfit = 0;
            int minPrice = int.MaxValue;

            for(int i=0; i< prices.Length; i++)
            {
                if (prices[i] < minPrice)
                    minPrice = prices[i];
                else if ((prices[i] - minPrice) > maxProfit)
                    maxProfit = prices[i] - minPrice;

            }
            return maxProfit;
        }

        //Best time to buy and sell stock 2 | Valley peak approach | Leetcode #122
        //Multiple Buy and Sell allowed but not on same day
        //Recursion with DP memorization and final as below one 
        //https://www.youtube.com/watch?v=K8iHi8AW1ls
        public static int StockBuySellMaxProfit2(int[] prices)
        {
            int n = prices.Length;

            int diff = 0;

            for(int i=1; i< n; i++)
            {
                if (prices[i] > prices[i - 1])
                    diff += prices[i] - prices[i - 1];
            }

            return diff;
        }
        //https://www.youtube.com/watch?v=37s1_xBiqH0
        //https://gist.github.com/SuryaPratapK/0ae68f837c87c8504d4c682c521899c6
        //Best Time to Buy and Sell Stock III | Leetcode #123 - With minimum transactions        
        
        public static int StockBuySellMaxProfit3In2Transactions(int[] prices, int k)
        {
            /*   
               pos->current position
               t->transactions done
               bought->If current stock is bought
           */
            //Method 1. Using Recursion with DP's memorization            
            int[,,] mem = new int[2, 3, prices.Length]; ////3D Array

            int res = StockBuySellMaxProfit3In2TransactionsRecursionWithDP(prices, mem, 0, 2, 0);

            //Method 2. Using Divide & Conquer algo          
            res = StockBuySellMaxProfit3In2TransactionsOptimal(prices);
            return res;
        }

        private static int StockBuySellMaxProfit3In2TransactionsOptimal(int[] prices)
        {
            int n = prices.Length;
            int maxProfit = 0;
            if (n == 0) return 0;

            int[] left = new int[n];
            int[] right = new int[n];

            int leftMin = prices[0];

            //Fill 1st transaction (LEFT)
            for (int i=1; i< n; i++)
            {
                left[i] = Math.Max(left[i - 1], prices[i] - leftMin);
                leftMin = Math.Min(leftMin, prices[i]);
            }

            //Fill 2nd transaction (RIGHT)
            int rightMax = prices[n - 1];

            for(int i=n-2; i>=0; --i)
            {
                right[i] = Math.Max(right[i + 1], rightMax - prices[i]);
                rightMax = Math.Min(rightMax, prices[i]);
            }
            
            //Find Max Profit value
            maxProfit = right[0];
            for (int i = 1; i < n; i++)
                maxProfit = Math.Max(maxProfit, left[i - 1] + right[i]);

            return maxProfit;

        }

        private static int StockBuySellMaxProfit3In2TransactionsRecursionWithDP(int[] prices, int[,,] mem, int pos, int t, int bought)
        {
            if (pos >= prices.Length || t == 0) //Boundary cases
                return 0;
            //0 - Sell, 1 - bought
            if (mem[bought, t, pos] != -1) //Return if already calculated
                return mem[bought, t, pos];

            //3 choices for a position-->Buy/Sell/Skip

            int result = StockBuySellMaxProfit3In2TransactionsRecursionWithDP(prices, mem, pos + 1, t, bought); // SKIP

            if (bought == 1)
                result = Math.Max(result, StockBuySellMaxProfit3In2TransactionsRecursionWithDP(prices, mem, pos + 1, t, 0)+prices[pos]); //SELL
            else
                result = Math.Max(result, StockBuySellMaxProfit3In2TransactionsRecursionWithDP(prices, mem, pos + 1, t, 1) - prices[pos]); //BUY

            mem[bought, t, pos] = result;

            return result;
        }

        public class PriceComparator : IComparer<int[]>
        {
            public int Compare(int[] x, int[] y)
            {
                return x[0] - x[1] - (y[0] - y[1]);
                
            }
        }

        //City Schedule - Minimize cost
        //https://www.youtube.com/watch?v=vtNoP43hGJA
        //https://leetcode.com/problems/two-city-scheduling/
        public static int TwoCityScheduleMinimizeCostUsingSorting(int[,] costs)
        {
            // Sort by a gain which company has 
            // by sending a person to city A and not to city B
            
            //Array.Sort(costs,0, costs.Length, new PriceComparator());

            return 0;
        }
        public static int TwoCityScheduleMinimizeCostUsingHashMap(int[,] costs)
        {
            Heap heap = new Heap(); //Max Heap

            Dictionary<int, int[]> costsIndexMap = new Dictionary<int, int[]>();
            int minCost = 0;
            for (int i=0; i< costs.GetLength(0); ++i)
            {
                int costDiff = Math.Abs(costs[i, 0] - costs[i, 1]);
                heap.Push(costDiff);
                costsIndexMap[costDiff] = new int[] { costs[i, 0], costs[i, 1] }; 
            }
            int n = 1;
            while (heap.Size > 0)
            {
                var val = heap.Pop();

                if (n > costs.GetLength(0) / 2)
                    minCost += costsIndexMap[val][1];
                else
                    minCost += costsIndexMap[val][0];

                n++;
                
            }

            return minCost;
        }
        //Pair of songs with total durations divisable by 60
        //https://www.youtube.com/watch?v=toYgBIaUdfM
        public static int NoOfPairOfSongsTotalDurationDivisableByKUsingArrays(int[] times, int k)
        {
            int[] rem = new int[times.Length];

            foreach (int i in times)
                rem[i % k] += 1;
            
            int count = 0;

            //Special cases for 0 and k/2
            //n*n-1/2
            count += (times[0] * (times[0] - 1)) / 2;
            count += (times[k/2] * (times[k/2] - 1)) / 2;

            for (int i = 1; i <= ((k / 2) - 1); ++i)
                count += rem[i]*rem[k-i];

            return count;
        }
        public static int NoOfPairOfSongsTotalDurationDivisableByKUsingMap(int[] times, int k)
        {
            //key - reminder, value -> frequency
            Dictionary<int, int> map = new Dictionary<int, int>();

            for (int i = 0; i < times.Length; i++)
            {
                times[i] %= k;

                map[times[i]] += 1;
            }
            

            int count = 0;

            //Special cases for 0 and k/2
            //n*n-1/2
            
            foreach(KeyValuePair<int, int> item in map)
            {
                if (item.Key == 0 || item.Key == 30)
                    count += (item.Value * (item.Value - 1)) / 2;
                else if (item.Key < (k / 2) && map[k - item.Key] > 0)
                    count += item.Value * map[k - item.Key];


            }
            return count;
        }
        //Jump Game 
        //https://www.youtube.com/watch?v=muDPTDrpS28
        public static bool CanJumpToEnd(int[] nums)
        {
            int maxReachableIndex = 0;

            for(int i=0; i< nums.Length; i++)
            {
                if (maxReachableIndex < i)
                    return false;

                maxReachableIndex = Math.Max(maxReachableIndex, i + nums[i]);

            }
            return false;

        }
        //https://www.youtube.com/watch?v=38JLfQGVlkw
        //https://www.geeksforgeeks.org/minimum-number-platforms-required-railwaybus-station/
        //Minimum Platforms needed on  railway station with train arrival/dep = Maximum platforms needed at any time 
        //Minimum platforms needed in a railway station
        public static int MinPaltformNeededOptimal(int[] arrival, int[] dep)
        {
            Array.Sort(arrival);
            Array.Sort(dep);

            int i=1, j = 0;
            int res = 1, needPlatforms = 1;
            while(i < arrival.Length && j< dep.Length)
            {

                if (arrival[i] <= dep[j]) 
                {
                    needPlatforms += 1;
                    i++;
                }
                else if (arrival[i] > dep[j])
                {
                    needPlatforms -= 1;
                    j++;
                }

                res = Math.Max(res, needPlatforms);
            }

            return res;
        }
        //https://www.youtube.com/watch?v=38JLfQGVlkw
        //https://www.geeksforgeeks.org/minimum-number-platforms-required-railwaybus-station/
        //Minimum Platforms needed on railway station = Maximum platforms needed at any time

        //Minimum platforms needed in a railway station
        public static int MinPaltformNeededNaive(int[] arrival, int[] dep)
        {
            int res = 0;
            int neededPlatforms = 1;
            
            for(int i=0; i< arrival.Length; i++)
            {
                neededPlatforms = 1;
                for (int j=i+1; j< dep.Length; ++j)
                {
                    if (arrival[j] >= arrival[i] && arrival[j] <= dep[i] || arrival[i]>= arrival[j] && arrival[i] <= dep[j] ) //Overlapping
                    {
                        neededPlatforms++;

                    }
                }

                res = Math.Max(res, neededPlatforms);
            }


            return res;
        }

        //https://youtu.be/5o-kdjv7FD0?list=PLBZBJbE_rGRVnpitdvpdY9952IsKMDuev&t=887
        //Strircase steps
        public static int NumberOfWaysToCompleteStaircaseGivenXSteps(int n , int[] steps)            
        {
            if (n == 0) return 1;
            return 1; //TODO

        }

            //https://www.youtube.com/watch?v=5o-kdjv7FD0&list=PLBZBJbE_rGRVnpitdvpdY9952IsKMDuev&index=2
            //Strircase steps
            public static int NumberOfWaysToCompleteStaircase(int n)
        {
            if (n == 0 || n == 1) return 1;

            // 1 or 2 steps
            return NumberOfWaysToCompleteStaircaseIterative(n-1)+ NumberOfWaysToCompleteStaircaseIterative(n-2);          


            
        }

        private static int NumberOfWaysToCompleteStaircaseIterative(int n)
        {
            //DP - Bottom-up approach
            if (n == 0 || n == 1) return 1;

            int[] memo = new int[n];

            memo[0]=1; memo[1] = 1;

            for (int i=2; i< n; i++)
            {
                memo[i] = memo[i - 1] + memo[i - 2];
            }

            return memo[n];
        }

        private static int NumberOfWaysToCompleteStaircaseDfsGivenXSteps(int n)
        {
            if (n == 0 || n == 1) return 1;
            return 1;

        }

        //https://www.youtube.com/watch?v=qli-JCrSwuk&list=PLBZBJbE_rGRVnpitdvpdY9952IsKMDuev&index=1
        //How many way to a decode a message
        public static int NumberWaysToDecode(string data)
        {
            
            return NumberWaysToDecodeDfs(data, data.Length, new int[data.Length]);

        }

        private static int NumberWaysToDecodeDfs(string data, int len, int[] memo)
        {

            if (len == 0) return 1;

            int s = data.Length - len;

            if (data[s] == '0')
                return 0;

            if (memo[len] != default)
                return memo[len];

            int res = NumberWaysToDecodeDfs(data, len - 1, memo);

            if (len >= 2 && int.Parse(data.Substring(s,s+1)) <= 26) 
                res += NumberWaysToDecodeDfs(data, len - 2, memo);

            memo[len] = res;
            return res;



            
        }

        //https://www.youtube.com/watch?v=9SnkdYXNIzM
        public static int FirstMissingPositiveInteger(int[] nums)
        {
            if (nums == null || nums.Length == 0) return 1;

            int n = nums.Length;
            bool isInputContainsOne = false;

            //step1
            foreach(int n1 in nums)
            {
                if (nums[n1] == 1)
                    isInputContainsOne = true;
                else if (nums[n1] <=0 || nums[n1] > n)
                     nums[n1] = 1;

                if (!isInputContainsOne) return 1;
                
                //step 2
                for(int i=0; i<n; i++)
                {
                    int index = Math.Abs(nums[i])-1;

                    if (nums[index] > 0) nums[index] = -1 * nums[index];
                }

                //step3
                for (int i = 0; i < n; i++)
                {
                    if (nums[i] > 0)
                    {
                        return i+1;
                    }

                }

                }
            //ex: [1,2,3] and missng is 4 as it will be [-1, -2, -3]
            return n+1;
            }
        public static  int  TownJudge(int n, int[,] people)
        {
            int[] trustCnt = new int[n+1];
            int[] trustByCnt = new int[n+1];

            for(int i=0; i<people.GetLength(0); i++)
            {
                trustCnt[people[i, 0]] += 1;
                trustByCnt[people[i, 1]] += 1;
            }

            for(int i=1; i <= n; i++){

                if (trustCnt[i] == 0 && trustByCnt[i] == n - 1) return i;
            }
            return -1;
        }

        //https://www.youtube.com/watch?v=yJv_ltADGuA
        public static string RemoveAdjacentDuplicates(string str)
        {
            string result = "";
            int i = 0;
            while(i< str.Length)
            {
                if (str[i] != str[i + 1])
                {
                    result += str[i];
                }

                    while (i + 1 < str.Length && str[i + 1] == str[i])
                        i++;
                 i++;
              

            }

            return result;
        }
    }
}
