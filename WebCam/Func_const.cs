using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newton_method
{
    public class FuncConst : FuncN
    {
        public readonly int Cnt;
        public readonly int Var1;
        public readonly double Value;

        public FuncConst(int cnt, int var1, double value)
        {
            Cnt = cnt;
            Var1 = var1;
            Value = value;
        }

        public override double calc(double[] p)
        {
            return p[Var1] - Value;
        }

        public override double calcDer(int var, double[] p)
        {
            if (var == Var1)
                return 1;
            else
                return 0;

        }
        public override int cnt()
        {
            return Cnt;
        }
    }
}
