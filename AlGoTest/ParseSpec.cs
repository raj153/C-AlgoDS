using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlGoLiv.ProbSol.String;
namespace AlGoTest
{
    [TestClass]
    public class ParseSpec
    {
        [TestMethod]
        public void TestParseLogic()
        {
            Assert.IsTrue(Parse.ValidParanthesis("[{([()])}]"));
        }

    }
}
