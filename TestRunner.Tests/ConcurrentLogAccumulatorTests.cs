using Microsoft.VisualStudio.TestTools.UnitTesting;
using Libraries.Log;

namespace TestRunner.Tests
{
    [TestClass]
    public class ConcurrentLogAccumulatorTests
    {
        [TestMethod]
        public void Is_Singleton()
        {
            var concurrentLogAccumulator1 = ConcurrentLogAccumulator.GetLogger;
            var concurrentLogAccumulator2 = ConcurrentLogAccumulator.GetLogger;

            Assert.AreEqual(concurrentLogAccumulator2, concurrentLogAccumulator1);
        }

        [TestMethod]
        public void Release_Returns_Logs()
        {
            var concurrentLogAccumulator = ConcurrentLogAccumulator.GetLogger;

            concurrentLogAccumulator.Log("Message 1");
            var log = concurrentLogAccumulator.Release();

            Assert.AreEqual(log.Count, 1);
        }

        [TestMethod]
        public void Release_Frees_Logs()
        {
            var concurrentLogAccumulator = ConcurrentLogAccumulator.GetLogger;

            concurrentLogAccumulator.Log("Message 1");
            var firstReleaseCall1 = concurrentLogAccumulator.Release();
            var secondReleaseCall2 = concurrentLogAccumulator.Release();

            Assert.AreEqual(secondReleaseCall2.Count, 0);
        }

        [TestMethod]
        public void Add_2_Message_To_Logger()
        {
            var concurrentLogAccumulator = ConcurrentLogAccumulator.GetLogger;

            concurrentLogAccumulator.Log("Message 1");
            concurrentLogAccumulator.Log("Message 2");
            var log = concurrentLogAccumulator.Release();

            Assert.AreEqual(log.Count, 2);
        }
    }
}
