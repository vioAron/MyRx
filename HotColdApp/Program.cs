using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
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

            RefCount();

            Console.ReadKey();
        }

        private static void RefCount()
        {
            throw new NotImplementedException();
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
