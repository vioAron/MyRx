using System;
using System.Reactive.Linq;

namespace TimeShiftedSeqApp
{
    class Program
    {
        static void Main()
        {
            Buffer();

            Console.ReadKey();
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
