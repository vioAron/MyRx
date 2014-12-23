using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace ObserveOnSubscribeOnApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var observable = Observable.Create<string>(observer =>
                {
                    observer.OnNext("item1");
                    observer.OnNext("item2");
                    observer.OnNext("item3");
                    observer.OnNext("item4");

                    observer.OnCompleted();
                    return Disposable.Empty;
                });

            observable.ObserveOn(new SynchronizationContextScheduler(WindowsFormsSynchronizationContext.Current)).SubscribeOn(Scheduler.Default).Subscribe(i => listView1.Items.Add(i));
        }
    }
}
