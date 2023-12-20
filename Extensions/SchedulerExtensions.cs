using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace VendingMachine.Extensions
{
    public static class SchedulerExtensions
    {
        public static IDisposable Schedule(this IScheduler scheduler, TimeSpan timeSpan, Action action)
        {
            return scheduler.Schedule<object>(null, timeSpan, (s1, s2) =>
            {
                action();
                return Disposable.Empty;
            });
        }

        public static IDisposable Schedule(this IScheduler scheduler, Action action)
        {
            return scheduler.Schedule<object>(null, (s1, s2) =>
            {
                action();
                return Disposable.Empty;
            });
        }
    }
}