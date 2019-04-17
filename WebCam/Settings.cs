using CameraLibrary;
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
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
           
        }

        protected override void OnLoad(EventArgs e)
        {
            MainForm.Worker.OpenWindow(cameraPanel.Handle.ToInt32());
            int ch=0, cs = 0, cv = 0, dch = 0, dcs = 0, dcv = 0;
            int bh = 0, bs = 0, bv = 0, dbh = 0, dbs = 0, dbv = 0;
            MainForm.Worker.GetConfig(
                ref ch, ref cs, ref cv, ref dch, ref dcs, ref dcv,
                ref bh, ref bs, ref bv, ref dbh, ref dbs, ref dbv);
            centerH.Value = ch;
            centerS.Value = cs;
            centerV.Value = cv;
            centerDH.Value = dch;
            centerDS.Value = dcs;
            centerDV.Value = dcv;
            hScrollBar6.Value = bh;
            hScrollBar5.Value = bs;
            hScrollBar4.Value = bv;
            hScrollBar9.Value = dbh;
            hScrollBar8.Value = dbs;
            hScrollBar7.Value = dbv;
            MainForm.Worker.SetColorCallback(ColorCallback);
            base.OnLoad(e);
        }

        private void ColorCallback(int h, int s, int v)
        {
            BeginInvoke(new CameraColorCallback(ColorCallbackInUI), h, s, v);
        }

        private void ColorCallbackInUI(int h, int s, int v)
        {
            string str = "h:" + h + " s:" + s + " v:" + v;
            textBoxColor.Text += str + "\r\n";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            MainForm.Worker.CloseWindow();
            base.OnClosing(e);
        }

        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            Color center = ColorConversion.ColorFromhsb(
                (float)(centerH.Value * 2.0),
                (float)(centerS.Value / 255.0),
                (float)(centerV.Value / 255.0));
            panelCenter.BackColor = center;
            labelCenterH.Text = centerH.Value.ToString();
            labelCenterS.Text = centerS.Value.ToString();
            labelCenterV.Text = centerV.Value.ToString();
            UpdateConfig();
        }

       private void Dif_ScrollBar_ValueChanged(object sender, EventArgs e)
        {
            labelcenterDH.Text = centerDH.Value.ToString();
            labelcenterDS.Text = centerDS.Value.ToString();
            labelcenterDV.Text = centerDV.Value.ToString();
            UpdateConfig();
        }
        private void RedScrollBar_ValueChanged(object sender, EventArgs e)
        {
            Color center = ColorConversion.ColorFromhsb(
                (float)(hScrollBar6.Value * 2.0),
                (float)(hScrollBar5.Value / 255.0),
                (float)(hScrollBar4.Value / 255.0));
            panel2.BackColor = center;
            label18.Text = hScrollBar6.Value.ToString();
            label19.Text = hScrollBar5.Value.ToString();
            label20.Text = hScrollBar4.Value.ToString();
            UpdateConfig();
        }
        private void Dif_RScrollBar_ValueChanged(object sender, EventArgs e)
        {
            label21.Text = hScrollBar9.Value.ToString();
            label22.Text = hScrollBar8.Value.ToString();
            label23.Text = hScrollBar7.Value.ToString();
            UpdateConfig();
        }

        private void UpdateConfig()
        {
            MainForm.Worker.Config(
                centerH.Value, centerS.Value, centerV.Value,
                centerDH.Value, centerDS.Value, centerDV.Value,
                hScrollBar6.Value, hScrollBar5.Value, hScrollBar4.Value,
                hScrollBar9.Value, hScrollBar8.Value, hScrollBar7.Value);
        }
    }
}
