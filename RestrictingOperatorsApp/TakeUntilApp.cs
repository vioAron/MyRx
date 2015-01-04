using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace RestrictingOperatorsApp
{
    class TakeUntilApp
    {
        public static void SkipUntil1()
        {
            Console.WriteLine("SkipUntil...");

            var subject = new Subject<int>();
            var otherSubject = new Subject<Unit>();

            subject.SkipUntil(otherSubject).Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));

            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            otherSubject.OnNext(Unit.Default);
            subject.OnNext(4);
            subject.OnNext(5);
            subject.OnNext(6);
            subject.OnNext(7);
            subject.OnNext(8);
            subject.OnCompleted();
        }

        public static void TakeUntil1()
        {
            Console.WriteLine("TakeUntil...");

            var subject = new Subject<int>();
            var otherSubject = new Subject<Unit>();

            subject.TakeUntil(otherSubject).Subscribe(Console.WriteLine, () => Console.WriteLine("Completed"));

            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            otherSubject.OnNext(Unit.Default);
            subject.OnNext(4);
            subject.OnNext(5);
            subject.OnNext(6);
            subject.OnNext(7);
            subject.OnNext(8);
            subject.OnCompleted();
        }
    }
}