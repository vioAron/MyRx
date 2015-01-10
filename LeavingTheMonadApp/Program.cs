using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;

namespace LeavingTheMonadApp
{
    class Program
    {
        private static readonly IObservable<long> Source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);

        static void Main()
        {
            Source.ForEach(i => Console.WriteLine("received {0} at {1}", i, DateTime.Now));

            Console.WriteLine("completed at {0}", DateTime.Now);

            var result = Source.ToEnumerable();

            foreach (var l in result)
            {
                Console.WriteLine(l);
            }

            Console.WriteLine("done");

            var array = Source.ToArray();

            array.Subscribe(arr =>
            {
                Console.WriteLine("Received the array");

                foreach (var l in arr)
                {
                    Console.WriteLine(l);
                }
            }, () => Console.WriteLine("Completed"));

            Console.WriteLine("Subscribed to array");

            ToTask();

            Console.ReadKey();
        }

        private static void ToTask()
        {
            var task = Source.ToTask();

            Console.WriteLine("subscribed to task");

            Console.WriteLine("task result => {0}", task.Result);

            var sourceWithError = Observable.Throw<long>(new Exception("Fail!"));

            var task2 = sourceWithError.ToTask();

            try
            {
                Console.WriteLine(task2.Result);
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.InnerException.Message);
            }

            ToEvent();
        }

        private static void ToEvent()
        {
            Console.WriteLine("ToEvent");
            IEventSource<long> @event = Source.ToEvent();

            @event.OnNext += val => Console.WriteLine("ToEvent {0}", val);

            Console.WriteLine("ToEventPattern");
            var source = Observable.Interval(TimeSpan.FromSeconds(1)).
                Select(i => new EventPattern<MyEventArgs>(null, new MyEventArgs(i)));

            var result = source.ToEventPattern();

            result.OnNext += (sender, eventArgs) => Console.WriteLine("ToEventPattern {0}", eventArgs.Value);
        }
    }

    public class MyEventArgs : EventArgs
    {
        private readonly long _value;

        public MyEventArgs(long value)
        {
            _value = value;
        }

        public long Value
        {
            get { return _value; }
        }
    }
}
