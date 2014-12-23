using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace MyRxTestsApp
{
    public class NumberGenerator
    {
        public IObservable<long> GetGeneratedNumbers()
        {
            return GetGeneratedNumbers(Scheduler.Default);
        }

        public IObservable<long> GetGeneratedNumbers(IScheduler scheduler)
        {
            return Observable.Interval(TimeSpan.FromSeconds(1), scheduler).Take(5);
        }
    }
}
