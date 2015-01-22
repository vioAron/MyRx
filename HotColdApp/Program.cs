using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace HotColdApp
{
    class Program
    {
        static void Main()
        {
            //Cold();

            //SimpleCold();

            //ShareACold();

            //RefCount();

            //PublishLast();

            //Replay();

            Multicast();

            Console.ReadKey();
        }

        private static void Multicast()
        {
            var period = TimeSpan.FromSeconds(1);

            //var observable = Observable.Interval(period).Publish();

            var observable = Observable.Interval(period);
            var shared = new Subject<long>();
            shared.Subscribe(i => Console.WriteLine("first subscription : {0}", i));
            observable.Subscribe(shared);   //'Connect' the observable.
            Thread.Sleep(period);
            Thread.Sleep(period);
            Thread.Sleep(period);
            shared.Subscribe(i => Console.WriteLine("second subscription : {0}", i));
        }

        private static void Replay()
        {
            var hot = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5).Do(l => Console.WriteLine("publishing {0}", l)).Publish();

            hot.Connect();

            var replay = hot.Replay();

            replay.Connect();

            var first = replay.Subscribe(i => Console.WriteLine("first subscription : {0}", i));

            var second = replay.Subscribe(i => Console.WriteLine("second subscription : {0}", i));

            Thread.Sleep(TimeSpan.FromSeconds(6));

            var third = replay.Subscribe(i => Console.WriteLine("third subscription : {0}", i));
        }

        private static void PublishLast()
        {
            var observable = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5).Do(l => Console.WriteLine("publishing {0}", l)).PublishLast();

            observable.Connect();

            var first = observable.Subscribe(i => Console.WriteLine("first subscription : {0}", i));

            Thread.Sleep(TimeSpan.FromSeconds(5));

            Console.WriteLine("dispose first!");

            first.Dispose();
        }

        private static void RefCount()
        {
            var observable = Observable.Interval(TimeSpan.FromSeconds(1)).Do(l => Console.WriteLine("publishing {0}", l)).Publish().RefCount();

            var first = observable.Subscribe(i => Console.WriteLine("first subscription : {0}", i));

            Thread.Sleep(TimeSpan.FromSeconds(5));

            Console.WriteLine("dispose first!");

            first.Dispose();
        }

        private static void ShareACold()
        {
            var observable = Observable.Interval(TimeSpan.FromSeconds(1)).Do(l => Console.WriteLine("publishing {0}", l)).Publish();

            var first = observable.Subscribe(i => Console.WriteLine("first subscription : {0}", i));

            var connection = observable.Connect();

            Thread.Sleep(TimeSpan.FromSeconds(5));

            //var second = observable.Subscribe(i => Console.WriteLine("second subscription : {0}", i));

            Thread.Sleep(TimeSpan.FromSeconds(3));

            Console.WriteLine("dispose first!");
            first.Dispose();

            //second.Dispose();
            //connection.Dispose();
        }

        private static void SimpleCold()
        {
            var observable = Observable.Interval(TimeSpan.FromSeconds(1));

            observable.Subscribe(i => Console.WriteLine("first subscription : {0}", i));

            Thread.Sleep(TimeSpan.FromSeconds(10));

            observable.Subscribe(i => Console.WriteLine("second subscription : {0}", i));
        }

        private static void Cold()
        {
            var threeProducts = GetProducts().Take(3);

            threeProducts.Subscribe(p => Console.WriteLine("consumed {0}", p));
        }

        private static IObservable<string> GetProducts()
        {
            return Observable.Create<string>(obs =>
                {
                    for (var i = 0; i < 10; i++)
                    {
                        Console.WriteLine("published on next {0}", i);
                        obs.OnNext(i.ToString());
                    }

                    obs.OnCompleted();

                    return Disposable.Empty;
                });
        }
    }
}
