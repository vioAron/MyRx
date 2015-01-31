using System;
using System.Reactive.Linq;

namespace SequencesOfCoincidence
{
    class Program
    {
        public static void Main()
        {
            Window();

            Console.ReadKey();
        }

        private static void Window()
        {
            var ten = Observable.Interval(TimeSpan.FromSeconds(1)).Take(10);

            var windowCount = -1;
            ten.Window(3).Subscribe(window =>
            {
                windowCount++;
                window.Subscribe(value => Console.WriteLine("window{0}, value{1}", windowCount, value));
            });
        }
    }
}
