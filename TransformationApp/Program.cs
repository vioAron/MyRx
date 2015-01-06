using System;
using System.Reactive.Linq;

namespace TransformationApp
{
    class Program
    {
        static void Main()
        {
            var source = Observable.Range(1, 5);

            source.Select(n => n + 3).Subscribe(Console.WriteLine);

            source.Select(n => new { Number = n, Description = string.Format("n={0}", n) })
                .Subscribe(Console.WriteLine);

            Observable.Interval(TimeSpan.FromSeconds(1)).Take(3).Timestamp().Subscribe(t => Console.WriteLine(t));
            Observable.Interval(TimeSpan.FromSeconds(1)).Take(3).TimeInterval().Subscribe(t => Console.WriteLine(t));

            Observable.Interval(TimeSpan.FromSeconds(1)).Take(3).Materialize().Subscribe(Console.WriteLine);

            Console.ReadKey();
        }
    }
}
