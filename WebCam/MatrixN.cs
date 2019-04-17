using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newton_method
{
    public class MatrixN
    {
        public readonly int Width;
        public readonly int Height;
        public readonly double[,] a;

        public MatrixN(int width, int height)
        {
            Width = width;
            Height = height;
            a = new double[Height, Width];
        }

        public MatrixN Clone()
        {
            MatrixN r = new MatrixN(Width, Height);
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                    r.a[row, col] = a[row, col];
            }
            return r;
        }
        public static MatrixN Transpose(MatrixN a)
        {
            MatrixN b = new MatrixN(a.Height, a.Width);
            for(int row = 0; row < a.Height; row++)
            {
                for(int col = 0; col < a.Width; col++)
                {
                    b.a[col, row] = a.a[row, col];
                }
            }
            return b;
        }

        public static MatrixN Multiply(MatrixN a1, MatrixN b1)
        {
            if (a1.Width != b1.Height) return null;
            MatrixN c = new MatrixN(b1.Width, a1.Height);
            for(int i = 0; i < c.Height; i++)
            {
                for (int j = 0; j < c.Width; j++)
                {
                    for (int k = 0; k < a1.Width; k++)
                    {
                        c.a[i , j] += a1.a[i, k] * b1.a[k, j];
                    }
                }
            }
            return c;
        }

        public static MatrixN Invert(MatrixN a)
        {
            a = a.Clone();
            MatrixN b = new MatrixN(a.Width, a.Height);
            for (int row = 0; row < a.Height; row++)
            {
                for (int col = 0; col < a.Width; col++)
                    if (row == col)
                        b.a[row, col] = 1;
            }
            for (int col2 = 0; col2 < a.Width; col2++)
            {
                if (Math.Abs(a.a[col2, col2]) < 0.000001)
                {
                    bool found = false;
                    for (int row2 = col2 + 1; row2 < a.Height; row2++)
                    {
                        if (Math.Abs(a.a[row2, col2]) > 0.000001)
                        {
                            for (int col3 = 0; col3 < a.Width; col3++)
                            {
                                a.a[col2, col3] += a.a[row2, col3];
                                b.a[col2, col3] += b.a[row2, col3];
                            }
                            found = true;
                            break;
                        }
                    }
                    if (!found) return null;
                }
                for (int row2 = col2 + 1; row2 < a.Height; row2++)
                {
                    double kofficient = -(a.a[row2, col2] / a.a[col2, col2]);
                    for (int col3 = 0; col3 < a.Width; col3++)
                    {
                        a.a[row2, col3] += a.a[col2, col3] * kofficient;
                        b.a[row2, col3] += b.a[col2, col3] * kofficient;
                    }
                }
            }
            for (int col2 = a.Width - 1; col2 >= 0; col2--)
            {
                for (int row2 = col2 - 1; row2 >= 0; row2--)
                {
                    double kofficient = -(a.a[row2, col2] / a.a[col2, col2]);
                    for (int col3 = 0; col3 < a.Width; col3++)
                    {
                        a.a[row2, col3] += a.a[col2, col3] * kofficient;
                        b.a[row2, col3] += b.a[col2, col3] * kofficient;
                    }
                }
                {
                    double kofficient = 1 / a.a[col2, col2];
                    for (int col3 = 0; col3 < a.Width; col3++)
                    {
                        a.a[col2, col3] *= kofficient;
                        b.a[col2, col3] *= kofficient;
                    }
                }
            }
            return b;
        }
    }
}
