using System;
using System.Reactive.Linq;

namespace LeavingTheMonadApp
{
    class Program
    {
        static void Main()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);

            source.ForEach(i => Console.WriteLine("received {0} at {1}", i, DateTime.Now));

            Console.WriteLine("completed at {0}", DateTime.Now);

            var result = source.ToEnumerable();

            foreach (var l in result)
            {
                Console.WriteLine(l);
            }

            Console.WriteLine("done");

            var array = source.ToArray();

            array.Subscribe(arr =>
            {
                Console.WriteLine("Received the array");

                foreach (var l in arr)
                {
                    Console.WriteLine(l);
                }
            },() => Console.WriteLine("Completed"));
            
            Console.WriteLine("Subscribed to array");

            Console.ReadKey();
        }
    }
}
