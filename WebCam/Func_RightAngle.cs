using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newton_method
{
    public class FuncRightAngle : FuncN
    {
        public readonly int Cnt;
        public readonly int VarX1;
        public readonly int VarY1;
        public readonly int VarX2;
        public readonly int VarY2;
        public readonly int VarX3;
        public readonly int VarY3;

        public FuncRightAngle(int cnt, int varX1, int varY1, int varX2, int varY2, int varX3, int varY3)
        {
            Cnt = cnt;
            VarX1 = varX1;
            VarY1 = varY1;
            VarX2 = varX2;
            VarY2 = varY2;
            VarX3 = varX3;
            VarY3 = varY3;

        }

        public override double calc(double[] p)
        {
            return (p[VarX2] - p[VarX1]) * (p[VarX3] - p[VarX1]) + (p[VarY2] - p[VarY1]) * (p[VarY3] - p[VarY1]);
        }

        public override double calcDer(int var, double[] p)
        {
            if (var == VarX1)
                return -p[VarX2] - p[VarX3] + 2 * p[VarX1];
            if (var == VarX2)
                return p[VarX3] - p[VarX1];
            if (var == VarX3)
                return p[VarX2] - p[VarX1];
            if (var == VarY1)
                return -p[VarY2] - p[VarY3] + 2 * p[VarY1];
            if (var == VarY2)
                return p[VarY3] - p[VarY1];
            if (var == VarY3)
                return p[VarY2] - p[VarY1];
            else
                return 0;
        }

        public override int cnt()
        {
            return Cnt;
        }
    }
}
