using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebCam
{
    public class Dispaly : Control
    {
        private Brush mBackgroundBrush = new SolidBrush(Color.White);
        private Brush mBackgroundBrush_Rec = new SolidBrush(Color.LightSteelBlue);
        private Brush mDotBrush = new SolidBrush(Color.Black);
        private Brush mDotBrush_True = new SolidBrush(Color.Blue);
        private Pen mLinePen = new Pen(Color.Red, 5);
        private Pen mButtonPen = new Pen(Color.Black, 2);
        private Pen mButtonPen2 = new Pen(Color.Blue, 2);
        private Font mButtonFont = new Font("Verdana", 15);
        public Bitmap Image;
        private Graphics ImageGraphics;
        public MainForm Form;
        public bool CircleMode = false;
        public bool ClearMode = false;
        Graphics g;
        public double selection_X;
        public double selection_Y;
        public double startingP_X;
        public double startingP_Y;
        public double endingP_X;
        public double endingP_Y;
        bool starting = false;

        public Dispaly()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            mLinePen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, DashCap.Flat);
            Image = new Bitmap(1350, 760, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            ImageGraphics = Graphics.FromImage(Image);
            ImageGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            ImageGraphics.FillRectangle(mBackgroundBrush, 0, 0, Image.Width, Image.Height);
            ImageGraphics.Flush();
            alg.AddFunctions();
        }

        private InterpolationAlgoritm alg = new InterpolationAlgoritm();


        private Rectangle mButtonCircle = new Rectangle();
        private Rectangle mButtonSelect = new Rectangle();
        private Rectangle mButtonClear = new Rectangle();
        private bool mButtonCircleState = false;
        private bool mButtonSelectState = false;
        private bool mButtonClearState = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(Image, 0, 0);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;


            mButtonSelect = new Rectangle(Width - 110, 120, 100, 100);
            if(mButtonSelectState == true)
                e.Graphics.DrawString("Избери", mButtonFont, mDotBrush_True, mButtonSelect, format);
            else
                e.Graphics.DrawString("Избери", mButtonFont, mDotBrush, mButtonSelect, format);
            e.Graphics.DrawRectangle(mButtonPen2, mButtonSelect);

            mButtonCircle = new Rectangle(Width - 110, 230, 100, 100);
            if (mButtonCircleState == true)
            {
                e.Graphics.DrawString("Кръгче", mButtonFont, mDotBrush_True, mButtonCircle, format);
                e.Graphics.DrawEllipse(mButtonPen2, mButtonCircle);

                CircleMode = true;
            }
            else
            {
                e.Graphics.DrawString("Кръгче", mButtonFont, mDotBrush, mButtonCircle, format);
                e.Graphics.DrawEllipse(mButtonPen2, mButtonCircle);
                CircleMode = false;
            }
            mButtonClear = new Rectangle(Width - 110, 10, 100, 100);
            if (mButtonClearState == true)
            {
                e.Graphics.FillRectangle(mBackgroundBrush_Rec, mButtonClear);
                e.Graphics.DrawString("Изтрий", mButtonFont, mDotBrush, mButtonClear, format);
                ClearMode = true;
            }
            else
            {
                e.Graphics.FillRectangle(mBackgroundBrush_Rec, mButtonClear);
                e.Graphics.DrawString("Изтрий", mButtonFont, mDotBrush, mButtonClear, format);
                ClearMode = false;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (mButtonClear.Contains(e.X, e.Y))
            {
                mButtonClearState = true;
                Form.PlayAudio("Try_again.wav");
                Clear();
            }
            else if (mButtonCircle.Contains(e.X, e.Y))
            {
                mButtonCircleState = !mButtonCircleState;
            }
            else if (mButtonSelect.Contains(e.X, e.Y))
            {
                mButtonSelectState = !mButtonSelectState;
                if(mButtonSelectState)
                {
                    if (alg.EndGesture(Form))
                    {
                        DrawResult(null);
                    }
                }
            }
            else if(mButtonSelectState)
            {
                Figure fig = null;
                Function func = null;
                double distanceSqr = double.MaxValue;
                bool startingLocal = false;
                foreach(Figure figs in alg.figures)
                {
                    foreach(Function f in figs.fig)
                    {
                        if ((f.StartingPoint.X - e.X) * (f.StartingPoint.X - e.X) + (f.StartingPoint.Y - e.Y) *(f.StartingPoint.Y - e.Y) < distanceSqr)
                        {
                            distanceSqr = (f.StartingPoint.X - e.X) * (f.StartingPoint.X - e.X) + (f.StartingPoint.Y - e.Y) * (f.StartingPoint.Y - e.Y);
                            fig = figs;
                            func = f;
                            startingLocal = true;
                        }
                        else if ((f.EndingPoint.X - e.X) * (f.EndingPoint.X - e.X) + (f.EndingPoint.Y - e.Y) * (f.EndingPoint.Y - e.Y) < distanceSqr)
                        {
                            distanceSqr = (f.EndingPoint.X - e.X) * (f.EndingPoint.X - e.X) + (f.EndingPoint.Y - e.Y) * (f.EndingPoint.Y - e.Y);
                            fig = figs;
                            func = f;
                            startingLocal = false;
                        }
                    }
                }
                selection_X = e.X;
                selection_Y = e.Y;
                if (distanceSqr < 100)
                {
                    startingP_X = func.StartingPoint.X;
                    startingP_Y = func.StartingPoint.Y;
                    endingP_X = func.EndingPoint.X;
                    endingP_Y = func.EndingPoint.Y;

                    double distance;
                    alg.selectedFigure = fig;
                    alg.selectedFunction = func;
                    alg.select_State = true;
                    starting = startingLocal;
                    if (starting)
                    {
                        alg.selectedStartingP = true;
                        distance = Math.Sqrt((func.StartingPoint.X - selection_X) * (func.StartingPoint.X - selection_X) + (func.StartingPoint.Y - selection_Y) * (func.StartingPoint.Y - selection_Y));

                    }
                    else
                    {
                        alg.selectedStartingP = false;
                        distance = Math.Sqrt((func.EndingPoint.X - selection_X) * (func.EndingPoint.X - selection_X) + (func.EndingPoint.Y - selection_Y) * (func.EndingPoint.Y - selection_Y));
                    }
                    DrawResult(null);
                    Invalidate();
                }
                else
                {

                }
            }
            Invalidate();
            base.OnMouseMove(e);
        }

        DateTime currentTime = (DateTime.Now);
        bool checkIfFirst = false;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            HandleMove(e.X, e.Y, (e.Button & MouseButtons.Left) != 0);
            base.OnMouseMove(e);
        }


        public void OnPointMove(int x, int y, int width, int height, bool buttonDown)
        {
            Debug.WriteLine(x + " " + y);
            x = (int)Math.Round((double)(x * Width) / width);
            y = (int)Math.Round((double)(y * Height) / height);
            HandleMove(x, y, buttonDown);
        }

        private int circleStage = 0;
        private bool hasCenter = false;
        private bool hasRadius = false;
        private int circleX;
        private int circleY;
        private float circleR;
        public void HandleMove(int x, int y, bool buttonDown)
        {
            if (mButtonSelectState)
            {
                if (buttonDown)
                {
                    if (alg.selectedFunction == null)
                        return;
                    Point oldPoint = new Point();
                    Point newPoint = new Point();
                    if (starting)
                    {
                        oldPoint.X = alg.selectedFunction.StartingPoint.X;
                        oldPoint.Y = alg.selectedFunction.StartingPoint.Y;
                        newPoint.X = startingP_X + (x - selection_X);
                        newPoint.Y = startingP_Y + (y - selection_Y);
                    }
                    else
                    {
                        oldPoint.X = alg.selectedFunction.EndingPoint.X;
                        oldPoint.Y = alg.selectedFunction.EndingPoint.Y;
                        newPoint.X = endingP_X + (x - selection_X);
                        newPoint.Y = endingP_Y + (y - selection_Y);
                    }
                    foreach(Figure fn in alg.figures)
                    {
                        foreach(Function func in fn.fig)
                        {
                            if(Point.Distance(oldPoint, func.StartingPoint) < 0.1)
                            {
                                func.StartingPoint = newPoint;
                            }
                            if (Point.Distance(oldPoint, func.EndingPoint) < 0.1)
                            {
                                func.EndingPoint = newPoint;
                            }
                        }
                    }
                    DrawResult(null);
                    Invalidate();
                }
                return;
            }
            if(CircleMode)
            {
                if (!buttonDown)
                {
                    if ((circleStage == 0) && hasCenter)
                    {
                        circleStage = 1;
                        circleR = 50;
                        DrawResult(null);
                    }
                    if ((circleStage == 1) && hasRadius)
                    {
                        circleStage = 0;
                        hasCenter = false;
                        hasRadius = false;
                        //circleX,circleY,circleR
                        Circle newcircle = new Circle();
                        newcircle.X = circleX;
                        newcircle.Y = circleY;
                        newcircle.R = circleR;
                        alg.FixCircle(newcircle);
                        alg.circles.Add(newcircle);
                        Form.ShowMassage("circle.wav", "Окръжност");
                        DrawResult(null);
                    }
                        return;
                }
                else
                {
                    if(circleStage == 0)
                    {
                        hasCenter = true;
                        circleX = x;
                        circleY = y;
                        DrawResult(null);
                    }
                    else if (circleStage == 1)
                    {
                        hasRadius = true;
                        circleR = (float)Math.Sqrt((x - circleX) * (x - circleX) + (y - circleY) * (y - circleY));
                        DrawResult(null);
                    }
                }
                return;
            }
            if (!buttonDown)
            {
                if (alg.EndGesture(Form))
                {
                    DrawResult(null);
                }
                return;
            }
            alg.AddPoint(new Point(x, y));
            Function lastFunc = null;
            if (alg.ExistingGraphics.Count >= 1)
                lastFunc = alg.ExistingGraphics[alg.ExistingGraphics.Count - 1].Clone();
            Function f = alg.InterpolatePoints();
            bool functionAdded = false;
            if (f != null)
            {
                functionAdded = alg.CheckFunction(f);
                if (!functionAdded && (alg.ExistingGraphics.Count >= 1))
                {
                    alg.LineJoin(alg.ExistingGraphics[alg.ExistingGraphics.Count - 1], f);
                }
            }

            DrawResult(f);

            if (!functionAdded && (lastFunc != null))
                alg.ExistingGraphics[alg.ExistingGraphics.Count - 1] = lastFunc;

        }

        private void DrawResult(Function f)
        {
            ImageGraphics.FillRectangle(mBackgroundBrush, 0, 0, Image.Width, Image.Height);
            alg.DrawPoints(ImageGraphics);
            if (f != null)
            {
                alg.DrawFunction(ImageGraphics, f.BestFunction, f.BestCoefficient, f.TransformStartingPoint, f.TransformEndingPoint, f.TransformInverse);
            }
            alg.DrawAllFunctions(ImageGraphics);

            if (CircleMode)
            {
                if (hasCenter)
                {
                    ImageGraphics.FillEllipse(mDotBrush, circleX - 5, circleY - 5, 10, 10);
                }
                if(circleStage == 1)
                {
                    ImageGraphics.DrawEllipse(mLinePen, circleX - circleR, circleY - circleR, 2 * circleR, 2 * circleR);
                }
            }

            ImageGraphics.Flush();
            Invalidate();
        }

        Matrix mat = Matrix.Invert(new Matrix());

        public void Clear()
        {
            alg.ClearAll();
            ImageGraphics.FillRectangle(mBackgroundBrush, 0, 0, Image.Width, Image.Height);
            Invalidate();      
        }

        public void ClearGesture()
        {
            alg.ClearGesture();
            circleStage = 0;
            hasCenter = false;
            hasRadius = false;
            DrawResult(null);
            Invalidate();
        }
    }
}
