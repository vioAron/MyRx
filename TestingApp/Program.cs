using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using NUnit.Framework;

namespace TestingApp
{
    class Program
    {
        static void Main()
        {
            //PrintISchedulerParamMethods();
            //First();
            //ScheduleAndAdvance();
            //ScheduleAndStart();
            //ScheduleAndStartStop();
            SchedulerCollisions();

            Console.ReadKey();
        }

        private static void SchedulerCollisions()
        {
            var scheduler = new TestScheduler();

            scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("A"));
            scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("B"));
            scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("C"));

            scheduler.AdvanceTo(10);
        }

        private static void ScheduleAndAdvance()
        {
            var scheduler = new TestScheduler();

            scheduler.Schedule(() => Console.WriteLine("A"));

            scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("B"));
            scheduler.Schedule(TimeSpan.FromTicks(20), () => Console.WriteLine("C"));

            scheduler.AdvanceTo(1);
            scheduler.AdvanceTo(10);
            Console.WriteLine("To 15 ->");
            scheduler.AdvanceTo(15);
            Console.WriteLine("To 20 ->");
            scheduler.AdvanceTo(20);
        }

        private static void ScheduleAndStart()
        {
            var scheduler = new TestScheduler();

            scheduler.Schedule(() => Console.WriteLine("A"));

            scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("B"));
            scheduler.Schedule(TimeSpan.FromTicks(20), () => Console.WriteLine("C"));

            Console.WriteLine("Start");
            scheduler.Start();
            scheduler.Schedule(() => Console.WriteLine("D"));
            Console.WriteLine("scheduler.Clock:{0}", scheduler.Clock);
        }

        private static void ScheduleAndStartStop()
        {
            var scheduler = new TestScheduler();

            scheduler.Schedule(() => Console.WriteLine("A"));

            scheduler.Schedule(TimeSpan.FromTicks(10), () => Console.WriteLine("B"));
            scheduler.Schedule(TimeSpan.FromTicks(15), scheduler.Stop);
            scheduler.Schedule(TimeSpan.FromTicks(20), () => Console.WriteLine("C"));

            Console.WriteLine("Start");
            scheduler.Start();
            scheduler.Schedule(() => Console.WriteLine("D"));
            Console.WriteLine("scheduler.Clock:{0}", scheduler.Clock);
        }

        [Test]
        public static void First()
        {
            var scheduler = new TestScheduler();

            var wasExecuted = false;
            scheduler.Schedule(() => wasExecuted = true);

            Assert.False(wasExecuted);

            scheduler.AdvanceBy(1);

            Assert.IsTrue(wasExecuted);
        }

        private static void PrintISchedulerParamMethods()
        {
            var methods = from m in typeof(Observable).GetMethods()
                          from p in m.GetParameters()
                          where typeof(IScheduler).IsAssignableFrom(p.ParameterType)
                          group m by m.Name into method
                          orderby method.Key
                          select method.Key;

            foreach (var method in methods)
            {
                Console.WriteLine(method);
            }
        }
    }
}
