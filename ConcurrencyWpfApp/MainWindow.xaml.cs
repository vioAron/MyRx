using System;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Windows;
using ConcurrencyWpfApp.Annotations;

namespace ConcurrencyWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string _value;
        private readonly Subject<string> _subject = new Subject<string>();
        
        public MainWindow()
        {
            InitializeComponent();

            MyButton.Click += MyButton_Click;
            DataContext = this;

            Value = "default value";

            Value = _subject.First();
            _subject.Take(1).Subscribe(value => Value = value);
        }

        void MyButton_Click(object sender, RoutedEventArgs e)
        {
            _subject.OnNext("New value");
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                var handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
