using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlGoLiv.ProbSol.DS;
namespace AlGoTest
{
    [TestClass]
    public class StackSpec
    {
        [TestMethod]
        public void TestStackLogic()
        {
            var stack = new Stack<int>(3);
            Assert.IsTrue(stack.IsEmpty());
            stack.Push(2);
            stack.Push(1);
            Assert.IsTrue(stack.Pop() == 1);
            Assert.IsTrue(stack.Peek() == 2);
            stack.Pop();
            
            string str = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random r = new Random();

            Console.Write(str[r.Next(0, str.Length)]);
        }

    }
}
