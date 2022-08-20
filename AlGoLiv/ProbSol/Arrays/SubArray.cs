using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.Arrays
{
    public class SubArray
    {
        //Subarray with given sum k
        //https://www.youtube.com/watch?v=Ofl4KgFhLsM
        public static bool SubArraySumK(int[] nums, int k)
        {
            int left = 0, right = 0, curSum=0;

            for(; right<nums.Length;)
            {

                if (curSum > k)
                {
                    curSum -= nums[left];
                    left += 1;
                }
                else
                {
                    curSum += nums[right];

                    if (curSum == k) return true;

                    right += 1;
                }
            }
            return false;

        }

        //https://www.youtube.com/watch?v=YxuK6A3SvTs
        //Longest sum contiguous subarray - Kadane's Algo
        public static int LongestSumContigousSubArray(int[] nums)
        {
            int localMax = 0, globalMax = int.MinValue;

            //Max Subarry positions
            int m, n = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                localMax += nums[i];

                if (localMax < nums[i])
                {
                    localMax = nums[i];
                    m = i;
                }

                if (localMax > globalMax)
                {
                    globalMax = localMax;
                    n = i;
                }

            }
            return globalMax;
        }
        
        /// <summary>
        /// Find the first maximum length even word from a string
        /*https://www.geeksforgeeks.org/find-the-first-maximum-length-even-word-from-a-string/
         
        Number of subarrays having sum exactly equal to k
        Given an unsorted array of integers, find the number of subarrays having sum exactly equal to a given number k.

        Examples:

        Input : arr[] = {10, 2, -2, -20, 10}, 
                k = -10
        Output : 3
        Subarrays: arr[0...3], arr[1...4], arr[3..4]
        have sum exactly equal to -10.

        Input : arr[] = {9, 4, 20, 3, 10, 5},
                    k = 33
        Output : 2
        Subarrays : arr[0...2], arr[2...4] have sum
        exactly equal to 33.
        */
        /// </summary>
        public static int CountSubArrSumToK(int[] arr, int k)
        {
            int subArrCnt = 0;

            for(int iCnt =0; iCnt<arr.Length; iCnt++)
            {
                int sum = arr[iCnt];
                for (int j = iCnt+1; j < arr.Length; j++)
                {
                    sum += arr[j];
                    if (sum == k)
                    {
                        subArrCnt++;
                        break;
                    }
                }
            }
            
            return subArrCnt;

        }
        public static int CountSubArrSumToKMemory(int[] arr, int sum)
        {
            int subArrCnt = 0;
            Dictionary<int, int> dict = new Dictionary<int, int>();
            int currsum =0;
            for (int iCnt = 0; iCnt < arr.Length; iCnt++)
            {
                // Add current element to sum so far.  
                currsum += arr[iCnt];

                // If currsum is equal to desired sum,  
                // then a new subarray is found. So  
                // increase count of subarrays.  
                if (currsum == sum)
                    subArrCnt++;

                // currsum exceeds given sum by currsum   
                //  - sum. Find number of subarrays having   
                // this sum and exclude those subarrays  
                // from currsum by increasing count by   
                // same amount.  
                if (dict.ContainsKey(currsum - sum))
                    subArrCnt += dict[currsum - sum];

                
                // Add currsum value to count of   
                // different values of sum.  
                if (!dict.ContainsKey(currsum))
                    dict.Add(currsum, 1);
                else
                    dict[currsum]+= 1;
            }

            return subArrCnt;

        }


    }
}
