using System;
using System.Reactive.Linq;

namespace TimeShiftedSeqApp
{
    class Program
    {
        static void Main()
        {
            //Buffer();

            //Delay();

            //Sample();

            Timeout();

            Console.ReadKey();
        }

        private static void Timeout()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(100)).Take(10)
                                   .Concat(Observable.Interval(TimeSpan.FromSeconds(2)));
            
            var timeout = source.Timeout(TimeSpan.FromSeconds(1));

            timeout.Subscribe(Console.WriteLine, Console.WriteLine, () => Console.WriteLine("Completed"));
        }

        private static void Sample()
        {
            var source = Observable.Interval(TimeSpan.FromMilliseconds(150));

            var onePerSecond = source.Sample(TimeSpan.FromSeconds(1));

            Console.WriteLine("~ started at {0}", DateTime.Now.TimeOfDay);

            onePerSecond.Subscribe(l => Console.WriteLine("value = {0} at {1}", l, DateTime.Now.TimeOfDay));
        }

        private static void Delay()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1));
            var delayed = source.Delay(TimeSpan.FromSeconds(10));

            Console.WriteLine("~ started at {0}", DateTime.Now.TimeOfDay);

            delayed.Subscribe(l => Console.WriteLine(DateTime.Now.TimeOfDay));
        }

        private static void Buffer()
        {
            var seq1 = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);
            var seq2 = Observable.Interval(TimeSpan.FromMilliseconds(200)).Take(30);

            seq1.Concat(seq2).Buffer(TimeSpan.FromSeconds(10), 5).Subscribe(buffer =>
                {
                    Console.WriteLine("Buffered:");

                    foreach (var l in buffer)
                    {
                        Console.WriteLine(l);
                    }
                });

            seq1.Buffer(3, 5).Subscribe(buffer =>
                {
                    Console.WriteLine("Buffered:");
                    foreach (var l in buffer)
                    {
                        Console.WriteLine(l);
                    }
                }, () => Console.WriteLine("Completed"));
        }
    }
}
