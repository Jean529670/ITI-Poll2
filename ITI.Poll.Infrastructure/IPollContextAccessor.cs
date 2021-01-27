using System;
using System.Threading.Tasks;

namespace ITI.Poll.Infrastructure
{
    public interface IPollContextAccessor
    {
        void AcquirePollContext(Action<PollContext> action);

        T AcquirePollContext<T>(Func<PollContext, T> action);

        Task AcquirePollContext(Func<PollContext, Task> asyncAction);

        Task<T> AcquirePollContext<T>(Func<PollContext, Task<T>> asyncAction);
    }
}