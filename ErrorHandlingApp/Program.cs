using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace ErrorHandlingApp
{
    class Program
    {
        static void Main()
        {
            //Swallow();

            //Catch();

            //Finally();

            Using();

            Console.ReadKey();
        }

        private static void Swallow()
        {
            var source = new Subject<int>();
            var result = source.Catch(Observable.Empty<int>());
            result.Subscribe(Console.WriteLine);

            source.OnNext(1);
            source.OnNext(2);
            source.OnError(new Exception("Fail!"));
        }

        private static void Catch()
        {
            var source = new Subject<int>();
            var result = source.Catch<int, TimeoutException>(n => Observable.Return(-1));
            result.Subscribe(Console.WriteLine);

            source.OnNext(1);
            source.OnNext(2);
            source.OnError(new TimeoutException("Fail!"));
        }

        private static void Finally()
        {
            var source = new Subject<int>();
            var result = source.Finally(() => Console.WriteLine("Finally!!!!"));
            result.Subscribe(Console.WriteLine);

            source.OnNext(1);
            source.OnNext(2);
            source.OnError(new Exception("Fail!"));
            source.OnCompleted();
            //source.OnError(new TimeoutException("Fail!"));
        }

        private static void Using()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1));
            var result = Observable.Using(() => new MyResource(), resource => source);

            result.Take(5).Subscribe(Console.WriteLine);
        }

        private class MyResource : IDisposable
        {
            public MyResource()
            {
                Console.WriteLine("MyResource created!");
            }

            public void Dispose()
            {
                Console.WriteLine("MyResource disposed!");
            }
        }
    }
}
