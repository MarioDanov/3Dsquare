using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebCam
{
    class Dispaly : Control
    {
        private Brush mBackgroundBrush = new SolidBrush(Color.Red);
        private Brush mDotBrush = new SolidBrush(Color.Blue);
        private Pen mLinePen = new Pen(Color.Blue, 10);
        private Pen mGreenPen = new Pen(Color.Green, 10);
        private bool Valid = false;
        private int DotX = 0;
        private int DotY = 0;
        private Bitmap Image;
        private Graphics ImageGraphics;

        public Dispaly()
        {
            mLinePen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, DashCap.Flat);
            Image = new Bitmap(800, 800, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            ImageGraphics = Graphics.FromImage(Image);
            ImageGraphics.FillRectangle(mBackgroundBrush, 0, 0, Image.Width, Image.Height);
            ImageGraphics.Flush();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Image, 0, 0);
        }

        int startX;
        int starty;
        DateTime lastmove;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Valid)
            {
                ImageGraphics.DrawLine(mLinePen, DotX, DotY, e.X, e.Y);
                if ((DateTime.Now - lastmove).TotalSeconds > 0.5)
                {
                    ImageGraphics.DrawLine(mGreenPen, startX, starty, DotX, DotY);
                    startX = e.X;
                    starty = e.Y;
                }
            }
            else
            {
                startX = e.X;
                starty = e.Y;
            }
            lastmove = DateTime.Now;
            DotX = e.X;
            DotY = e.Y;
            Valid = true;
//            float r = 5;
//            ImageGraphics.FillEllipse(mDotBrush, DotX - r, DotY - r, 2 * r, 2 * r);
            ImageGraphics.Flush();

            base.OnMouseMove(e);
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Valid = false;
            base.OnMouseLeave(e);
        }

        public void Clear()
        {
            ImageGraphics.FillRectangle(mBackgroundBrush, 0, 0, Image.Width, Image.Height);
            ImageGraphics.Flush();
            Invalidate();
        }
    }
}
