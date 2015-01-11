using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDApp
{
    public partial class Form1 : Form
    {
        private IObservable<EventPattern<EventArgs>> _clickObservable;
 
        public Form1()
        {
            InitializeComponent();

            _clickObservable = Observable.FromEventPattern<EventHandler, EventArgs>(ev => button1.Click += ev, ev => button1.Click -= ev);

            _clickObservable.ObserveOn(Scheduler.Default).Do(e =>
            {
                Console.WriteLine("Do started @ {0}", DateTime.Now.TimeOfDay);
                Thread.Sleep(5000);
                Console.WriteLine("Do ended @ {0}", DateTime.Now.TimeOfDay);
            }).Subscribe(e =>
            {
                Console.WriteLine("Subscribe @ {0}", DateTime.Now.TimeOfDay);
            });
        }
    }
}
