using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FoodStuffsControllerTests.src
{
    [TestClass()]
    public class MonitorLockThreadTest
    {
        private readonly object _LOCKED = new object();
        private string concurrentString;
        string expected = "Thread 1Thread 1Thread 2Thread 2";

        /// <summary>
        /// Test that locking a method ensures that one thread finishes before another starts.
        /// </summary>
        [TestMethod()]
        public void MonitorLockTest()
        {
            concurrentString = "";

            Thread t1 = new Thread(ConcurrencyWithLockTest);
            t1.Name = "Thread 1";
            t1.Start();

            Thread t2 = new Thread(ConcurrencyWithLockTest);
            t2.Name = "Thread 2";
            t2.Start();

            Thread.Sleep(10000);

            Assert.AreEqual(expected, concurrentString);
        }

        /// <summary>
        /// Test that both threads access the resource at once without a lock.
        /// </summary>
        [TestMethod()]
        public void MonitorNoLockTest()
        {
            concurrentString = "";

            Thread t1 = new Thread(ConcurrencyNoLockTest);
            t1.Name = "Thread 1";
            t1.Start();

            Thread t2 = new Thread(ConcurrencyNoLockTest);
            t2.Name = "Thread 2";
            t2.Start();

            Thread.Sleep(10000);

            Assert.AreNotEqual(expected, concurrentString);
        }

        /// <summary>
        /// Test that locking one method also prevents use of another.
        /// </summary>
        [TestMethod()]
        public void LockMultipleMethodsTest()
        {
            concurrentString = "";

            Thread t1 = new Thread(Method1);
            t1.Name = "Thread 1";
            t1.Start();

            Thread t2 = new Thread(Method2);
            t2.Name = "Thread 2";
            t2.Start();

            Thread.Sleep(10000);

            Assert.AreEqual(expected, concurrentString);
        }

            /// <summary>
            /// Call method2 twice, with a 2 second gap.
            /// Confirm that multiple threads cannot access the critical Method1 at the same time.
            /// </summary>
            private void ConcurrencyWithLockTest() 
        {
            Monitor.Enter(_LOCKED);
            try 
            {
                AddThreadNameToConcurrentString();
                Thread.Sleep(2000);
                AddThreadNameToConcurrentString();
            }
            finally { Monitor.Exit(_LOCKED); }
        }

        /// <summary>
        /// Call method2 twice, with a 2 second gap.
        /// Confirm that multiple threads cannot access the critical Method1 at the same time.
        /// </summary>
        private void ConcurrencyNoLockTest()
        {            
            AddThreadNameToConcurrentString();
            Thread.Sleep(2000);
            AddThreadNameToConcurrentString();             
        }

        /// <summary>
        /// Add the name of the current thread to the concurrentString.
        /// </summary>
        private void AddThreadNameToConcurrentString() 
        {
            concurrentString += Thread.CurrentThread.Name;
        }


        private void Method1() 
        {
            Monitor.Enter(_LOCKED);
            try
            {
                AddThreadNameToConcurrentString();
                Thread.Sleep(2000);
                AddThreadNameToConcurrentString();
            }
            finally { Monitor.Exit(_LOCKED); }
        }
        private void Method2() 
        {
            Monitor.Enter(_LOCKED);
            try
            {
                AddThreadNameToConcurrentString();
                Thread.Sleep(2000);
                AddThreadNameToConcurrentString();
            }
            finally { Monitor.Exit(_LOCKED); }
        }
    }
}
