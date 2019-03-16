using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlGoLiv.ProbSol.String;
namespace AlGoRun
{
    class Program
    {
        static void Main(string[] args)
        {

            string str = Console.ReadLine();
            Console.WriteLine($"First Max Len Even Word is {Parse.FindFirstMaxLenEvenWordFrmString(str)}");
            Console.ReadLine();
        }

    }
}
