using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using TripPoint.WindowsPhone.Utils;

namespace TripPoint.Test.Utils
{
    [TestClass]
    public class CountdownTimerTest
    {
        private CountdownTimer _timer;

        [TestInitialize]
        public void InitializeTest()
        {
            _timer = new CountdownTimer();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            _timer.Stop();
            _timer = null;
        }

        [TestMethod]
        [Ignore]
        public void TestCountdown()
        {
            int counter = 0;

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.OnExplode = () =>
            {
                lock (_timer)
                {
                    counter++;
                    Monitor.Pulse(_timer);
                }
            };

            _timer.Start();

            // _timer.OnExposed never has a chance to run before Wait times out
            // TODO: figure out how to run the timer on a separate thread
            lock (_timer)
            {
                Monitor.Wait(_timer, 3000);
            }

            Assert.AreEqual<int>(1, counter, "The timer must explode exactly once");
        }
    }
}
