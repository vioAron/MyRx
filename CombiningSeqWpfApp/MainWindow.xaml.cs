using System;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;

namespace CombiningSeqWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var source = Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>
                (ev => MouseMove += ev, ev => MouseMove -= ev).Select(ep =>
                    new Coord
                    {
                        X = ep.EventArgs.GetPosition(null).X,
                        Y = ep.EventArgs.GetPosition(null).Y
                    });

            var delayedMouse = source.Skip(1);
            source.Zip(delayedMouse, (ep1, ep2) => new Coord
            {
                X = ep1.X - ep2.X,
                Y = ep1.Y - ep2.Y
            }).Subscribe(c =>
            {
                if (Math.Abs(c.X) > 3)
                    MovementXTextBlock.Text = c.X > 0 ? "Left" : "Right";
                if (Math.Abs(c.Y) > 3)
                    MovementYTextBlock.Text = c.Y > 0 ? "Up" : "Down";
            });
        }
    }

    public class Coord
    {
        public double X { get; set; }
        public double Y { get; set; }
        public override string ToString()
        {
            return string.Format("{0},{1}", X, Y);
        }
    }
}
