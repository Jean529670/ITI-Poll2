using System;
using System.Threading;
using System.Threading.Tasks;

namespace ITI.Poll.Infrastructure
{
    public sealed class PollContextAccessor : IPollContextAccessor
    {
        readonly PollContext _pollContext;
        readonly SemaphoreSlim _mutex;

        public PollContextAccessor(PollContext pollContext)
        {
            if (pollContext == null) throw new ArgumentNullException(nameof(pollContext));

            _pollContext = pollContext;
            _mutex = new SemaphoreSlim(1);
        }
        
        public void AcquirePollContext(Action<PollContext> action)
        {
            try
            {
                _mutex.Wait();
                action(_pollContext);
            }
            finally
            {
                _mutex.Release();
            }
        }

        public async Task AcquirePollContext(Func<PollContext, Task> asyncAction)
        {
            try
            {
                await _mutex.WaitAsync();
                await asyncAction(_pollContext);
            }
            finally
            {
                _mutex.Release();
            }
        }

        public async Task<T> AcquirePollContext<T>(Func<PollContext, Task<T>> asyncAction)
        {
            try
            {
                await _mutex.WaitAsync();
                return await asyncAction(_pollContext);
            }
            finally
            {
                _mutex.Release();
            }
        }

        public T AcquirePollContext<T>(Func<PollContext, T> action)
        {
            try
            {
                _mutex.Wait();
                return action(_pollContext);
            }
            finally
            {
                _mutex.Release();
            }
        }
    }
}