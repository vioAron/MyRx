using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace ConcurrencyApp
{
    class Program
    {
        static void Main()
        {
            //OnNextDiffThreads();

            //SubscribeOn();

            Deadlock();

            Console.ReadKey();
        }

        private static void Deadlock()
        {
            var seq = new Subject<int>();

            var value = seq.First();

            seq.OnNext(1);
        }

        private static void SubscribeOn()
        {
            var observable = Observable.Create<int>(observer =>
                {
                    Console.WriteLine("OnNext() on threadId:{0}", Thread.CurrentThread.ManagedThreadId);

                    observer.OnNext(1);
                    observer.OnNext(2);
                    observer.OnNext(3);

                    observer.OnCompleted();

                    Console.WriteLine("Finished on threadId:{0}", Thread.CurrentThread.ManagedThreadId);

                    return Disposable.Empty;
                });

            //todo
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var sync = SynchronizationContext.Current;

            observable.SubscribeOn(NewThreadScheduler.Default).ObserveOn(sync).Subscribe(
                i => Console.WriteLine("Receiving({0}) on threadId: {1}", i, Thread.CurrentThread.ManagedThreadId),
                () => Console.WriteLine("OnCompleted on threadId:{0}", Thread.CurrentThread.ManagedThreadId));

            Console.WriteLine("-------> Subscribed on threadId:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        private static void OnNextDiffThreads()
        {
            Console.WriteLine("Starting on threadId: {0}", Thread.CurrentThread.ManagedThreadId);

            var subject = new Subject<int>();

            subject.Subscribe(i => Console.WriteLine("Receiving({0}) on threadId: {1}", i, Thread.CurrentThread.ManagedThreadId),
                              () => Console.WriteLine("Completed"));

            ParameterizedThreadStart notify = p =>
            {
                Console.WriteLine("OnNext({1}) on threadId:{0}", Thread.CurrentThread.ManagedThreadId, p);
                subject.OnNext((int)p);
            };

            notify(1);

            var t1 = new Thread(notify);
            var t2 = new Thread(notify);

            t1.Start(2);
            t2.Start(3);
        }
    }
}
