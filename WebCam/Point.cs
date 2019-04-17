using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCam
{
    class Point
    {
        public double X;
        public double Y;

        public Point()
        {
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public static double Distance(Point p1, Point p2)
        {
            double d = Math.Sqrt(((p2.X - p1.X) * (p2.X - p1.X)) + ((p2.Y - p1.Y) * (p2.Y - p1.Y)));
            return d;
        }

        public Point Clone()
        {
            Point p = new Point();
            p.X = X;
            p.Y = Y;

            return p;
        }
    }
}
