using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCam
{
    class Line
    {
        public double A;
        public double B;
        public double C;

        public static Point Intersect(Line l1, Line l2)
        {
            Point p = new Point();
            p.X = ((-l1.C * l2.B) - ((- l2.C) * l1.B)) / ((l1.A * l2.B) - (l2.A * l1.B));
            p.Y = ((l1.A) * (-l2.C) - (l2.A * (-l1.C))) / ((l1.A * l2.B) - (l2.A * l1.B));
            return p;
        }

        public static Point ProjectPoint(Line l, Point p)
        {
            Line l2 = new Line();
            l2.A = -l.B;
            l2.B = l.A;
            l2.C = l.B * p.X - l.A * p.Y;
            return Intersect(l, l2);
        }
    }
}
