using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CombiningSeqApp
{
    class Program
    {
        static void Main()
        {
            //Concat();

            //Repeat();

            //StartWith();

            Amb();

            Console.ReadKey();
        }

        private static void Amb()
        {
            var s1 = new Subject<int>();
            var s2 = new Subject<int>();
            var s3 = new Subject<int>();
            
            var result = Observable.Amb(s1, s2, s3);
            
            result.Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));

            s1.OnNext(1);
            s2.OnNext(2);
            s3.OnNext(3);
            s1.OnNext(1);
            s2.OnNext(2);
            s3.OnNext(3);

            s1.OnCompleted();
            s2.OnCompleted();
            s3.OnCompleted();
        }

        private static void StartWith()
        {
            var s = Observable.Range(0, 3);

            s.StartWith(-3, -2, -1).Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
        }

        private static void Repeat()
        {
            var s = Observable.Range(0, 3);

            s.Repeat(3).Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));
        }

        private static void Concat()
        {
            var s1 = Observable.Range(0, 3);
            var s2 = Observable.Range(5, 5);

            s1.Concat(s2).Subscribe(Console.WriteLine);
        }
    }
}
