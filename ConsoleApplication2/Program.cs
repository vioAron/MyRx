using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    public class Program
    {
        static void Main()
        {
            //var observable = new MyObservable();
            //var observer = new MyObserver();

            //observable.Subscribe();

            UseMyCache();

            Task.Delay(TimeSpan.FromMinutes(1)).Wait();

            var index = 0;

            var hot = MyRepository.Instance.StringsObservable.Publish();

            hot.Connect();

            var observable = hot.Replay();

            observable.Connect();

            observable.Subscribe(s => Console.WriteLine("First => {0}", s));

            Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Subscribe(t =>
            {
                MyRepository.Instance.Add(index++.ToString());
            });

            Thread.Sleep(TimeSpan.FromSeconds(5));
            
            observable.Subscribe(s => Console.WriteLine("Second => {0}", s));
            
            Task.Delay(TimeSpan.FromMinutes(5)).Wait();

            Console.WriteLine("Press any key!");
            Console.ReadKey();
        }

        public static void UseMyCache()
        {
            int index = 11;
            Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1)).Subscribe(n =>
            {
                MyRepository.Instance.Add(index++.ToString());
            });
            var connection1 = MyRepository.Instance.StringsObservable.Subscribe(s => Console.WriteLine("First con {0}", s));
            var connection2 = MyRepository.Instance.StringsObservable.Subscribe(s => Console.WriteLine("Second con {0}", s));

            Task.Delay(TimeSpan.FromSeconds(6)).Wait();
            connection1.Dispose();
            connection2.Dispose();
        }
    }
}
