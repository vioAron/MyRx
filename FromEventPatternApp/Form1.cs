using System;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace FromEventPatternApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Observable.FromEventPattern<MouseEventHandler, MouseEventArgs>(ev => comboBox1.MouseWheel += ev, ev => comboBox1.MouseWheel -= ev).Subscribe(args => Console.WriteLine(args.EventArgs.Delta));
        }
    }
}
