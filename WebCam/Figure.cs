using Newton_method;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCam
{
    class Figure
    {
        public List<Function> fig = new List<Function>();
        public void AddFigure(InterpolationAlgoritm alg)
        {
            for (int i = 0; i < alg.ExistingGraphics.Count; i++)
            {
                fig.Add(alg.ExistingGraphics[i]);
            }
        }

        public void GetRules(List<FuncN> funcs, int variableCount)
        {
            bool closed = (Point.Distance(fig[0].StartingPoint, fig[fig.Count - 1].EndingPoint) < 0.001);
            if (!closed)
            {
                return;
            }
            int sides = fig.Count;
            double[] lengths = new double[fig.Count];
            double[] angles = new double[fig.Count];
            for (int i = 0; i < fig.Count; i++)
            {
                lengths[i] = Point.Distance(fig[i].StartingPoint, fig[i].EndingPoint);
                int j = (i + fig.Count - 1) % fig.Count;
                Vector v1 = new Vector(fig[i].EndingPoint.X - fig[i].StartingPoint.X, fig[i].EndingPoint.Y - fig[i].StartingPoint.Y);
                Vector v2 = new Vector(fig[j].StartingPoint.X - fig[j].EndingPoint.X, fig[j].StartingPoint.Y - fig[j].EndingPoint.Y);
                v1 = v1.Normalize();
                v2 = v2.Normalize();
                double cos = v1.X * v2.X + v1.Y * v2.Y;
                double angle = Math.Acos(cos);
                angles[i] = angle;
            }
            bool treatAsPolygon = false;
            if (sides == 3)
            {
                bool provoygylen = false;
                bool typoygylen = false;
                int ravnobedren = 0;
                int rightAngle = 0;
                for (int k = 0; k < 3; k++)
                {
                    if (Math.Abs(angles[k] - Math.PI / 2) < 0.01)
                    {
                        provoygylen = true;
                        rightAngle = k;
                    }
                    else if (angles[k] > Math.PI / 2)
                    {
                        typoygylen = true;
                    }
                    if (Math.Abs(lengths[k] - lengths[(k + 2) % 3]) < 5)
                    {
                        ravnobedren++;
                    }
                }
                if (provoygylen)
                {
                    int d = 0;
                    funcs.Add(new FuncRightAngle(
                        variableCount,
                        ((rightAngle + d + variableCount) * 2) % variableCount, ((rightAngle + d + variableCount) * 2 + 1) % variableCount,
                        ((rightAngle + d - 1 + variableCount) * 2) % variableCount, ((rightAngle + d - 1 + variableCount) * 2 + 1) % variableCount,
                        ((rightAngle + d + 1 + variableCount) * 2) % variableCount, ((rightAngle + d + 1 + variableCount) * 2 + 1) % variableCount));
                }
            }
        }

        public void Announce(MainForm form)
        {
            bool closed = (Point.Distance(fig[0].StartingPoint, fig[fig.Count - 1].EndingPoint) < 0.001);
            if(!closed)
            {
                return;
            }
            int sides = fig.Count;
            double[] lengths = new double[fig.Count];
            double[] angles = new double[fig.Count];
            for(int i = 0; i < fig.Count; i++)
            {
                lengths[i] = Point.Distance(fig[i].StartingPoint, fig[i].EndingPoint);
                int j = (i + fig.Count - 1) % fig.Count;
                Vector v1 = new Vector(fig[i].EndingPoint.X - fig[i].StartingPoint.X, fig[i].EndingPoint.Y - fig[i].StartingPoint.Y);
                Vector v2 = new Vector(fig[j].StartingPoint.X - fig[j].EndingPoint.X, fig[j].StartingPoint.Y - fig[j].EndingPoint.Y);
                v1 = v1.Normalize();
                v2 = v2.Normalize();
                double cos = v1.X * v2.X + v1.Y * v2.Y;
                double angle = Math.Acos(cos);
                angles[i] = angle;
            }
            bool treatAsPolygon = false;
            if (sides == 3)
            {
                bool provoygylen = false;
                bool typoygylen = false;
                int ravnobedren = 0;
                for (int k = 0; k < 3; k++)
                {
                    if (Math.Abs(angles[k] - Math.PI / 2) < 0.01)
                    {
                        provoygylen = true;
                    }
                    else if (angles[k] > Math.PI / 2)
                    {
                        typoygylen = true;
                    }
                    if (Math.Abs(lengths[k] - lengths[(k + 2) % 3]) < 5)
                    {
                        ravnobedren++;
                    }
                }
                if (provoygylen)
                {
                    if (ravnobedren > 0)
                    {
                        form.ShowMassage("IsoscelesRightAngledTriangle.wav", "Правоъгълен равнобедрен триъгълник");
                    }
                    else
                    {
                        form.ShowMassage("RightAngledTriangle.wav", "Правоъгълен триъгълник");
                    }
                }
                else if (typoygylen)
                {
                    if (ravnobedren > 0)
                    {
                        form.ShowMassage("IsoscelesObtuseTriangle.wav", "Тъпоъгълен равнобедрен триъгълник");
                    }
                    else
                    {
                        form.ShowMassage("ObtuseTriangle.wav", "Тъпоъгълен триъгълник");
                    }
                }
                else
                {
                    if (ravnobedren > 1)
                    {
                        form.ShowMassage("EquilateralTriangle.wav", " Равностранен триъгълник");
                    }
                    else if (ravnobedren > 0)
                    {
                        form.ShowMassage("IsoscelesAcuteTriangle.wav", "Остроъгълен равнобедрен триъгълник");
                    }
                    else
                    {
                        form.ShowMassage("AcuteTriangle.wav", "Остроъгълен триъгълник");
                    }
                }
            }
            else if (sides == 4)
            {
                int l = 0;
                int l2 = 0;
                int equalOpositeSides = 0;
                for (int k = 0; k < 4; k++)
                {
                    if (Math.Abs(angles[k] - Math.PI / 2) < 0.01)
                    {
                        l++;
                    }

                    if (Math.Abs(angles[k] + angles[(k + 1) % 4] - Math.PI) < 0.01)
                    {
                        l2++;
                    }
                    if (Math.Abs(lengths[k] - lengths[(k + 2) % 4]) < 5)
                    {
                        equalOpositeSides++;
                    }

                }
                if (l == 4)
                {
                    form.ShowMassage("Rectangle.wav", "Правоъгълник");
                }
                else if (l2 > 0)
                {
                    if (l2 == 4)
                    {
                        form.ShowMassage("Parallelogram.wav", "Успоредник");
                    }
                    else
                    {
                        if (l > 0)
                        {
                            form.ShowMassage("RightAngledTrapez.wav", "Правоъгълен трапец");
                        }
                        else
                        {
                            if (equalOpositeSides > 0)
                            {
                                form.ShowMassage("IsoscelesTrapez.wav", "Равнобедрен трапец");
                            }
                            else
                            {
                                form.ShowMassage("Trapez.wav", "Трапец");
                            }
                        }
                    }
                }
                else treatAsPolygon = true;
            }
            else treatAsPolygon = true;
            if(treatAsPolygon)
            {
                bool concave = false;
                for (int k = 0; k < sides; k++)
                {
                    Line a = new Line();
                    a.A = -(fig[k].EndingPoint.Y - fig[k].StartingPoint.Y);
                    a.B = fig[k].EndingPoint.X - fig[k].StartingPoint.X;
                    a.C = -a.A * fig[k].StartingPoint.X - a.B * fig[k].StartingPoint.Y;
                    for (int m = 0; m < sides; m++)
                    {
                        if (k != m)
                        {
                            Line b = new Line();
                            b.A = -(fig[m].EndingPoint.Y - fig[m].StartingPoint.Y);
                            b.B = fig[m].EndingPoint.X - fig[m].StartingPoint.X;
                            b.C = -b.A * fig[m].StartingPoint.X - b.B * fig[m].StartingPoint.Y;
                            Point p = Line.Intersect(a, b);
                            if(
                                (
                                (Math.Abs(fig[k].StartingPoint.X - fig[k].EndingPoint.X) - Math.Abs(p.X - fig[k].StartingPoint.X) > 4)
                                &&
                                (Math.Abs(fig[k].StartingPoint.X - fig[k].EndingPoint.X) - Math.Abs(p.X - fig[k].EndingPoint.X) > 4)
                                )
                                ||
                                (
                                (Math.Abs(fig[k].StartingPoint.Y - fig[k].EndingPoint.Y) - Math.Abs(p.Y - fig[k].StartingPoint.Y) > 4)
                                &&
                                (Math.Abs(fig[k].StartingPoint.Y - fig[k].EndingPoint.Y) - Math.Abs(p.Y - fig[k].EndingPoint.Y) > 4)
                                ))
                            {
                                concave = true;
                            }

                        }
                    }
                }
                if(concave)
                {
                    if (sides == 4)
                    {
                        form.ShowMassage("ConcaveQuadragon.wav", "Вдлъбнат четириъгълник");
                    }
                    else if(sides == 5)
                    {
                        form.ShowMassage("ConcavePentagon.wav", "Вдлъбнат петоъгълник");
                    }
                    else
                    {
                        form.ShowMassage("ConcavePolygon.wav", "Вдлъбнат многоъгълник");
                    }

                }
                else
                {
                    if (sides == 4)
                    {
                        form.ShowMassage("ConvexQuadragon.wav", "Изпъкнал четириъгълник");
                    }
                    else if (sides == 5)
                    {
                        form.ShowMassage("ConvexPentagon.wav", "Изпъкнал петоъгълник");
                    }
                    else
                    {
                        form.ShowMassage("ConvexPolygon.wav", "Изпъкнал многоъгълник");
                    }
                }
            }
        }
    }
}

