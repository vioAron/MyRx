using System;

namespace TestingApp.ViewModel
{
    public interface IMyModel
    {
        IObservable<decimal> PriceStream(string symbol);
    }
}