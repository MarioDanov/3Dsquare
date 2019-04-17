using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newton_method
{
    public class NMJ
    {
        public static double Epsilon = 1e-6;

        public static double[] Solve(FuncN[] funcs, double[] p)
        {
            for (int cnt = 0; cnt < 100; cnt++)
            {
                MatrixN y = new MatrixN(1, funcs.Length);

                int zeros = 0;
                for (int k = 0; k < funcs.Length; k++)
                {
                    y.a[k, 0] = funcs[k].calc(p);
                    if (Math.Abs(y.a[k, 0]) < Epsilon)
                        zeros++;
                }
                if (zeros == funcs.Length) break;

                MatrixN j = new MatrixN(p.Length, funcs.Length);
                for (int i = 0; i < p.Length; i++)
                {
                    for (int k = 0; k < funcs.Length; k++)
                    {
                        j.a[k, i] = funcs[k].calcDer(i, p);
                    }
                }
                MatrixN jt = MatrixN.Transpose(j);
                MatrixN gm = MatrixN.Multiply(jt, j);
                MatrixN im = MatrixN.Invert(gm);
                if (im == null)
                {
                    for (int i = 0; i < p.Length; i++)
                    {
                        p[i] += 0.01;
                    }
                    continue;
                }
                MatrixN jp = MatrixN.Multiply(im, jt);
                MatrixN dif = MatrixN.Multiply(jp, y);

                for (int i = 0; i < p.Length; i++)
                {
                    p[i] -= dif.a[i, 0];
                }
            }
            return p;

        }
    }
}
