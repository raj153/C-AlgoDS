using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlGoLiv.ProbSol.DS;
namespace AlGoLiv.ProbSol.String
{
    /// <summary>
    /// Problems revolves around text/strings
    /// </summary>
    public class Parse
    {

        /// <summary>
        /// Find the first maximum length even word from a string
        /*https://www.geeksforgeeks.org/find-the-first-maximum-length-even-word-from-a-string/
         
        *Given a string of words separated by spaces.The task is to find the first maximum length even word from the string. 
        * Eg: “You are given an array of n numbers” The answer would be “an” and not “of” because “an” comes before “of”.
        
        Examples:
        Input:  "this is a test string"
        Output:  string
        Even length words are this, is, test, string. Even
        maximum length word is string.

        Input:  "geeksforgeeks is a platform for geeks"
        Output:  platform
        Only even length word is platform.
        */
        /// </summary>
        public static string FindFirstMaxLenEvenWordFrmString(string s1)
        {
            var arrStr = s1.Split(' ');
            int index=0, maxEvenLen =0;
            for(int iCnt=0; iCnt<arrStr.Length; iCnt++)
            {
                if (arrStr[iCnt].Length >=2 && arrStr[iCnt].Length % 2 == 0 && arrStr[iCnt].Length>maxEvenLen)
                {
                    maxEvenLen = arrStr[iCnt].Length;
                    index = iCnt;
                }

            }

            return maxEvenLen ==0? "NoFound": arrStr[index];
        }

        public static bool ValidParanthesis(string paran)
        {
            DS.Stack<char> stack = new DS.Stack<char>(paran.Length);

            foreach(char ch in paran)
            {
                if(ch == '(' || ch == '{' || ch == '[')
                {
                    stack.Push(ch);
                }
                else
                {
                    if (stack.IsEmpty())
                        return false;
                    char ch1 = stack.Pop();
                    if (!IsMatchingPair(ch1, ch))
                        return false;
                }
            }
            return stack.IsEmpty();
            
        }
        private static bool IsMatchingPair(char ch1, char ch2)
        {
            if (ch1 == '[' && ch2 == ']')
                return true;
            if (ch1 == '(' && ch2 == ')')
                return true;
            if (ch1 == '{' && ch2 == '}')
                return true;
            return false;
        }

    }
}
