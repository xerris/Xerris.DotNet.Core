using System;

namespace Xerris.DotNet.Core.Time
{
    public sealed class FreezeClock : IDisposable
    {
        private bool disposed;

        public FreezeClock()
        {
            ClockManager.Freeze();
        }

        public FreezeClock(DateTime dateTime)
        {
            ClockManager.Freeze(dateTime);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~FreezeClock()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing) ClockManager.Thaw();
            disposed = true;
        }
    }
}