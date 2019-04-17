using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCam
{
    abstract class Gf
    {
        public abstract double calc(int i, double x);
        public abstract int getK();

        public double CalcAll(List<double> c, double x)
        {
            double value = 0;
            int k = getK();
            for (int i = 0; i < k; i++)
            {
                value += c[i] * calc(i, x);
            }
            return value;
        }
    }

    class GfSqr : Gf
    {
        public override double calc(int i, double x)
        {
            if (i == 0) return x * x;
            if (i == 1) return x;
            if (i == 2) return 1;
            return 0;
        }

        public override int getK()
        {
            return 3;
        }
    }

    class GfLine : Gf
    {
        public override double calc(int i, double x)
        {
            if (i == 0) return x;
            if (i == 1) return 1;
            return 0;
        }

        public override int getK()
        {
            return 2;
        }
    }

    class LSM
    {
        public static List<double> CalcLSM(Gf gf, List<Point> points)
        {
            List<List<double>> a = new List<List<double>>();
            List<double> b = new List<double>();
            int n = points.Count;
            for (int i = 0; i < gf.getK(); i++)
            {
                a.Add(new List<double>());
                for (int j = 0; j < gf.getK(); j++)
                {
                    double av = 0;
                    for (int l = 0; l < n; l++)
                    {
                        av += gf.calc(j, points[l].X) * gf.calc(i, points[l].X);
                    }
                    a[i].Add(av);
                }
                double bv = 0;
                for (int l = 0; l < n; l++)
                {
                    bv += points[l].Y * gf.calc(i, points[l].X);
                }
                b.Add(bv);
            }
            return Gauss.Solve(a, b);
        }

        internal static void CalcLSM(int v, List<Point> x)
        {
            throw new NotImplementedException();
        }
    }

    class Gauss
    {
        public static double Epsilon = 1e-6;
        public static List<double> Solve(List<List<double>> a, List<double> b)
        {
            int n = b.Count;
            double j,r;
            for (int col = 0; col < n; col++)
            {
                if(Math.Abs(a[col][col]) < Epsilon)
                {
                    bool found = false;
                    for(int row = col + 1; row < n; row++)
                    {
                        if (a[row][col] > Epsilon)
                        {
                            for(int i = 0; i < n; i++)
                            {
                                j = a[row][i];
                                a[row][i] = a[col][i];
                                a[col][i] = j;
                            }  
                            r = b[row];
                            b[row] = b[col];
                            b[col] = r;
                            found = true;
                            break;
                        }
                    }
                    if (!found) return null;
                }
                for(int row = col + 1; row < n; row++)
                {
                    double k = -a[row][col] / a[col][col];
                    for(int i = 0; i < n; i++)
                    {
                        a[row][i] += a[col][i] * k;
                    }
                    b[row] += b[col] * k;
                }
            }
            List<double> solution = new List<double>();
            for (int i = 0; i < n; i++) solution.Add(0);
            for (int i = n - 1; i >= 0; i--)
            {
                double v = b[i];
                for (int l = i + 1; l < n; l++)
                {
                    v -= a[i][l] * solution[l];
                }
                v /= a[i][i];
                solution[i] = v;
            }


            return solution;

        }
    }
}
