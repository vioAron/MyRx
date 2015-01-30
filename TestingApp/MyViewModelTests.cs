using System;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Moq;
using NUnit.Framework;
using TestingApp.Core;
using TestingApp.ViewModel;

namespace TestingApp
{
    public class MyViewModelTests
    {
        [Test]
        public void Show_NoPriceUpdates_EmptyPrices()
        {
            var modelMock = new Mock<IMyModel>();
            var schedulers = new TestSchedulers();

            var viewModel = new MyViewModel(modelMock.Object, schedulers);

            var pricesSubject = new Subject<decimal>();

            modelMock.Setup(m => m.PriceStream(It.Is<string>(symbol => symbol == "AAPL"))).Returns(pricesSubject);

            viewModel.Show("AAPL");

            schedulers.ThreadPool.AdvanceTo(1);
            schedulers.Dispatcher.AdvanceTo(1);

            Assert.AreEqual(0, viewModel.Prices.Count);
        }

        [Test]
        public void Show_OnePriceUpdate_OnePrice()
        {
            var modelMock = new Mock<IMyModel>();
            var schedulers = new TestSchedulers();

            var viewModel = new MyViewModel(modelMock.Object, schedulers);

            var pricesSubject = new Subject<decimal>();

            modelMock.Setup(m => m.PriceStream(It.Is<string>(symbol => symbol == "AAPL"))).Returns(pricesSubject);

            viewModel.Show("AAPL");

            schedulers.ThreadPool.Schedule<object>(null, (_, a) =>
            {
                pricesSubject.OnNext(10);
                return Disposable.Empty;
            });

            schedulers.ThreadPool.AdvanceTo(1);
            schedulers.Dispatcher.AdvanceTo(1);

            Assert.AreEqual(1, viewModel.Prices.Count);
        }

        [Test]
        public void Show_NoPriceUpdatesIn11Seconds_Disconnected()
        {
            var modelMock = new Mock<IMyModel>();
            var schedulers = new TestSchedulers();

            var viewModel = new MyViewModel(modelMock.Object, schedulers);

            var pricesSubject = new Subject<decimal>();

            modelMock.Setup(m => m.PriceStream(It.Is<string>(symbol => symbol == "AAPL"))).Returns(pricesSubject);

            viewModel.Show("AAPL");

            schedulers.ThreadPool.AdvanceTo(TimeSpan.FromSeconds(11).Ticks);
            schedulers.Dispatcher.AdvanceTo(1);

            Assert.IsFalse(viewModel.IsConnected);
        }
    }
}
