﻿using System;
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
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            closeTimer.Start();
        }

        private void closeTimer_Tick(object sender, EventArgs e)
        {
            closeTimer.Stop();
            Close();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {

        }
    }
}
