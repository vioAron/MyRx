using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ObservableTimerApp
{
    class Program
    {
        static void Main()
        {
            //Form1 form1 = new Form1();
            ////form1.Show();
            //Application.Run(form1);
            //return;
            var myProcessor = new MyProcessor();

            myProcessor.Process();

            //Thread.Sleep(400);

            //myProcessor.Dispose();

            //Thread.Sleep(TimeSpan.FromMinutes(2));

            
            Console.ReadKey();
        }
    }

    public class MyProcessor : IDisposable
    {
        private volatile bool _isDisposed;

        private readonly SingleAssignmentDisposable _singleAssignmentDisposable = new SingleAssignmentDisposable();

        public void Process()
        {
            int index = 0;
            _singleAssignmentDisposable.Disposable = Observable.Timer(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(100)).Subscribe(n =>
                {
                    Console.WriteLine("step1 -> {0}", DateTime.Now.ToString("mm:ss:ffff"));
                    //step1
                    //Thread.Sleep(TimeSpan.FromSeconds(10));

                    Console.WriteLine("step2");
                }, ex => Console.WriteLine(ex.Message));
        }

        public void Dispose()
        {
            _isDisposed = true;
            _singleAssignmentDisposable.Dispose();
        }
    }
}
