using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;

namespace AggregationApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IScheduler _scheduler;

        public MainWindow()
        {
            InitializeComponent();

            _scheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);

            Loaded += MainWindow_Loaded;
        }

        private IObservable<long> _intervalObservable;

        private IDisposable _last;
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var rangeObservable = Observable.Range(1, 10);
            _intervalObservable = Observable.Interval(TimeSpan.FromSeconds(1));

            listView.Items.Add("Only after completed");
            rangeObservable.Count().Subscribe(count => listView.Items.Add(count.ToString()));
            rangeObservable.Min().Subscribe(count => listView.Items.Add(count.ToString()));
            rangeObservable.Max().Subscribe(count => listView.Items.Add(count.ToString()));
            rangeObservable.Average().Subscribe(count => listView.Items.Add(count.ToString()));
            rangeObservable.FirstAsync().Subscribe(count => listView.Items.Add(count.ToString()));
            rangeObservable.LastAsync().Subscribe(count => listView.Items.Add(count.ToString()));

            listView.Items.Add("Endless observable");
            _intervalObservable.FirstAsync().ObserveOn(_scheduler).Subscribe(count => listView.Items.Add(count.ToString()));
            _last = _intervalObservable.LastAsync().ObserveOn(_scheduler).Subscribe(count => listView.Items.Add(count.ToString()));
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _last.Dispose();
        }
    }
}
