using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingApp.Core;

namespace TestingApp.ViewModel
{
    public class MyViewModel
    {
        private readonly IMyModel _myModel;
        private readonly TestSchedulers _schedulers;
        private readonly ObservableCollection<decimal> _prices;

        public MyViewModel(IMyModel myModel, TestSchedulers schedulers)
        {
            _myModel = myModel;
            _schedulers = schedulers;
            _prices = new ObservableCollection<decimal>();
        }

        public ObservableCollection<decimal> Prices
        {
            get { return _prices; }
        }

        public void Show(string symbol)
        {
            //TODO: resource mgt, exception handling etc...
            _myModel.PriceStream(symbol)
            .SubscribeOn(_schedulers.ThreadPool)
            .ObserveOn(_schedulers.Dispatcher)
            .Timeout(TimeSpan.FromSeconds(10), _schedulers.ThreadPool)
            .Subscribe(price =>
            
            Prices.Add(price),
            
            ex =>
            {
                if (ex is TimeoutException)
                {
                    IsConnected = false;
                }
            });
            IsConnected = true;
        }

        public bool IsConnected { get; set; }
    }
}
