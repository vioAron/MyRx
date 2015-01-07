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

            Console.ReadKey();
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
