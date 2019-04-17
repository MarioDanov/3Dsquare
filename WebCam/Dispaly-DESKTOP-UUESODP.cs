using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Brush mBackgroundBrush = new SolidBrush(Color.White);
        private Brush mDotBrush = new SolidBrush(Color.Black);
        private Pen mLinePen = new Pen(Color.Red, 10);
        private Bitmap Image;
        private Graphics ImageGraphics;

        public Dispaly()
        {
            mLinePen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, DashCap.Flat);
            Image = new Bitmap(800, 800, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            ImageGraphics = Graphics.FromImage(Image);
            ImageGraphics.FillRectangle(mBackgroundBrush, 0, 0, Image.Width, Image.Height);
            ImageGraphics.Flush();

            funcs.Add(new GfSqr());
        }



        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Image, 0, 0);
        }

        int startX;
        int starty;
        double CoefOfEquation, FreeCoefOfEquation;
        int br = 0;
        DateTime currentTime = (DateTime.Now);
        double currX, currY, summX = 0, summY = 0, sqXs, XmpY, summSq;
        Point p1 = new Point(), p2 = new Point();
        Point pp_3;
        Line line_3;
        List<Point> list_V = new List<Point>();

        List<Gf> funcs = new List<Gf>();

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == 0) return;
            Point p = new Point();
            p.X = e.X;
            p.Y = e.Y;
            list_V.Add(p);


            Gf bestF = funcs[0];
            List<double> bestC = null;
            foreach (Gf func in funcs)
            {
                List<double> c = LSM.CalcLSM(func, list_V);
                bestC = c;
            }

            if (bestC == null) return;



            float r = 5;
            ImageGraphics.FillRectangle(mBackgroundBrush, 0, 0, 800, 800);
            foreach (Point point in list_V)
            {
                ImageGraphics.FillEllipse(mDotBrush, (float)point.X - r, (float)point.Y - r, 2 * r, 2 * r);
            }

            var n = p[0].;
            for (int i = 0; i < 800; i += 10)
            {

                var m = n + 10;

                double leftY = bestF.CalcAll(bestC, n);
                double rightY = bestF.CalcAll(bestC, m);
                ImageGraphics.DrawLine(mLinePen, n, (float)leftY, m, (float)rightY);
                n = m;
            }


            ImageGraphics.Flush();
            Invalidate();

            /*

                        if ((DateTime.Now - currentTime).TotalSeconds > 0.1)
                        {
                            float r = 5;
                            ImageGraphics.FillEllipse(mDotBrush, e.X - r, e.Y - r, 2 * r, 2 * r);
                            currX = e.X;
                            currY = e.Y;
                            summX += currX;
                            summY += currY;
                            sqXs += currX * currX;
                            XmpY += currX * currY;
                            if (br == 0)
                            {
                                p1.X = e.X;
                                p1.Y = e.Y;
                            }
                            br++;




                            CoefOfEquation = (XmpY * br - summX * summY) / ((br * sqXs) - (summX * summX));
                            FreeCoefOfEquation = (summY - CoefOfEquation * summX) / br;
                            currentTime = (DateTime.Now);
                            summSq += (CoefOfEquation * currX + (FreeCoefOfEquation - summY)) * (CoefOfEquation * summX + (FreeCoefOfEquation - summY));

                            Point p = new Point();
                            p.X = e.X;
                            p.Y = e.Y;
                            list_V.Add(p);
                            double sum = 0;
                            if (list_V.Count > 4)
                            {
                                foreach (Point cp in list_V) //Защо foreach а не For
                                {
                                    double diff = (p.Y - CoefOfEquation * p.X - FreeCoefOfEquation);
                                    sum += diff * diff;
                                }
                            }
                            Debug.WriteLine(sum);
                            if (sum > 100)
                            {
                                summX -= currX;
                                summY -= currY;
                                sqXs -= currX * currX;
                                XmpY -= currX * currY;
                                br--;
                                CoefOfEquation = (XmpY * br - summX * summY) / ((br * sqXs) - (summX * summX));
                                FreeCoefOfEquation = (summY - CoefOfEquation * summX) / br;
                                p2.X = list_V[list_V.Count - 2].X;
                                p2.Y = list_V[list_V.Count - 2].Y;
                                Line line = new Line();
                                line.A = CoefOfEquation;
                                line.B = -1;
                                line.C = FreeCoefOfEquation;
                                Point pp1;
                                Point pp2;
                                if (line_3 != null)
                                {
                                    pp1 = Line.Intersect(line_3, line);
                                    ImageGraphics.DrawLine(mLinePen, (float)pp_3.X, (float)pp_3.Y, (float)pp1.X, (float)pp1.Y);
                                }
                                else
                                {
                                    pp1 = Line.ProjectPoint(line, p1);
                                }
                                pp2 = Line.ProjectPoint(line, p2);
                                ImageGraphics.DrawLine(mLinePen, (float)pp1.X, (float)pp1.Y, (float)pp2.X, (float)pp2.Y);
                                pp_3 = pp2;
                                line_3 = line;
                                //ImageGraphics.DrawLine(mLinePen, 0, (float)FreeCoefOfEquation, 800, (float)(CoefOfEquation * 800 + FreeCoefOfEquation));
                                //draw 
                                currX = 0;
                                currY = 0;
                                summX = 0;
                                summY = 0;
                                sqXs = 0;
                                XmpY = 0;
                                br = 0;
                                CoefOfEquation = ((summY * summX) - br * (XmpY)) / ((br * sqXs) + (sqXs));
                                FreeCoefOfEquation = (summY - CoefOfEquation * summX) / br;

                                list_V.Clear();
                            }
                            ImageGraphics.Flush();
                            Invalidate();
                        }
                        */

            base.OnMouseMove(e);
        }


        public void Clear()
        {
            ImageGraphics.FillRectangle(mBackgroundBrush, 0, 0, Image.Width, Image.Height);
            ImageGraphics.Flush();
            Invalidate();
        }
    }
}
