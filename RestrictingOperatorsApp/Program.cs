using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RestrictingOperatorsApp
{
    class Program
    {
        static void Main()
        {
            Distinct();

            //Distinct2();

            Console.ReadKey();
        }

        private static void Distinct2()
        {
            var subject = new Subject<int>();

            subject.Distinct().Subscribe(Console.WriteLine);

            subject.OnNext(1);
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnCompleted();
        }

        private static void Distinct()
        {
            var observable = Observable.Create<string>(observer =>
            {
                observer.OnNext("first");
                observer.OnNext("first");
                observer.OnNext("first");
                observer.OnNext("second");
                observer.OnNext("third");

                return Disposable.Empty;
            });

            observable.Distinct().Subscribe(Console.WriteLine);
        }
    }
}
