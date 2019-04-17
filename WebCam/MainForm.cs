using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CameraLibrary;
using System.IO;
using System.Reflection;

namespace WebCam
{
    public partial class MainForm : Form
    {
        public static CameraWorker Worker;
        private Dispaly mDisplay;

        private void mouseCallback(int x, int y, int width, int height, bool available)
        {
            BeginInvoke(new CameraCallback(mouseCallbackInGUI), x, y, width, height, available);
        }

        private void mouseCallbackInGUI(int x, int y, int width, int height, bool available)
        {
            mDisplay.OnPointMove(x, y, width, height, available);
        }

        public MainForm()
        {
            InitializeComponent();
            mDisplay = new Dispaly();
            Controls.Add(mDisplay);
            mDisplay.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            mDisplay.Left = 10;
            mDisplay.Width = Width - 20;
            mDisplay.Top = 30;
            mDisplay.Height = Height - 40;
            mDisplay.Form = this;
        }

        protected override void OnLoad(EventArgs e)
        {
            Worker = new CameraWorker();
            Worker.StartCamera(new CameraCallback(mouseCallback));
            base.OnLoad(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Worker.StopCamera();
            base.OnClosing(e);
        }

        private void mClearButton_Click(object sender, EventArgs e)
        {
            PlayAudio("Try_again.wav");
            mDisplay.Clear();
        }

        private void mDisplay_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayAudio("Save.wav");
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".png";
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                mDisplay.Image.Save(dialog.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (mDisplay.CircleMode)
            {
                mDisplay.CircleMode = false;
                mDisplay.ClearGesture();
                button2.BackColor = Color.FromKnownColor(KnownColor.Control);
            }
            else
            {
                mDisplay.CircleMode = true;
                mDisplay.ClearGesture();
                button2.BackColor = Color.LightBlue;
            }
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            (new helpDialog()).ShowDialog();
        }

        public void ShowMassage(string audioName, string mesageText)
        {
            if (audioName != null)
            {
                PlayAudio(audioName);
            }

            message.Text = mesageText;
            message.Visible = true;
            messageTimer.Start();
        }

        public void PlayAudio(string audioName)
        {
            string directory = typeof(MainForm).Assembly.CodeBase;
            directory = Path.GetDirectoryName(directory);
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(directory + @"\" + audioName);
            player.Play();
        }

        private void messageTimer_Tick(object sender, EventArgs e)
        {
            messageTimer.Stop();
            message.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            (new helpDialog()).ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string directory = typeof(MainForm).Assembly.CodeBase;
            directory = Path.GetDirectoryName(directory);
            System.Diagnostics.Process.Start(directory + @"\Marker.pdf");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            (new Settings()).ShowDialog();
        }
    }
}
