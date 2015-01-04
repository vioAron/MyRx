using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

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
                var index = 0;
                _strings.ForEach(s =>
                {
                    if (!s.Contains(searchText)) return;
                    index += 5;
                    Task.Delay(TimeSpan.FromSeconds(index)).ContinueWith(t => observer.OnNext(s));
                });

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
                    resultObservable.TakeUntil(textChangedObservable).ObserveOn(_uiScheduler).Subscribe(result =>
                    {
                        SubscribeTextBlock.Text += Environment.NewLine +
                                               string.Format("{0} - {1}", DateTime.Now.ToLongTimeString(),
                                                   result);
                    });
                });
        }
    }
}
