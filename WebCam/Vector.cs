using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCam
{
    class Vector
    {
        public double X;
        public double Y;

        public Vector()
        {

        }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double GetLength()
        {
            double length = Math.Sqrt(X * X + Y * Y); 
            return length;
        }

        public Vector Normalize()
        {
            Vector v = new Vector();
            double length = GetLength();
            v.X = X / length;
            v.Y = Y / length;
            return v;
        }
    }
}
