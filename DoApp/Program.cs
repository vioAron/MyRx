using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace DoApp
{
    class Program
    {
        static void Main()
        {
            var observable = Observable.Create<int>(observer =>
                {
                    observer.OnNext(1);
                    observer.OnNext(2);
                    observer.OnNext(3);
                    observer.OnCompleted();
                    return Disposable.Create(() => Console.WriteLine("observer has been unsubscribed!"));
                });

            observable.Do(n => Console.WriteLine("inside do -> {0}", n)).Subscribe(Console.WriteLine);

            Task.Delay(TimeSpan.FromSeconds(10)).Wait();
        }
    }
}
