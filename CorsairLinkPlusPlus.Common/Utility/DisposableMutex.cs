#region LICENSE
/**
 * CorsairLinkPlusPlus
 * Copyright (c) 2014, Mark Dietzer & Simon Schick, All rights reserved.
 *
 * CorsairLinkPlusPlus is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3.0 of the License, or (at your option) any later version.
 *
 * CorsairLinkPlusPlus is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with CorsairLinkPlusPlus.
 */
 #endregion

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
