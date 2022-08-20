using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.DS
{
    public class Backtracking
    {
        //https://www.youtube.com/watch?v=nNGSZdx6F3M&list=RDCMUChQRyFNgb7lbfzoacC5hk_A&index=3
        public static List<string> LetterCombinationsOfPhoneNum(string digits)
        {
            List<string> result = new List<string>();

            if (digits != null && digits.Length > 0) {
                string[] map = { "", "", "abc", "def", "ghi", "jkl", "mno", "pqrs", "tuv", "wxyz" };
                
                DfsLetterComb(map, digits, result, new StringBuilder(), 0);
             }
            return result; 

        }
        private static void DfsLetterComb(string[] map, string digits, List<string> result, StringBuilder sb, int index)
        {
            if (index == digits.Length)
            {
                result.Add(sb.ToString());
                return;
            }
            char c = digits[index];
            int digit = int.Parse(c.ToString());
            string letters = map[digit];

            for(int i=0; i<letters.Length; i++)
            {
                char ch = letters[i];
                sb.Append(ch);
                DfsLetterComb(map, digits, result, sb, index + 1);
                sb.Remove(sb.Length - 1,1);
            }



        }
        public static List<List<int>> CombinationSum(int[] candidates, int target)
        {
            List<List<int>> result = new List<List<int>>();
            Array.Sort(candidates);

            Dfs(candidates, result, target, new List<int>(), 0);
            return result;
        }

        private static void Dfs(int[] candidates, List<List<int>> result, int target, List<int> li, int start)
        {
            if (target == 0)
            {
                result.Add(new List<int>(li));
                return;
            }


            for (int i=start; i<candidates.Length; i++)
            {
                
                li.Add(candidates[i]);
                
                int newTarget = target - candidates[i];
                //Console.WriteLine(i+"#"+candidates[i]+"-"+start+"$"+ newTarget);
                if (newTarget >= 0)
                {
                    Dfs(candidates, result, newTarget, li, i);
                }
                Console.WriteLine(string.Join(",", li) + "--------------------");
                li.RemoveAt(li.Count - 1);
                
            }
            
        }

        
        
    }
}
