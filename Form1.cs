using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebCam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void mClearButton_Click(object sender, EventArgs e)
        {
            mDisplay.Clear();
        }

        private void mDisplay_Click(object sender, EventArgs e)
        {

        }
    }
}
