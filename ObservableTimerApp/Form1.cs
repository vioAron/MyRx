using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ObservableTimerApp
{
    public partial class Form1 : Form
    {
        private IScheduler _scheduler;
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _scheduler = new SynchronizationContextScheduler(SynchronizationContext.Current);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            var index = 0;

            var s = new Stopwatch();
            s.Start();

            for (var i = 1; i <= 1000000; i++)
            {
                Observable.Timer(TimeSpan.FromSeconds(3), _scheduler)
                      .Subscribe(n =>
                          {
                              index++;
                              if (index == 1000000)
                              {
                                  s.Stop();
                                  label1.Text = s.ElapsedMilliseconds.ToString();
                              }
                          });
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var index = 0;

            var s = new Stopwatch();
            s.Start();

            for (var i = 1; i <= 1000000; i++)
            {
                Observable.Timer(TimeSpan.FromSeconds(3)).ObserveOn(_scheduler)
                      .Subscribe(n =>
                      {
                          index++;
                          if (index == 1000000)
                          {
                              s.Stop();
                              label2.Text = s.ElapsedMilliseconds.ToString();
                          }
                      });
            }
        }
    }
}
