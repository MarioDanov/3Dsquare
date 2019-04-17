using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCam
{
    class Matrix
    {
        public double a11;
        public double a12;
        public double a21;
        public double a22;
        public double a13;
        public double a23;

        public Matrix()
        {
            a11 = 1;
            a12 = 0;
            a21 = 0;
            a22 = 1;
            a13 = 0;
            a23 = 0;
        }

        public Matrix Clone()
        {
            Matrix r = new Matrix();
            r.a11 = a11;
            r.a12 = a12;
            r.a13 = a13;
            r.a21 = a21;
            r.a22 = a22;
            r.a23 = a23;

            return r;
        }

        public Point MulP(Point p)
        {
            Point pointResult = new Point();
            pointResult.X = a11 * p.X + a12 * p.Y + a13;
            pointResult.Y = a21 * p.X + a22 * p.Y + a23;

            return pointResult;
        }

        public static Matrix Mul(Matrix m1, Matrix m2)
        {
            Matrix r = new Matrix();
            r.a11 = m1.a11 * m2.a11 + m1.a12 * m2.a21;
            r.a12 = m1.a11 * m2.a12 + m1.a12 * m2.a22;
            r.a13 = m1.a11 * m2.a13 + m1.a12 * m2.a23 + m1.a13;
            r.a21 = m1.a21 * m2.a11 + m1.a22 * m2.a21;
            r.a22 = m1.a21 * m2.a12 + m1.a22 * m2.a22;
            r.a23 = m1.a21 * m2.a13 + m1.a22 * m2.a23 + m1.a23;

            return r;
        }

        public static Matrix Invert(Matrix a)
        {
            a = a.Clone();
            Matrix b = new Matrix();
            if (Math.Abs(a.a11) < 0.000001)
            {
                a.a11 += a.a21;
                a.a12 += a.a22;
                a.a13 += a.a23;
                b.a11 += b.a21;
                b.a12 += b.a22;
                b.a13 += b.a23;
            }
            if (Math.Abs(a.a11) < 0.000001) return null;
            b.a13 = -a.a13;
            b.a23 = -a.a23;
            a.a13 = 0;
            a.a23 = 0;
            double k = -a.a21 / a.a11;
            a.a21 += k * a.a11;
            a.a22 += k * a.a12;
            a.a23 += k * a.a13;
            b.a21 += k * b.a11;
            b.a22 += k * b.a12;
            b.a23 += k * b.a13;
            if (Math.Abs(a.a22) < 0.000001) return null;
            k = -a.a12 / a.a22;
            a.a11 += k * a.a21;
            a.a12 += k * a.a22;
            a.a13 += k * a.a23;
            b.a11 += k * b.a21;
            b.a12 += k * b.a22;
            b.a13 += k * b.a23;
            k = a.a11;
            a.a11 /= k;
            a.a12 /= k;
            a.a13 /= k;
            b.a11 /= k;
            b.a12 /= k;
            b.a13 /= k;
            k = a.a22;
            a.a21 /= k;
            a.a22 /= k;
            a.a23 /= k;
            b.a21 /= k;
            b.a22 /= k;
            b.a23 /= k;
            /* b.a11 = a.a21 + 1;
             b.a12 = a.a12 / a.a11 + a.a22;
             b.a13 = a.a13 / a.a11 + a.a23;
             */

            /*                b.a11 = -(a.a22) / ((a.a11 * a.a22) - (a.a12 * a.a21));
                            b.a12 = a.a12 / (a.a11 * a.a22 - a.a12 * a.a21);
                            b.a13 = -(a.a23 * a.a12 - a.a22 * a.a13) / (a.a11 * a.a22 - a.a12 * a.a21);
                            b.a21 = -a.a22 / (a.a11 * a.a22 - a.a12 * a.a21);
                            b.a22 = (a.a12 * a.a21 + a.a12 * a.a21 - a.a11 * a.a22) / (a.a11 * a.a22 * a.a21 - a.a12 * a.a21 * a.a21);
                            b.a23 = ((a.a21 * a.a23 * a.a12) - (a.a21 * a.a22 * a.a13) - (a.a23 * a.a11 * a.a22) + (a.a12 * a.a21 * a.a23)) / ((a.a21 * a.a11 * a.a22) - (a.a21 * a.a21 * a.a12));
                            */


            return b;
        }
        public static Matrix mat(Vector v)
        {
            double l = v.GetLength();
            Matrix per = new Matrix();
            per.a11 = v.X / l;
            per.a12 = -v.Y / l;
            per.a21 = v.Y / l;
            per.a22 = v.X / l;

            return per;
        }
        public static Matrix Mul_p(Matrix m1,Point p1)
        {
            Matrix r = new Matrix();
            Point p = new Point();
            r.a11 = m1.a11 * p.X;
            r.a12 = m1.a12 * p.Y;

            return r;
        }
    }
}
