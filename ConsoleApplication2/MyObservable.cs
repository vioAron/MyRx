using System;
using System.Globalization;
using System.Reactive.Disposables;

namespace ConsoleApplication2
{
    public class MyObservable : IObservable<string>
    {
        public IDisposable Subscribe(IObserver<string> observer)
        {
            for (var i = 0; i < 10; i++)
            {
                observer.OnNext(i.ToString(CultureInfo.InvariantCulture));
            }

            return Disposable.Empty;
        }
    }
}
