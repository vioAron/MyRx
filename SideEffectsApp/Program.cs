using System;
using System.Reactive.Linq;

namespace SideEffectsApp
{
    class Program
    {
        static void Main()
        {
            SideEffect();
            CorrectedSideEffect();
            Do();
            Console.ReadKey();
        }

        private static void Log(object onNextValue)
        {
            Console.WriteLine("Logging OnNext{0} at {1}", onNextValue, DateTime.Now);
        }

        private static void Log(Exception onErrorValue)
        {
            Console.WriteLine("Logging OnError{0} at {1}", onErrorValue, DateTime.Now);
        }

        private static void Log()
        {
            Console.WriteLine("Logging OnCompleted at {0}", DateTime.Now);
        }

        private static void Do()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(3);

            var result = source.Do(i => Log(i), ex => Log(ex), () => Log()).
                Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
        }

        private static void CorrectedSideEffect()
        {
            var source = Observable.Range(0, 3);
            var result = source.Select((idx, value) => new
            {
                Index = idx,
                Letter = (char)(value + 65)
            });

            result.Subscribe(c => Console.WriteLine("Received {0} at index {1}", c.Letter, c.Index),
                () => Console.WriteLine("Completed"));

            result.Subscribe(c => Console.WriteLine("Also Received {0} at index {1}", c.Letter, c.Index),
                () => Console.WriteLine("2nd Completed"));
        }

        private static void SideEffect()
        {
            var letters = Observable.Range(0, 3).Select(i => (char)(i + 65));
            var index = -1;
            var result = letters.Select(c =>
            {
                index++;
                return c;
            });

            result.Subscribe(c => Console.WriteLine("Received {0} at index {1}", c, index),
                () => Console.WriteLine("Completed"));

            result.Subscribe(c => Console.WriteLine("Also Received {0} at index {1}", c, index),
                () => Console.WriteLine("2nd Completed"));
        }
    }
}
