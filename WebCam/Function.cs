using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebCam
{
    class Function
    {
        public Gf BestFunction; 
        public List<double> BestCoefficient;
        public Point StartingPoint;
        public Point EndingPoint;
        public double bestSqr;
        public Matrix Transform;
        public Matrix TransformInverse;
        public Point TransformStartingPoint;
        public Point TransformEndingPoint;

        public Function Clone()
        {
            Function cloning = new Function();
            cloning.BestFunction = BestFunction;
            cloning.BestCoefficient = new List<double>(BestCoefficient);
            cloning.StartingPoint = StartingPoint.Clone();
            cloning.EndingPoint = EndingPoint.Clone();
            cloning.bestSqr = bestSqr;
            cloning.Transform = Transform.Clone();
            cloning.TransformInverse = TransformInverse.Clone();
            cloning.TransformStartingPoint = TransformStartingPoint.Clone();
            cloning.TransformEndingPoint = TransformEndingPoint.Clone();

            return cloning;
        }
    }
}
