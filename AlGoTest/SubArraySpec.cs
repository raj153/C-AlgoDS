using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlGoLiv.ProbSol.Arrays;
namespace AlGoTest
{
    [TestClass]
    public class SubArraySpec
    {
        [TestMethod]
        public void TestCountSubArrSumToK()
        {
            var subCnt2 = SubArray.CountSubArrSumToKMemory(new int[] { 10, 2, -2, -20, 10 }, -10);
            var subCnt = SubArray.CountSubArrSumToK(new int[] { 10, 2, -2, -20, 10 }, -10);
            var subCnt1 = SubArray.CountSubArrSumToK(new int[] { 9, 4, 20, 3, 10, 5 },33);
        }

    }
}
