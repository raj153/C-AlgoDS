using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlGoLiv.ProbSol.Logic;
namespace AlGoTest
{
    [TestClass]
    public class LogicSpec
    {
        [TestMethod]
        public void TestCountSubArrSumToK()
        {
            var subCnt2 = Logic.FetchRainWaterTrapBetweenTowersOpt(new int[] { 1, 5, 2, 3, 1, 7, 2,4 });
            Assert.IsTrue(subCnt2 == 11);

            subCnt2 = Logic.FetchRainWaterTrapBetweenTowersOpt(new int[] { 2,0,2});
            Assert.IsTrue(subCnt2 == 2);

            subCnt2 = Logic.FetchRainWaterTrapBetweenTowersOpt(new int[] {3, 0 ,0,2,0,4 });
            Assert.IsTrue(subCnt2 == 10);

            subCnt2 = Logic.FetchRainWaterTrapBetweenTowersOpt(new int[] { 0, 1, 0, 2, 1, 0, 1, 3, 2, 1, 2, 1 });
            Assert.IsTrue(subCnt2 == 6);
            
        }

    }
}
