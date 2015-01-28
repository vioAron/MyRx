﻿using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyApp
{
    class Program
    {

        static void Main()
        {
            //OnNextDiffThreads();

            //SubscribeOn();

            //Deadlock();

            //PassTheState(Scheduler.NewThread);

            //Cancellation(Scheduler.Immediate);

            //UseCancellationToken();

            //Recursion(Scheduler.NewThread);

            //UseImmediateScheduler();

            Console.WriteLine("CurrentThread");
            Console.WriteLine();
            UseCurrentThreadScheduler(Scheduler.CurrentThread);

            Console.WriteLine();
            Console.WriteLine("Immediate");
            Console.WriteLine();
            UseCurrentThreadScheduler(Scheduler.Immediate);

            Console.ReadKey();
        }

        private static void UseCurrentThreadScheduler(IScheduler scheduler)
        {
            var leafAction = new Action(() => Console.WriteLine("leafAction"));

            var innerAction = new Action(() =>
                {
                    Console.WriteLine("innerActionBegin");

                    scheduler.Schedule(leafAction);

                    Console.WriteLine("innerActionEnd");
                });

            var outerAction = new Action(() => 
            {
                Console.WriteLine("outerActionBegin");

                scheduler.Schedule(innerAction);

                Console.WriteLine("outerActionEnd");
            });

            scheduler.Schedule(outerAction);
        }

        private static void UseImmediateScheduler()
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString());

            Scheduler.Immediate.Schedule(TimeSpan.FromSeconds(5), () => Console.WriteLine("Run!" + DateTime.Now.ToLongTimeString()));
        }

        private static void Recursion(IScheduler scheduler)
        {
            Action<Action> work = self =>
                {
                    Console.WriteLine("Running");
                    self();
                };

            var token = scheduler.Schedule(work);

            Console.ReadKey();
            Console.WriteLine("Cancelling");
            token.Dispose();
        }

        private static void UseCancellationToken()
        {
            var ints = new List<int>();
            var cancelToken = Work(Scheduler.Default, ints);

            Task.Delay(2000).ContinueWith((_, token) => ((IDisposable)token).Dispose(), cancelToken);
        }

        public static IDisposable Work(IScheduler scheduler, List<int> list)
        {
            var tokenSource = new CancellationTokenSource();
            var cancelToken = tokenSource.Token;

            var task = new Task(() =>
                {
                    Console.WriteLine();
                    for (var i = 0; i < 1000; i++)
                    {
                        var sw = new SpinWait();
                        for (var j = 0; j < 3000; j++) sw.SpinOnce();
                        Console.Write(".");

                        list.Add(i);

                        if (cancelToken.IsCancellationRequested)
                        {
                            Console.WriteLine("Cancelation requested");
                            //cancelToken.ThrowIfCancellationRequested();
                            return;
                        }
                    }

                });
            task.Start();

            return Disposable.Create(tokenSource.Cancel);
        }

        private static void Cancellation(IScheduler scheduler)
        {
            Console.WriteLine("started at {0}", DateTime.Now.ToLongTimeString());

            var cancellation = scheduler.Schedule(() => Console.WriteLine(DateTime.Now.ToLongTimeString()));

            cancellation.Dispose();
        }

        private static void PassTheState(IScheduler scheduler)
        {
            const string name = "Lee";

            scheduler.Schedule(name, (_, state) =>
                {
                    Console.WriteLine(state);
                    return Disposable.Empty;
                });

            var list = new List<int>();

            scheduler.Schedule(list, (innerScheduler, state) =>
                {
                    Console.WriteLine(state.Count);

                    return Disposable.Empty;
                });

            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(3);
            list.Add(3);
            list.Add(3);
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
