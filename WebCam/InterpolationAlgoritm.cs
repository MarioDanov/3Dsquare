using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebCam
{
    class InterpolationAlgoritm
    {
        public List<Point> mPointList = new List<Point>();
        List<Function> mFunctions = new List<Function>();
        public List<Function> ExistingGraphics = new List<Function>();
        private Brush mDotBrush = new SolidBrush(Color.Black);
        private Brush mDotBrush2 = new SolidBrush(Color.Blue);
        private Pen mLinePen = new Pen(Color.Red, 5);
        private Brush mTextBrush = new SolidBrush(Color.Black);
        private Font mFont = new Font("Arial", 16);
        public List<Figure> figures = new List<Figure>();
        public List<Circle> circles = new List<Circle>();
        public bool select_State = false;
        public Function selectedFunction = null;
        public bool selectedStartingP = false;
        public Figure selectedFigure = null;


        public void InterfacepolationalAgoritm()
        {
            mLinePen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round,
                DashCap.Flat);
        }



        List<Gf> funcs = new List<Gf>();

        public void AddFunctions()
        {
            funcs.Add(new GfLine());
        }

        public void LineJoin(Function f1, Function f2)
        {
            double x1 = (f1.TransformEndingPoint.X - f1.TransformStartingPoint.X) / 2;
            double y1 = f1.BestFunction.CalcAll(f1.BestCoefficient, x1);
            Point p1 = f1.TransformInverse.MulP(new Point(x1, y1));
            double x2 = (f2.TransformEndingPoint.X - f2.TransformStartingPoint.X) / 2;
            double y2 = f2.BestFunction.CalcAll(f2.BestCoefficient, x2);
            Point p2 = f2.TransformInverse.MulP(new Point(x2, y2));
            double dis = Point.Distance(p1, p2);

            double[] xa = new double[8];
            double[] xb = new double[8];
            Point[] pa = new Point[8];
            Point[] pb = new Point[8];

            for (double step = 20; step >= 0.001; step /= 2)
            {
                while (true)
                {
                    double x11 = x1 - step;
                    double x12 = x2 - step;
                    double x21 = x1 + step;
                    double x22 = x2 + step;

                    double y11 = f1.BestFunction.CalcAll(f1.BestCoefficient, x11);
                    double y12 = f2.BestFunction.CalcAll(f2.BestCoefficient, x12);
                    double y21 = f1.BestFunction.CalcAll(f1.BestCoefficient, x21);
                    double y22 = f2.BestFunction.CalcAll(f2.BestCoefficient, x22);

                    Point p11 = f1.TransformInverse.MulP(new Point(x11, y11));
                    Point p12 = f2.TransformInverse.MulP(new Point(x12, y12));
                    Point p21 = f1.TransformInverse.MulP(new Point(x21, y21));
                    Point p22 = f2.TransformInverse.MulP(new Point(x22, y22));

                    pa[0] = p11; pb[0] = p12; xa[0] = x11; xb[0] = x12;
                    pa[1] = p11; pb[1] = p22; xa[1] = x11; xb[1] = x22;
                    pa[2] = p21; pb[2] = p12; xa[2] = x21; xb[2] = x12;
                    pa[3] = p21; pb[3] = p22; xa[3] = x21; xb[3] = x22;
                    pa[4] = p1; pb[4] = p12; xa[4] = x1; xb[4] = x12;
                    pa[5] = p1; pb[5] = p22; xa[5] = x1; xb[5] = x22;
                    pa[6] = p11; pb[6] = p2; xa[6] = x11; xb[6] = x2;
                    pa[7] = p21; pb[7] = p2; xa[7] = x21; xb[7] = x2;

                    double minDistance = Point.Distance(pa[0], pb[0]);
                    int mindistanceIndex = 0;
                    for(int i = 1; i < pa.Length; i++)
                    {
                        double distance = Point.Distance(pa[i], pb[i]);
                        if(distance < minDistance)
                        {
                            minDistance = distance;
                            mindistanceIndex = i;
                        }
                    }
                    if (minDistance >= dis) break;
                    dis = minDistance;
                    p1 = pa[mindistanceIndex];
                    p2 = pb[mindistanceIndex];
                    x1 = xa[mindistanceIndex];
                    x2 = xb[mindistanceIndex];
                }
            }
            Console.WriteLine("Join and distance " + dis);
            y1 = f1.BestFunction.CalcAll(f1.BestCoefficient, x1);
            y2 = f2.BestFunction.CalcAll(f2.BestCoefficient, x2);
            if(Math.Abs(f1.TransformStartingPoint.X - x1) < Math.Abs(f1.TransformEndingPoint.X - x1))
            {
                f1.TransformStartingPoint = new WebCam.Point(x1, y1);
                f1.StartingPoint = p1;
            }
            else
            {
                f1.TransformEndingPoint = new WebCam.Point(x1, y1);
                f1.EndingPoint = p1;
            }
            if (Math.Abs(f2.TransformStartingPoint.X - x2) < Math.Abs(f2.TransformEndingPoint.X - x2))
            {
                f2.TransformStartingPoint = new WebCam.Point(x2, y2);
                f2.StartingPoint = p2;
            }
            else
            {
                f2.TransformEndingPoint = new WebCam.Point(x2, y2);
                f2.EndingPoint = p2;
            }
        }

        public void AddPoint(Point p)
        {
            mPointList.Add(p);
        }

        public Function InterpolatePoints()
        {
            if(mPointList.Count < 5)return null;
            List<Point> tp = new List<Point>();
            //get vect
            Vector directionVector = new Vector();
            directionVector.X = mPointList[mPointList.Count - 1].X - mPointList[0].X;
            directionVector.Y = mPointList[mPointList.Count - 1].Y - mPointList[0].Y;
            //normalize
            directionVector = directionVector.Normalize();
            //cslcu matrix
            Matrix m = Matrix.mat(directionVector);
            Matrix mi = Matrix.Invert(m);
            
            //transform
            int n = mPointList.Count;
            for (int i = 0; i < n; i++)
            {
                tp.Add(mi.MulP(mPointList[i]));
            }

            List<double> bestC = null;
            double bestSqr = double.MaxValue;
            Gf bestF = funcs[0];
            foreach (Gf func in funcs)
            {
                List<double> c = LSM.CalcLSM(func, tp);
                if (c == null) continue;
                double sqrs = 0;
                for (int j = 0; j < tp.Count; j++)
                {
                    sqrs += Math.Pow(tp[j].Y - func.CalcAll(c, tp[j].X), 2);
                }
                if (sqrs < bestSqr)
                {
                    bestSqr = sqrs;
                    bestF = func;
                    bestC = c;
                }
            }
            if (bestC == null) return null;

            Function mFunction = new Function();
            mFunction.BestFunction = bestF;
            mFunction.BestCoefficient = bestC;
            mFunction.bestSqr = bestSqr;
            mFunction.Transform = mi;
            mFunction.TransformInverse = m;
            mFunction.TransformStartingPoint = tp[0];
            mFunction.TransformStartingPoint.Y = mFunction.BestFunction.CalcAll(mFunction.BestCoefficient, mFunction.TransformStartingPoint.X);
            mFunction.TransformEndingPoint = tp[mPointList.Count - 1];
            mFunction.TransformEndingPoint.Y = mFunction.BestFunction.CalcAll(mFunction.BestCoefficient, mFunction.TransformEndingPoint.X);
            mFunction.StartingPoint = mFunction.TransformInverse.MulP(mFunction.TransformStartingPoint);
            mFunction.EndingPoint = mFunction.TransformInverse.MulP(mFunction.TransformEndingPoint);
            return mFunction;
    }

    public void AddFunction(Function f, bool last)
    {
            /*Point closestPoint;
            double minD = double.MaxValue;
            foreach (Function func in ExistingGraphics)
            {
               double distance = Point.Distance(func.StartingPoint, f.EndingPoint);
                if (minD > distance)
                {
                    minD = distance;
                    closestPoint = func.StartingPoint;
                }
                distance = Point.Distance(func.EndingPoint, f.EndingPoint);
                if (minD > distance)
                {
                    minD = distance;
                    closestPoint = func.EndingPoint;
                }
            }
            if(minD < 100)
                {
                f.EndingPoint = ;
            }*/
            bool join = false;
            if(last && (ExistingGraphics.Count > 0))
            {
                if(Point.Distance(ExistingGraphics[0].StartingPoint, f.EndingPoint) < 100)
                {
                    join = true;
                }
            }


            double dx = f.EndingPoint.X - f.StartingPoint.X;
            double dy = f.EndingPoint.Y - f.StartingPoint.Y;
            double alfa = 0;
            List<double> angle = new List<double>();
            angle.Add(0);
            angle.Add(Math.PI / 6);
            angle.Add(Math.PI / 4);
            angle.Add(Math.PI / 3);
            angle.Add(Math.PI / 2);
            angle.Add(2 * Math.PI / 3);
            angle.Add(3 * Math.PI / 4);
            angle.Add(5 * Math.PI / 6);
            angle.Add(Math.PI);
            angle.Add(7 * Math.PI / 6);
            angle.Add(5 * Math.PI / 4);
            angle.Add(4 * Math.PI / 3);
            angle.Add(3 * Math.PI / 2);
            angle.Add(5 * Math.PI / 3);
            angle.Add(7 * Math.PI / 4);
            angle.Add(11 * Math.PI / 6);
            if (Math.Abs(dx) > Gauss.Epsilon)
            {
                if (dx > 0)
                    alfa = Math.Atan(dy / dx);
                else alfa = Math.Atan(dy / dx) + Math.PI;
                double min = double.MaxValue;
                double realValue = 0;
                foreach( double a in angle)
                {
                    double dif = a - alfa;
                    while (dif > Math.PI)
                        dif -= 2 * Math.PI;
                    while (dif < -Math.PI)
                        dif += 2 * Math.PI;
                    if (Math.Abs(dif) < min)
                    {
                        min = Math.Abs(dif);
                        realValue = dif; 
                    }
                }
                if (min < 0.15)
                {
                    double cx = (f.EndingPoint.X + f.StartingPoint.X) / 2;
                    double cy = (f.EndingPoint.Y + f.StartingPoint.Y) / 2;
                    Matrix curMat = new Matrix();
                    curMat.a13 = cx;
                    curMat.a23 = cy;
                    Matrix secMat = new Matrix();
                    //realValue = -realValue;
                    secMat.a11 = Math.Cos(realValue);
                    secMat.a12 = - Math.Sin(realValue);
                    secMat.a21 = Math.Sin(realValue);
                    secMat.a22 = Math.Cos(realValue);
                    Matrix tirdMat = new Matrix();
                    tirdMat.a13 = -cx;
                    tirdMat.a23 = -cy;
                    Matrix n = Matrix.Mul(curMat, secMat);
                    Matrix multipleM = Matrix.Mul(n, tirdMat);
                    f.TransformInverse = Matrix.Mul(multipleM, f.TransformInverse);
                    f.Transform = Matrix.Invert(f.TransformInverse);
                    f.StartingPoint = f.TransformInverse.MulP(f.TransformStartingPoint);
                    f.EndingPoint = f.TransformInverse.MulP(f.TransformEndingPoint);
                    dx = f.EndingPoint.X - f.StartingPoint.X;
                    dy = f.EndingPoint.Y - f.StartingPoint.Y;
                    if (dx > 0)
                        alfa = Math.Atan(dy / dx);
                    else alfa = Math.Atan(dy / dx) + Math.PI;
                    min = double.MaxValue;
                }


            }

        if(join)
            LineJoin(f, ExistingGraphics[0]);

        ExistingGraphics.Add(f);

        if (ExistingGraphics.Count >= 2)
        {
            LineJoin(ExistingGraphics[ExistingGraphics.Count - 2], ExistingGraphics[ExistingGraphics.Count - 1]);
        }
    }

    public bool CheckFunction(Function f)
        {
            if (f != null)
            {
                if (f.bestSqr / mPointList.Count > 50)
                {
                    for(int i = 0; i < 4; i++)
                    {
                        mPointList.RemoveAt(mPointList.Count - 1);
                        Function f2 = InterpolatePoints();
                        if (f2 != null)
                        {
                            f = f2;
                            if (f.bestSqr / mPointList.Count < 20) break;
                        }
                    }
                    Console.WriteLine("cut");
                    AddFunction(f, false);
                    mPointList.Clear();
                    return true;
                }
            }
            return false;
        }

        public bool EndGesture(MainForm form)
        {
            Function f = InterpolatePoints();
            if (f != null)
            {
                    AddFunction(f, true);
                    mPointList.Clear();
            }
            if(ExistingGraphics.Count > 0)
            {
                Figure cfig = new Figure();
                cfig.AddFigure(this);
                FixFigure(cfig);
                figures.Add(cfig);
                ExistingGraphics.Clear();
                cfig.Announce(form);
                return true;
            }
            return false;
        }

        private Point FindSnap(List<Point> snapPoints, Point p)
        {
            Point closest = null;
            double minDistance = double.MaxValue;
            foreach (Point p1 in snapPoints)
            {
                double d = Math.Sqrt((p1.X - p.X) * (p1.X - p.X) + (p1.Y - p.Y) * (p1.Y - p.Y));
                if (d < minDistance)
                {
                    minDistance = d;
                    closest = p1;
                }
            }
            if(minDistance < 50)
            {
                return closest;
            }
            return p;
        }

        public void FixCircle(Circle c)
        {
            List<Point> snapPoints = GetSnapPoints();
            Point cp = FindSnap(snapPoints, new Point(c.X, c.Y));
            c.X = cp.X;
            c.Y = cp.Y;
        }

        private List<Point> GetSnapPoints()
        {
            List<Point> snapPoints = new List<Point>();
            foreach (Figure of in figures)
            {
                foreach (Function fun in of.fig)
                {
                    snapPoints.Add(fun.StartingPoint);
                    snapPoints.Add(fun.EndingPoint);
                    snapPoints.Add(
                        new Point(
                            (fun.StartingPoint.X + fun.EndingPoint.X) / 2,
                            (fun.StartingPoint.Y + fun.EndingPoint.Y) / 2));
                }
            }
            foreach (Circle c in circles)
            {
                snapPoints.Add(new Point(c.X, c.Y));
            }
            return snapPoints;
        }

        private void FixFigure(Figure f)
        {
            List<Point> snapPoints = GetSnapPoints();
            foreach (Function fun in f.fig)
            {
                fun.StartingPoint = FindSnap(snapPoints, fun.StartingPoint);
                fun.EndingPoint = FindSnap(snapPoints, fun.EndingPoint);
            }
            /*            int variableCount = f.fig.Count * 2;
                        bool closed = false;
                        if (Point.Distance(f.fig[0].StartingPoint, f.fig[f.fig.Count - 1].EndingPoint) < 0.1)
                        {
                            closed = true;
                        }
                        else
                        {
                            variableCount += 2;
                        }
                        List<FuncN> funcs = new List<FuncN>();
                        bool[] used = new bool[variableCount];
                        for (int i = 0; i < f.fig.Count; i++)
                        {
                            Function fun = f.fig[i];
                            Point snapStart = FindSnap(snapPoints, fun.StartingPoint);
                            if (snapStart != fun.StartingPoint)
                            {
                                if (!used[i * 2])
                                {
                                    funcs.Add(new FuncConst(variableCount, i * 2, snapStart.X));
                                    used[i * 2] = true;
                                }
                                if (!used[i * 2 + 1])
                                {
                                    funcs.Add(new FuncConst(variableCount, i * 2 + 1, snapStart.Y));
                                    used[i * 2 + 1] = true;
                                }
                            }
                            Point snapEnd = FindSnap(snapPoints, fun.EndingPoint);
                            if (snapEnd != fun.EndingPoint)
                            {
                                if (!used[(i * 2 + 2) % variableCount])
                                {
                                    funcs.Add(new FuncConst(variableCount, (i * 2 + 2) % variableCount, snapEnd.X));
                                    used[(i * 2 + 2) % variableCount] = true;
                                }
                                if (!used[(i * 2 + 3) % variableCount])
                                {
                                    funcs.Add(new FuncConst(variableCount, (i * 2 + 3) % variableCount, snapEnd.Y));
                                    used[(i * 2 + 3) % variableCount] = true;
                                }
                            }
                        }

                        f.GetRules(funcs, variableCount);
                        while (funcs.Count < variableCount)
                        {
                            for (int i = 0; i < variableCount; i++)
                            {
                                if (!used[i])
                                {
                                    int fn = i / 2;
                                    bool ending = false;
                                    if (fn >= f.fig.Count)
                                    {
                                        ending = true;
                                        fn--;
                                    }
                                    Point vPoint = (ending ? f.fig[fn].EndingPoint : f.fig[fn].StartingPoint);
                                    double v = ((i % 2) == 0) ? vPoint.X : vPoint.Y;
                                    funcs.Add(new FuncConst(variableCount, i, v));
                                    used[i] = true;
                                    break;
                                }
                            }
                        }
                        double[] x = new double[variableCount];
                        for (int i = 0; i < f.fig.Count; i++)
                        {
                            x[i * 2] = f.fig[i].StartingPoint.X;
                            x[i * 2 + 1] = f.fig[i].StartingPoint.Y;
                        }
                        if (!closed)
                        {
                            x[variableCount - 2] = f.fig[f.fig.Count - 1].EndingPoint.X;
                            x[variableCount - 1] = f.fig[f.fig.Count - 1].EndingPoint.Y;
                        }
                        x = NMJ.Solve(funcs.ToArray(), x);
                        if (x != null)
                        {
                            for (int i = 0; i < f.fig.Count; i++)
                            {
                                Function fun = f.fig[i];
                                fun.StartingPoint.X = x[i * 2];
                                fun.StartingPoint.Y = x[i * 2 + 1];
                                fun.EndingPoint.X = x[(i * 2 + 2) % variableCount];
                                fun.EndingPoint.Y = x[(i * 2 + 3) % variableCount];
                            }
                        }*/
        }

        public void DrawFunctionFinished(Graphics g, Function f)
        {
            float r = 2;
            g.DrawLine(mLinePen, (float)f.StartingPoint.X, (float)f.StartingPoint.Y, (float)f.EndingPoint.X, (float)f.EndingPoint.Y);
            g.FillEllipse(mDotBrush, (float)((f.StartingPoint.X + f.EndingPoint.X) / 2 - r), (float)((f.StartingPoint.Y + f.EndingPoint.Y) / 2 - r), 2 * r, 2 * r);
        }

        public void DrawFunction(Graphics g, Gf f, List<double> c, Point s, Point e, Matrix transform)
        {
            if (s.X > e.X)
            {
                Point b = s;
                s = e;
                e = b;
            }
            Point previous = new Point();
            Point current = new Point();
            Point pt = null;
            Point ct = null;
            double step = (e.X - s.X) / 10;
            for (double x = s.X; x < e.X; x += step)
            {
                current.X = x;
                current.Y = f.CalcAll(c, x);
                ct = transform.MulP(current);
                if(x > s.X)
                {
                    g.DrawLine(mLinePen, (float)pt.X, (float)pt.Y, (float)ct.X, (float)ct.Y);
                }
                previous.X = current.X;
                previous.Y = current.Y;
                pt = ct;
            }
            current.X = e.X;
            current.Y = f.CalcAll(c, e.X);
            ct = transform.MulP(current);
            g.DrawLine(mLinePen, (float)pt.X, (float)pt.Y, (float)ct.X, (float)ct.Y);
        }

        public void DrawAllFunctions(Graphics g)
        {
            float r = 5;
            foreach (Figure fi in figures)
            {

                foreach (Function func_fig in fi.fig)
                {
                    DrawFunctionFinished(g, func_fig);
                }
                double sumOfcoordinatesX = 0;
                double sumOfcoordinatesY = 0;
                foreach (Function func_fig in fi.fig)
                {
                    sumOfcoordinatesX += func_fig.StartingPoint.X;
                    sumOfcoordinatesY += func_fig.StartingPoint.Y;
                }
                sumOfcoordinatesX /= fi.fig.Count;
                sumOfcoordinatesY /= fi.fig.Count;

                int i = 0;
                foreach (Function func_fig in fi.fig)
                {
                    Vector v = new Vector();
                    v.X = func_fig.StartingPoint.X - sumOfcoordinatesX;
                    v.Y = func_fig.StartingPoint.Y - sumOfcoordinatesY;
                    v = v.Normalize();
                    int d = 16;
                    v.X *= d;
                    v.Y *= d;
                    string letter = "" + (char)('A' + i);
                    SizeF size = g.MeasureString(letter, mFont);
                    g.DrawString(letter, mFont, mTextBrush, new PointF((float)(func_fig.StartingPoint.X + v.X - size.Width / 2), (float)(func_fig.StartingPoint.Y + v.Y - size.Height / 2)));
                    i++;
                }
                foreach (Function f1 in fi.fig)
                {
                    if (select_State && (f1 == selectedFunction))
                    {
                        if (selectedStartingP)
                            g.FillEllipse(mDotBrush2, (float)selectedFunction.StartingPoint.X - r, (float)selectedFunction.StartingPoint.Y - r, 2 * r, 2 * r);
                        else
                            g.FillEllipse(mDotBrush2, (float)selectedFunction.EndingPoint.X - r, (float)selectedFunction.EndingPoint.Y - r, 2 * r, 2 * r);
                    }
                }
            }
            foreach (Figure fi in figures)
            {
                foreach (Function func_fig in fi.fig)
                {
                    g.FillEllipse(mDotBrush, (float)func_fig.StartingPoint.X - r, (float)func_fig.StartingPoint.Y - r, 2 * r, 2 * r);
                    g.FillEllipse(mDotBrush, (float)func_fig.EndingPoint.X - r, (float)func_fig.EndingPoint.Y - r, 2 * r, 2 * r);
                }
            }
            foreach (Circle fc in circles)
            {
                g.DrawEllipse(mLinePen, (float)(fc.X - fc.R), (float)(fc.Y - fc.R), (float)(2 * fc.R), (float)(2 * fc.R));
                g.FillEllipse(mDotBrush, (float)fc.X - r, (float)fc.Y - r, 2 * r, 2 * r);
                g.DrawString("P", mFont, mTextBrush, new PointF((float)(fc.X), (float)(fc.Y)));
            }
            foreach (Function f1 in ExistingGraphics)
            {
                DrawFunction(g, f1.BestFunction, f1.BestCoefficient, f1.TransformStartingPoint, f1.TransformEndingPoint, f1.TransformInverse);
            }
            foreach (Function f1 in ExistingGraphics)
            {
                g.FillEllipse(
                    mDotBrush,
                    (float)f1.StartingPoint.X - r, (float)f1.StartingPoint.Y - r, 2 * r, 2 * r);
                g.FillEllipse(
                    mDotBrush,
                    (float)f1.EndingPoint.X - r, (float)f1.EndingPoint.Y - r, 2 * r, 2 * r);
            }
        }

        public void DrawPoints(Graphics g)
        {
            float r = 5;
            foreach (Point point in mPointList)
            {
                g.FillEllipse(mDotBrush, (float)point.X - r, (float)point.Y - r, 2 * r, 2 * r);
            }
        }


        public void ClearAll()
        {
            mPointList.Clear();
            ExistingGraphics.Clear();
            figures.Clear();
            circles.Clear();
        }

        public void ClearGesture()
        {
            mPointList.Clear();
            ExistingGraphics.Clear();
        }

        /*
        
        DrawAll(graphics) */
    }
}
