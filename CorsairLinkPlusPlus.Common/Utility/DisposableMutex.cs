using System;
using System.Threading;

namespace CorsairLinkPlusPlus.Common.Utility
{
    public class DisposableMutex
    {
        private readonly Mutex mutex;

        public DisposableMutex(string mutexName)
        {
            this.mutex = new Mutex(false, mutexName);
        }

        public IDisposable GetLock()
        {
            return new ActualDisposableMutex(mutex);
        }

        private class ActualDisposableMutex : IDisposable
        {
            private Mutex mutex;

            internal ActualDisposableMutex(Mutex mutex)
            {
                this.mutex = mutex;
                mutex.WaitOne();
            }

            public void Dispose()
            {
                if (mutex != null)
                    mutex.ReleaseMutex();
                mutex = null;
            }
        }
    }
}
