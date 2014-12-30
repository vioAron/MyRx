using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Controls;

namespace SearchApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            IObservable<EventPattern<TextChangedEventArgs>> textChangedObservable = Observable.FromEventPattern<TextChangedEventHandler, TextChangedEventArgs>(
                ev => SearchTextBox.TextChanged += ev, ev => SearchTextBox.TextChanged -= ev);

            textChangedObservable.Subscribe(args => SubscribeTextBlock.Text += Environment.NewLine + SearchTextBox.Text);
        }
    }
}
