using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Documents;

namespace SearchApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IScheduler _uiScheduler;

        private static readonly List<string> _strings = new List<string> { "aaaaaa", "bbbbbbbbb", "cccccccc", "aaaaabbbb", "aaaaacccccc" };

        private static IObservable<string> GetSearchResult(string searchText)
        {
            return Observable.Create<string>(observer =>
            {
                _strings.ForEach(s =>
                {
                    if (s.Contains(searchText))
                    {
                        Thread.Sleep(1000);
                        observer.OnNext(s);
                    }
                });

                observer.OnCompleted();

                return Disposable.Empty;
            });
        }

        public MainWindow()
        {
            InitializeComponent();

            _uiScheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);

            var textChangedObservable = Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
                ev => SearchTextBox.TextChanged += ev, ev => SearchTextBox.TextChanged -= ev);

            textChangedObservable.Select(ep => ((TextBox)ep.Sender).Text).Throttle(TimeSpan.FromSeconds(5)).DistinctUntilChanged().Subscribe(
                args =>
                {
                    var resultObservable = GetSearchResult(args);
                    resultObservable.Buffer(2).ObserveOn(_uiScheduler).Subscribe(results =>
                    {
                        foreach (var result in results)
                        {
                            SubscribeTextBlock.Text += Environment.NewLine +
                                                   string.Format("{0} - {1}", DateTime.Now.ToLongTimeString(),
                                                       result);
                        }
                    });
                });
        }
    }
}
