using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Reactive.Testing;
using Xunit;

namespace MyRxTestsApp.Tests
{
    public class NumberGeneratorTests : IDisposable
    {
        public NumberGeneratorTests()
        {
            Console.WriteLine("We are currently in the constructor");
        }

        [Fact]
        public void GetGeneratedNumbers_WrongAndSlow()
        {
            var numberGenerator = new NumberGenerator();

            var numbersObservable = numberGenerator.GetGeneratedNumbers();

            var list = new List<long>();

            var subcription = numbersObservable.Subscribe(n =>
                    {
                        Console.WriteLine(n);
                        list.Add(n);
                    });

            Thread.Sleep(TimeSpan.FromSeconds(6));

            subcription.Dispose();

            Assert.Equal(new List<long> { 0, 1, 2, 3, 4 }, list);
            Assert.Equal(5, list.Count);
        }

        [Fact]
        public void GetGeneratedNumbers_Observable_Generate5Numbers()
        {
            var numberGenerator = new NumberGenerator();

            var testScheduler = new TestScheduler();
            var numbersObservable = numberGenerator.GetGeneratedNumbers(testScheduler);

            var list = new List<long>();

            var subcription = numbersObservable.Subscribe(n =>
            {
                Console.WriteLine(n);
                list.Add(n);
            }, ex => Console.WriteLine(ex.Message));

            testScheduler.AdvanceBy(TimeSpan.FromSeconds(6).Ticks);

            subcription.Dispose();

            Assert.Equal(new List<long> { 0, 1, 2, 3, 4 }, list);
            Assert.Equal(5, list.Count);
        }

        public void Dispose()
        {
            Console.WriteLine("We are currently in the dispose");
        }
    }
}
