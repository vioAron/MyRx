using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FromEventPatternApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MouseClick +=Form1_MouseClick;
            Observable.FromEventPattern<MouseEventArgs>(ev => MouseClick += ev, ev => MouseClick -= ev)
        }

        void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
