using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newton_method
{
    public abstract class FuncN
    {
        public abstract int cnt();
        public abstract double calc(double[] p);
        public abstract double calcDer(int var, double[] p);
    }
}
