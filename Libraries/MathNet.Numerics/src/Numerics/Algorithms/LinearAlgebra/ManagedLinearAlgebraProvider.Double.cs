// <copyright file="ManagedLinearAlgebraProvider.Double.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
// Copyright (c) 2009-2010 Math.NET
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
namespace MathNet.Numerics.Algorithms.LinearAlgebra
{
    using System;
    using Properties;
    using Threading;

    /// <summary>
    /// The managed linear algebra provider.
    /// </summary>
    public partial class ManagedLinearAlgebraProvider : ILinearAlgebraProvider
    {
        /// <summary>
        /// Adds a scaled vector to another: <c>y += alpha*x</c>.
        /// </summary>
        /// <param name="y">The vector to update.</param>
        /// <param name="alpha">The value to scale <paramref name="x"/> by.</param>
        /// <param name="x">The vector to add to <paramref name="y"/>.</param>
        /// <remarks>This equivalent to the AXPY BLAS routine.</remarks>
        public virtual void AddVectorToScaledVector(double[] y, double alpha, double[] x)
        {
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }

            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (y.Length != x.Length)
            {
                throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            if (alpha == 0.0)
            {
                return;
            }

            if (alpha == 1.0)
            {
                CommonParallel.For(0, y.Length, index => { y[index] += x[index]; });
            }
            else
            {
                CommonParallel.For(0, y.Length, index => { y[index] += alpha * x[index]; });
            }
        }

        /// <summary>
        /// Scales an array. Can be used to scale a vector and a matrix.
        /// </summary>
        /// <param name="alpha">The scalar.</param>
        /// <param name="x">The values to scale.</param>
        /// <remarks>This is equivalent to the SCAL BLAS routine.</remarks>
        public virtual void ScaleArray(double alpha, double[] x)
        {
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (alpha == 1.0)
            {
                return;
            }

            CommonParallel.For(0, x.Length, index => { x[index] = alpha * x[index]; });
        }

        /// <summary>
        /// Computes the dot product of x and y.
        /// </summary>
        /// <param name="x">The vector x.</param>
        /// <param name="y">The vector y.</param>
        /// <returns>The dot product of x and y.</returns>
        /// <remarks>This is equivalent to the DOT BLAS routine.</remarks>
        public virtual double DotProduct(double[] x, double[] y)
        {
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }

            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (y.Length != x.Length)
            {
                throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            return CommonParallel.Aggregate(0, y.Length, index => y[index] * x[index]);
        }

        /// <summary>
        /// Does a point wise add of two arrays <c>z = x + y</c>. This can be used 
        /// to add vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the addition.</param>
        /// <remarks>There is no equivalent BLAS routine, but many libraries
        /// provide optimized (parallel and/or vectorized) versions of this
        /// routine.</remarks>
        public virtual void AddArrays(double[] x, double[] y, double[] result)
        {
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }

            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (y.Length != x.Length || y.Length != result.Length)
            {
                throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            CommonParallel.For(0, y.Length, index => { result[index] = x[index] + y[index]; });
        }

        /// <summary>
        /// Does a point wise subtraction of two arrays <c>z = x - y</c>. This can be used 
        /// to subtract vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the subtraction.</param>
        /// <remarks>There is no equivalent BLAS routine, but many libraries
        /// provide optimized (parallel and/or vectorized) versions of this
        /// routine.</remarks>
        public virtual void SubtractArrays(double[] x, double[] y, double[] result)
        {
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }

            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (y.Length != x.Length || y.Length != result.Length)
            {
                throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            CommonParallel.For(0, y.Length, index => { result[index] = x[index] - y[index]; });
        }

        /// <summary>
        /// Does a point wise multiplication of two arrays <c>z = x * y</c>. This can be used
        /// to multiple elements of vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the point wise multiplication.</param>
        /// <remarks>There is no equivalent BLAS routine, but many libraries
        /// provide optimized (parallel and/or vectorized) versions of this
        /// routine.</remarks>
        public virtual void PointWiseMultiplyArrays(double[] x, double[] y, double[] result)
        {
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }

            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (y.Length != x.Length || y.Length != result.Length)
            {
                throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            CommonParallel.For(0, y.Length, index => { result[index] = x[index] * y[index]; });
        }

        /// <summary>
        /// Does a point wise division of two arrays <c>z = x / y</c>. This can be used
        /// to divide elements of vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the point wise division.</param>
        /// <remarks>There is no equivalent BLAS routine, but many libraries
        /// provide optimized (parallel and/or vectorized) versions of this
        /// routine.</remarks>
        public virtual void PointWiseDivideArrays(double[] x, double[] y, double[] result)
        {
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }

            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (y.Length != x.Length || y.Length != result.Length)
            {
                throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            CommonParallel.For(0, y.Length, index => { result[index] = x[index] / y[index]; });
        }

        /// <summary>
        /// Computes the requested <see cref="Norm"/> of the matrix.
        /// </summary>
        /// <param name="norm">The type of norm to compute.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="matrix">The matrix to compute the norm from.</param>
        /// <returns>
        /// The requested <see cref="Norm"/> of the matrix.
        /// </returns>
        public virtual double MatrixNorm(Norm norm, int rows, int columns, double[] matrix)
        {
            var ret = 0.0;
            switch (norm)
            {
                case Norm.OneNorm:
                    for (var j = 0; j < columns; j++)
                    {
                        var s = 0.0;
                        for (var i = 0; i < rows; i++)
                        {
                            s += Math.Abs(matrix[(j * rows) + i]);
                        }

                        ret = Math.Max(ret, s);
                    }

                    break;
                case Norm.LargestAbsoluteValue:
                    
                    for (var i = 0; i < rows; i++)
                    {
                        for (var j = 0; j < columns; j++)
                        {
                            ret = Math.Max(Math.Abs(matrix[(j * rows) + i]), ret);
                        }
                    }

                    break;
                case Norm.InfinityNorm:
                    for (var i = 0; i < rows; i++)
                    {
                        var s = 0.0;
                        for (var j = 0; j < columns; j++)
                        {
                            s += Math.Abs(matrix[(j * rows) + i]);
                        }

                        ret = Math.Max(ret, s);
                    }

                    break;
                case Norm.FrobeniusNorm:
                    var aat = new double[rows * rows];
                    MatrixMultiplyWithUpdate(Transpose.DontTranspose, Transpose.Transpose, 1.0, matrix, rows, columns, matrix, rows, columns, 0.0, aat);

                    for (var i = 0; i < rows; i++)
                    {
                        ret += Math.Abs(aat[(i * rows) + i]);
                    }

                    ret = Math.Sqrt(ret);
                    break;
            }

            return ret;
        }

        /// <summary>
        /// Computes the requested <see cref="Norm"/> of the matrix.
        /// </summary>
        /// <param name="norm">The type of norm to compute.</param>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="matrix">The matrix to compute the norm from.</param>
        /// <param name="work">The work array. Not used in the managed provider.</param>
        /// <returns>
        /// The requested <see cref="Norm"/> of the matrix.
        /// </returns>
        public virtual double MatrixNorm(Norm norm, int rows, int columns, double[] matrix, double[] work)
        {
            return MatrixNorm(norm, rows, columns, matrix);
        }

        /// <summary>
        /// Multiples two matrices. <c>result = x * y</c>
        /// </summary>
        /// <param name="x">The x matrix.</param>
        /// <param name="rowsX">The number of rows in the x matrix.</param>
        /// <param name="columnsX">The number of columns in the x matrix.</param>
        /// <param name="y">The y matrix.</param>
        /// <param name="rowsY">The number of rows in the y matrix.</param>
        /// <param name="columnsY">The number of columns in the y matrix.</param>
        /// <param name="result">Where to store the result of the multiplication.</param>
        /// <remarks>This is a simplified version of the BLAS GEMM routine with alpha
        /// set to 1.0 and beta set to 0.0, and x and y are not transposed.</remarks>
        public virtual void MatrixMultiply(double[] x, int rowsX, int columnsX, double[] y, int rowsY, int columnsY, double[] result)
        {
            // First check some basic requirement on the parameters of the matrix multiplication.
            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (y == null)
            {
                throw new ArgumentNullException("y");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (rowsX * columnsX != x.Length)
            {
                throw new ArgumentException("x.Length != xRows * xColumns");
            }

            if (rowsY * columnsY != y.Length)
            {
                throw new ArgumentException("y.Length != yRows * yColumns");
            }

            if (columnsX != rowsY)
            {
                throw new ArgumentException("xColumns != yRows");
            }

            if (rowsX * columnsY != result.Length)
            {
                throw new ArgumentException("xRows * yColumns != result.Length");
            }

            // Check whether we will be overwriting any of our inputs and make copies if necessary.
            // TODO - we can don't have to allocate a completely new matrix when x or y point to the same memory
            // as result, we can do it on a row wise basis. We should investigate this.
            double[] xdata;
            if (ReferenceEquals(x, result))
            {
                xdata = (double[])x.Clone();
            }
            else
            {
                xdata = x;
            }

            double[] ydata;
            if (ReferenceEquals(y, result))
            {
                ydata = (double[])y.Clone();
            }
            else
            {
                ydata = y;
            }

            // Start the actual matrix multiplication.
            // TODO - For small matrices we should get rid of the parallelism because of startup costs.
            // Perhaps the following implementations would be a good one
            // http://blog.feradz.com/2009/01/cache-efficient-matrix-multiplication/
            MatrixMultiplyWithUpdate(Transpose.DontTranspose, Transpose.DontTranspose, 1.0, xdata, rowsX, columnsX, ydata, rowsY, columnsY, 0.0, result);
        }

        /// <summary>
        /// Multiplies two matrices and updates another with the result. <c>c = alpha*op(a)*op(b) + beta*c</c>
        /// </summary>
        /// <param name="transposeA">How to transpose the <paramref name="a"/> matrix.</param>
        /// <param name="transposeB">How to transpose the <paramref name="b"/> matrix.</param>
        /// <param name="alpha">The value to scale <paramref name="a"/> matrix.</param>
        /// <param name="a">The a matrix.</param>
        /// <param name="rowsA">The number of rows in the <paramref name="a"/> matrix.</param>
        /// <param name="columnsA">The number of columns in the <paramref name="a"/> matrix.</param>
        /// <param name="b">The b matrix</param>
        /// <param name="rowsB">The number of rows in the <paramref name="b"/> matrix.</param>
        /// <param name="columnsB">The number of columns in the <paramref name="b"/> matrix.</param>
        /// <param name="beta">The value to scale the <paramref name="c"/> matrix.</param>
        /// <param name="c">The c matrix.</param>
        public virtual void MatrixMultiplyWithUpdate(Transpose transposeA, Transpose transposeB, double alpha, double[] a, int rowsA, int columnsA, double[] b, int rowsB, int columnsB, double beta, double[] c)
        {
            // Choose nonsensical values for the number of rows in c; fill them in depending
            // on the operations on a and b.
            int rowsC;

            // First check some basic requirement on the parameters of the matrix multiplication.
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            if ((int)transposeA > 111 && (int)transposeB > 111)
            {
                if (rowsA != columnsB)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (columnsA * rowsB != c.Length)
                {
                    throw new ArgumentOutOfRangeException();
                }

                rowsC = columnsA;
            }
            else if ((int)transposeA > 111)
            {
                if (rowsA != rowsB)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (columnsA * columnsB != c.Length)
                {
                    throw new ArgumentOutOfRangeException();
                }

                rowsC = columnsA;
            }
            else if ((int)transposeB > 111)
            {
                if (columnsA != columnsB)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (rowsA * rowsB != c.Length)
                {
                    throw new ArgumentOutOfRangeException();
                }

                rowsC = rowsA;
            }
            else
            {
                if (columnsA != rowsB)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (rowsA * columnsB != c.Length)
                {
                    throw new ArgumentOutOfRangeException();
                }

                rowsC = rowsA;
            }

            if (alpha == 0.0 && beta == 0.0)
            {
                Array.Clear(c, 0, c.Length);
                return;
            }

            // Check whether we will be overwriting any of our inputs and make copies if necessary.
            // TODO - we can don't have to allocate a completely new matrix when x or y point to the same memory
            // as result, we can do it on a row wise basis. We should investigate this.
            double[] adata;
            if (ReferenceEquals(a, c))
            {
                adata = (double[])a.Clone();
            }
            else
            {
                adata = a;
            }

            double[] bdata;
            if (ReferenceEquals(b, c))
            {
                bdata = (double[])b.Clone();
            }
            else
            {
                bdata = b;
            }

            if (alpha == 1.0)
            {
                if (beta == 0.0)
                {
                    if ((int)transposeA > 111 && (int)transposeB > 111)
                    {
                        CommonParallel.For(
                            0,
                            columnsA,
                            j =>
                            {
                                var jIndex = j * rowsC;
                                for (var i = 0; i != rowsB; i++)
                                {
                                    var iIndex = i * rowsA;
                                    double s = 0;
                                    for (var l = 0; l != columnsB; l++)
                                    {
                                        s += adata[iIndex + l] * bdata[(l * rowsB) + j];
                                    }

                                    c[jIndex + i] = s;
                                }
                            });
                    }
                    else if ((int)transposeA > 111)
                    {
                        CommonParallel.For(
                            0, 
                                           columnsB, 
                                           j =>
                                           {
                                               var jcIndex = j * rowsC;
                                               var jbIndex = j * rowsB;
                                               for (var i = 0; i != columnsA; i++)
                                               {
                                                   var iIndex = i * rowsA;
                                                   double s = 0;
                                                   for (var l = 0; l != rowsA; l++)
                                                   {
                                                       s += adata[iIndex + l] * bdata[jbIndex + l];
                                                   }

                                                   c[jcIndex + i] = s;
                                               }
                                           });
                    }
                    else if ((int)transposeB > 111)
                    {
                        CommonParallel.For(
                            0,
                            rowsB,
                            j =>
                            {
                                var jIndex = j * rowsC;
                                for (var i = 0; i != rowsA; i++)
                                {
                                    double s = 0;
                                    for (var l = 0; l != columnsA; l++)
                                    {
                                        s += adata[(l * rowsA) + i] * bdata[(l * rowsB) + j];
                                    }

                                    c[jIndex + i] = s;
                                }
                            });
                    }
                    else
                    {
                        CommonParallel.For(
                            0,
                            columnsB,
                            j =>
                            {
                                var jcIndex = j * rowsC;
                                var jbIndex = j * rowsB;
                                for (var i = 0; i != rowsA; i++)
                                {
                                    double s = 0;
                                    for (var l = 0; l != columnsA; l++)
                                    {
                                        s += adata[(l * rowsA) + i] * bdata[jbIndex + l];
                                    }

                                    c[jcIndex + i] = s;
                                }
                            });
                    }
                }
                else
                {
                    if ((int)transposeA > 111 && (int)transposeB > 111)
                    {
                        CommonParallel.For(
                            0,
                            columnsA,
                            j =>
                            {
                                var jIndex = j * rowsC;
                                for (var i = 0; i != rowsB; i++)
                                {
                                    var iIndex = i * rowsA;
                                    double s = 0;
                                    for (var l = 0; l != columnsB; l++)
                                    {
                                        s += adata[iIndex + l] * bdata[(l * rowsB) + j];
                                    }

                                    c[jIndex + i] = (c[jIndex + i] * beta) + s;
                                }
                            });
                    }
                    else if ((int)transposeA > 111)
                    {
                        CommonParallel.For(
                            0,
                            columnsB,
                            j =>
                            {
                                var jcIndex = j * rowsC;
                                var jbIndex = j * rowsB;
                                for (var i = 0; i != columnsA; i++)
                                {
                                    var iIndex = i * rowsA;
                                    double s = 0;
                                    for (var l = 0; l != rowsA; l++)
                                    {
                                        s += adata[iIndex + l] * bdata[jbIndex + l];
                                    }

                                    c[jcIndex + i] = s + (c[jcIndex + i] * beta);
                                }
                            });
                    }
                    else if ((int)transposeB > 111)
                    {
                        CommonParallel.For(
                            0,
                            rowsB,
                            j =>
                            {
                                var jIndex = j * rowsC;
                                for (var i = 0; i != rowsA; i++)
                                {
                                    double s = 0;
                                    for (var l = 0; l != columnsA; l++)
                                    {
                                        s += adata[(l * rowsA) + i] * bdata[(l * rowsB) + j];
                                    }

                                    c[jIndex + i] = s + (c[jIndex + i] * beta);
                                }
                            });
                    }
                    else
                    {
                        CommonParallel.For(
                            0,
                            columnsB,
                            j =>
                            {
                                var jcIndex = j * rowsC;
                                var jbIndex = j * rowsB;
                                for (var i = 0; i != rowsA; i++)
                                {
                                    double s = 0;
                                    for (var l = 0; l != columnsA; l++)
                                    {
                                        s += adata[(l * rowsA) + i] * bdata[jbIndex + l];
                                    }

                                    c[jcIndex + i] = s + (c[jcIndex + i] * beta);
                                }
                            });
                    }
                }
            }
            else
            {
                if ((int)transposeA > 111 && (int)transposeB > 111)
                {
                    CommonParallel.For(
                        0,
                        columnsA,
                        j =>
                        {
                            var jIndex = j * rowsC;
                            for (var i = 0; i != rowsB; i++)
                            {
                                var iIndex = i * rowsA;
                                double s = 0;
                                for (var l = 0; l != columnsB; l++)
                                {
                                    s += adata[iIndex + l] * bdata[(l * rowsB) + j];
                                }

                                c[jIndex + i] = (c[jIndex + i] * beta) + (alpha * s);
                            }
                        });
                }
                else if ((int)transposeA > 111)
                {
                    CommonParallel.For(
                        0,
                        columnsB,
                        j =>
                        {
                            var jcIndex = j * rowsC;
                            var jbIndex = j * rowsB;
                            for (var i = 0; i != columnsA; i++)
                            {
                                var iIndex = i * rowsA;
                                double s = 0;
                                for (var l = 0; l != rowsA; l++)
                                {
                                    s += adata[iIndex + l] * bdata[jbIndex + l];
                                }

                                c[jcIndex + i] = (alpha * s) + (c[jcIndex + i] * beta);
                            }
                        });
                }
                else if ((int)transposeB > 111)
                {
                    CommonParallel.For(
                        0,
                        rowsB,
                        j =>
                        {
                            var jIndex = j * rowsC;
                            for (var i = 0; i != rowsA; i++)
                            {
                                double s = 0;
                                for (var l = 0; l != columnsA; l++)
                                {
                                    s += adata[(l * rowsA) + i] * bdata[(l * rowsB) + j];
                                }

                                c[jIndex + i] = (alpha * s) + (c[jIndex + i] * beta);
                            }
                        });
                }
                else
                {
                    CommonParallel.For(
                        0,
                        columnsB,
                        j =>
                        {
                            var jcIndex = j * rowsC;
                            var jbIndex = j * rowsB;
                            for (var i = 0; i != rowsA; i++)
                            {
                                double s = 0;
                                for (var l = 0; l != columnsA; l++)
                                {
                                    s += adata[(l * rowsA) + i] * bdata[jbIndex + l];
                                }

                                c[jcIndex + i] = (alpha * s) + (c[jcIndex + i] * beta);
                            }
                        });
                }
            }
        }

        /// <summary>
        /// Computes the LUP factorization of A. P*A = L*U.
        /// </summary>
        /// <param name="data">An <paramref name="order"/> by <paramref name="order"/> matrix. The matrix is overwritten with the
        /// the LU factorization on exit. The lower triangular factor L is stored in under the diagonal of <paramref name="data"/> (the diagonal is always 1.0
        /// for the L factor). The upper triangular factor U is stored on and above the diagonal of <paramref name="data"/>.</param>
        /// <param name="order">The order of the square matrix <paramref name="data"/>.</param>
        /// <param name="ipiv">On exit, it contains the pivot indices. The size of the array must be <paramref name="order"/>.</param>
        /// <remarks>This is equivalent to the GETRF LAPACK routine.</remarks>
        public virtual void LUFactor(double[] data, int order, int[] ipiv)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            if (ipiv == null)
            {
                throw new ArgumentNullException("ipiv");
            }

            if (data.Length != order * order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "data");
            }

            if (ipiv.Length != order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "ipiv");
            }

            // Initialize the pivot matrix to the identity permutation.
            for (var i = 0; i < order; i++)
            {
                ipiv[i] = i;
            }

            var vecLUcolj = new double[order];

            // Outer loop.
            for (var j = 0; j < order; j++)
            {
                var indexj = j * order;
                var indexjj = indexj + j;

                // Make a copy of the j-th column to localize references.
                for (var i = 0; i < order; i++)
                {
                    vecLUcolj[i] = data[indexj + i];
                }

                // Apply previous transformations.
                for (var i = 0; i < order; i++)
                {
                    // Most of the time is spent in the following dot product.
                    var kmax = Math.Min(i, j);
                    var s = 0.0;
                    for (var k = 0; k < kmax; k++)
                    {
                        s += data[(k * order) + i] * vecLUcolj[k];
                    }

                    data[indexj + i] = vecLUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                var p = j;
                for (var i = j + 1; i < order; i++)
                {
                    if (Math.Abs(vecLUcolj[i]) > Math.Abs(vecLUcolj[p]))
                    {
                        p = i;
                    }
                }

                if (p != j)
                {
                    for (var k = 0; k < order; k++)
                    {
                        var indexk = k * order;
                        var indexkp = indexk + p;
                        var indexkj = indexk + j;
                        var temp = data[indexkp];
                        data[indexkp] = data[indexkj];
                        data[indexkj] = temp;
                    }

                    ipiv[j] = p;
                }

                // Compute multipliers.
                if (j < order & data[indexjj] != 0.0)
                {
                    for (var i = j + 1; i < order; i++)
                    {
                        data[indexj + i] /= data[indexjj];
                    }
                }
            }
        }

        /// <summary>
        /// Computes the inverse of matrix using LU factorization.
        /// </summary>
        /// <param name="a">The N by N matrix to invert. Contains the inverse On exit.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <remarks>This is equivalent to the GETRF and GETRI LAPACK routines.</remarks>
        public virtual void LUInverse(double[] a, int order)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (a.Length != order * order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "a");
            }

            var ipiv = new int[order];
            LUFactor(a, order, ipiv);
            LUInverseFactored(a, order, ipiv);
        }

        /// <summary>
        /// Computes the inverse of a previously factored matrix.
        /// </summary>
        /// <param name="a">The LU factored N by N matrix.  Contains the inverse On exit.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="ipiv">The pivot indices of <paramref name="a"/>.</param>
        /// <remarks>This is equivalent to the GETRI LAPACK routine.</remarks>
        public virtual void LUInverseFactored(double[] a, int order, int[] ipiv)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (ipiv == null)
            {
                throw new ArgumentNullException("ipiv");
            }

            if (a.Length != order * order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "a");
            }

            if (ipiv.Length != order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "ipiv");
            }

            var inverse = new double[a.Length];
            for (var i = 0; i < order; i++)
            {
                inverse[i + (order * i)] = 1.0;
            }

            LUSolveFactored(order, a, order, ipiv, inverse);
            Buffer.BlockCopy(inverse, 0, a, 0, a.Length * Constants.SizeOfDouble);
        }

        /// <summary>
        /// Computes the inverse of matrix using LU factorization.
        /// </summary>
        /// <param name="a">The N by N matrix to invert. Contains the inverse On exit.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="work">The work array. The array must have a length of at least N,
        /// but should be N*blocksize. The blocksize is machine dependent. On exit, work[0] contains the optimal
        /// work size value.</param>
        /// <remarks>This is equivalent to the GETRF and GETRI LAPACK routines.</remarks>
        public virtual void LUInverse(double[] a, int order, double[] work)
        {
            LUInverse(a, order);
        }

        /// <summary>
        /// Computes the inverse of a previously factored matrix.
        /// </summary>
        /// <param name="a">The LU factored N by N matrix.  Contains the inverse On exit.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="ipiv">The pivot indices of <paramref name="a"/>.</param>
        /// <param name="work">The work array. The array must have a length of at least N,
        /// but should be N*blocksize. The blocksize is machine dependent.  On exit, work[0] contains the optimal
        /// work size value.</param>
        /// <remarks>This is equivalent to the GETRI LAPACK routine.</remarks>
        public virtual void LUInverseFactored(double[] a, int order, int[] ipiv, double[] work)
        {
           LUInverseFactored(a, order, ipiv);
        }

        /// <summary>
        /// Solves A*X=B for X using LU factorization.
        /// </summary>
        /// <param name="columnsOfB">The number of columns of B.</param>
        /// <param name="a">The square matrix A.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="b">The B matrix.</param>
        /// <remarks>This is equivalent to the GETRF and GETRS LAPACK routines.</remarks>
        public virtual void LUSolve(int columnsOfB, double[] a, int order, double[] b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            if (a.Length != order * order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "a");
            }

            if (b.Length != order * columnsOfB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            var ipiv = new int[order];
            LUFactor(a, order, ipiv);
            LUSolveFactored(columnsOfB, a, order, ipiv, b);
        }

        /// <summary>
        /// Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="columnsOfB">The number of columns of B.</param>
        /// <param name="a">The factored A matrix.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="ipiv">The pivot indices of <paramref name="a"/>.</param>
        /// <param name="b">The B matrix.</param>
        /// <remarks>This is equivalent to the GETRS LAPACK routine.</remarks>
        public virtual void LUSolveFactored(int columnsOfB, double[] a, int order, int[] ipiv, double[] b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (ipiv == null)
            {
                throw new ArgumentNullException("ipiv");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            if (a.Length != order * order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "a");
            }

            if (ipiv.Length != order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "ipiv");
            }

            if (b.Length != order * columnsOfB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            // Compute the column vector  P*B
            for (var i = 0; i < ipiv.Length; i++)
            {
                if (ipiv[i] == i)
                {
                    continue;
                }

                var p = ipiv[i];
                for (var j = 0; j < columnsOfB; j++)
                {
                    var indexk = j * order;
                    var indexkp = indexk + p;
                    var indexkj = indexk + i;
                    var temp = b[indexkp];
                    b[indexkp] = b[indexkj];
                    b[indexkj] = temp;
                }
            }

            // Solve L*Y = P*B
            for (var k = 0; k < order; k++)
            {
                var korder = k * order;
                for (var i = k + 1; i < order; i++)
                {
                    for (var j = 0; j < columnsOfB; j++)
                    {
                        var index = j * order;
                        b[i + index] -= b[k + index] * a[i + korder];
                    }
                }
            }

            // Solve U*X = Y;
            for (var k = order - 1; k >= 0; k--)
            {
                var korder = k + (k * order);
                for (var j = 0; j < columnsOfB; j++)
                {
                    b[k + (j * order)] /= a[korder];
                }

                korder = k * order;
                for (var i = 0; i < k; i++)
                {
                    for (var j = 0; j < columnsOfB; j++)
                    {
                        var index = j * order;
                        b[i + index] -= b[k + index] * a[i + korder];
                    }
                }
            }
        }

        /// <summary>
        /// Solves A*X=B for X using LU factorization.
        /// </summary>
        /// <param name="transposeA">How to transpose the <paramref name="a"/> matrix.</param>
        /// <param name="columnsOfB">The number of columns of B.</param>
        /// <param name="a">The square matrix A.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="b">The B matrix.</param>
        /// <remarks>This is equivalent to the GETRF and GETRS LAPACK routines.</remarks>
        public virtual void LUSolve(Transpose transposeA, int columnsOfB, double[] a, int order, double[] b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            if (a.Length != order * order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "a");
            }

            if (b.Length != order * columnsOfB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            var ipiv = new int[order];
            LUFactor(a, order, ipiv);
            LUSolveFactored(transposeA, columnsOfB, a, order, ipiv, b);
        }

        /// <summary>
        /// Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="transposeA">How to transpose the <paramref name="a"/> matrix.</param>
        /// <param name="columnsOfB">The number of columns of B.</param>
        /// <param name="a">The factored A matrix.</param>
        /// <param name="order">The order of the square matrix <paramref name="a"/>.</param>
        /// <param name="ipiv">The pivot indices of <paramref name="a"/>.</param>
        /// <param name="b">The B matrix.</param>
        /// <remarks>This is equivalent to the GETRS LAPACK routine.</remarks>
        public virtual void LUSolveFactored(Transpose transposeA, int columnsOfB, double[] a, int order, int[] ipiv, double[] b)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (ipiv == null)
            {
                throw new ArgumentNullException("ipiv");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            if (a.Length != order * order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "a");
            }

            if (ipiv.Length != order)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "ipiv");
            }

            if (b.Length != order * columnsOfB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            if ((transposeA == Transpose.Transpose) || (transposeA == Transpose.ConjugateTranspose))
            {
                var aT = new double[a.Length];
                for (var i = 0; i < order; i++)
                {
                    for (var j = 0; j < order; j++)
                    {
                        aT[(j * order) + i] = a[(i * order) + j];
                    }
                }

                LUSolveFactored(columnsOfB, aT, order, ipiv, b);
            }
            else
            {
                LUSolveFactored(columnsOfB, a, order, ipiv, b);
            }
        }

        /// <summary>
        /// Computes the Cholesky factorization of A.
        /// </summary>
        /// <param name="a">On entry, a square, positive definite matrix. On exit, the matrix is overwritten with the
        /// the Cholesky factorization.</param>
        /// <param name="order">The number of rows or columns in the matrix.</param>
        /// <remarks>This is equivalent to the POTRF LAPACK routine.</remarks>
        public virtual void CholeskyFactor(double[] a, int order)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            var tmpColumn = new double[order];

            // Main loop - along the diagonal
            for (int ij = 0; ij < order; ij++)
            {
                // "Pivot" element
                double tmpVal = a[(ij * order) + ij];

                if (tmpVal > 0.0)
                {
                    tmpVal = Math.Sqrt(tmpVal);
                    a[(ij * order) + ij] = tmpVal;
                    tmpColumn[ij] = tmpVal;

                    // Calculate multipliers and copy to local column
                    // Current column, below the diagonal
                    for (int i = ij + 1; i < order; i++)
                    {
                        a[(ij * order) + i] /= tmpVal;
                        tmpColumn[i] = a[(ij * order) + i];
                    }

                    // Remaining columns, below the diagonal
                    DoCholeskyStep(a, order, ij + 1, order, tmpColumn, Control.NumberOfParallelWorkerThreads);
                }
                else
                {
                    throw new ArgumentException(Resources.ArgumentMatrixPositiveDefinite);
                }

                for (int i = ij + 1; i < order; i++)
                {
                    a[(i * order) + ij] = 0.0;
                }
            }
        }

        /// <summary>
        /// Calculate Cholesky step
        /// </summary>
        /// <param name="data">Factor matrix</param>
        /// <param name="rowDim">Number of rows</param>
        /// <param name="firstCol">Column start</param>
        /// <param name="colLimit">Total columns</param>
        /// <param name="multipliers">Multipliers calculated previously</param>
        /// <param name="availableCores">Number of available processors</param>
        private static void DoCholeskyStep(double[] data, int rowDim, int firstCol, int colLimit, double[] multipliers, int availableCores)
        {
            var tmpColCount = colLimit - firstCol;

            if ((availableCores > 1) && (tmpColCount > 200))
            {
                var tmpSplit = firstCol + (tmpColCount / 3);
                var tmpCores = availableCores / 2;

                CommonParallel.Invoke(
                    () => DoCholeskyStep(data, rowDim, firstCol, tmpSplit, multipliers, tmpCores),
                    () => DoCholeskyStep(data, rowDim, tmpSplit, colLimit, multipliers, tmpCores));
            }
            else
            {
                for (var j = firstCol; j < colLimit; j++)
                {
                    var tmpVal = multipliers[j];
                    for (var i = j; i < rowDim; i++)
                    {
                        data[(j * rowDim) + i] -= multipliers[i] * tmpVal;
                    }
                }
            }
        }

        /// <summary>
        /// Solves A*X=B for X using Cholesky factorization.
        /// </summary>
        /// <param name="a">The square, positive definite matrix A.</param>
        /// <param name="orderA">The number of rows and columns in A.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="rowsB">The number of rows in the B matrix.</param>
        /// <param name="columnsB">The number of columns in the B matrix.</param>
        /// <remarks>This is equivalent to the POTRF add POTRS LAPACK routines.</remarks>
        public virtual void CholeskySolve(double[] a, int orderA, double[] b, int rowsB, int columnsB)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            if (orderA != rowsB)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions);
            }

            if (ReferenceEquals(a, b))
            {
                throw new ArgumentException(Resources.ArgumentReferenceDifferent);
            }

            CholeskyFactor(a, orderA);
            CholeskySolveFactored(a, orderA, b, rowsB, columnsB);
        }

        /// <summary>
        /// Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="a">The square, positive definite matrix A. Has to be different than <paramref name="b"/>.</param>
        /// <param name="orderA">The number of rows and columns in A.</param>
        /// <param name="b">The B matrix. Has to be different than <paramref name="a"/>.</param>
        /// <param name="rowsB">The number of rows in the B matrix.</param>
        /// <param name="columnsB">The number of columns in the B matrix.</param>
        /// <remarks>This is equivalent to the POTRS LAPACK routine.</remarks>
        public virtual void CholeskySolveFactored(double[] a, int orderA, double[] b, int rowsB, int columnsB)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            if (orderA != rowsB)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions);
            }

            if (ReferenceEquals(a, b))
            {
                throw new ArgumentException(Resources.ArgumentReferenceDifferent);
            }

            CommonParallel.For(
                0,
                columnsB,
                c =>
                {
                    var cindex = c * orderA;

                    // Solve L*Y = B;
                    double sum;
                    for (var i = 0; i < orderA; i++)
                    {
                        sum = b[cindex + i];
                        for (var k = i - 1; k >= 0; k--)
                        {
                            sum -= a[(k * orderA) + i] * b[cindex + k];
                        }

                        b[cindex + i] = sum / a[(i * orderA) + i];
                    }

                    // Solve L'*X = Y;
                    for (var i = orderA - 1; i >= 0; i--)
                    {
                        sum = b[cindex + i];
                        var iindex = i * orderA;
                        for (var k = i + 1; k < orderA; k++)
                        {
                            sum -= a[iindex + k] * b[cindex + k];
                        }

                        b[cindex + i] = sum / a[iindex + i];
                    }
                });
        }

        /// <summary>
        /// Computes the QR factorization of A.
        /// </summary>
        /// <param name="r">On entry, it is the M by N A matrix to factor. On exit,
        /// it is overwritten with the R matrix of the QR factorization. </param>
        /// <param name="rowsR">The number of rows in the A matrix.</param>
        /// <param name="columnsR">The number of columns in the A matrix.</param>
        /// <param name="q">On exit, A M by M matrix that holds the Q matrix of the
        /// QR factorization.</param>
        /// <remarks>This is similar to the GEQRF and ORGQR LAPACK routines.</remarks>
        public virtual void QRFactor(double[] r, int rowsR, int columnsR, double[] q)
        {
            if (r == null)
            {
                throw new ArgumentNullException("r");
            }

            if (q == null)
            {
                throw new ArgumentNullException("q");
            }

            if (r.Length != rowsR * columnsR)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "r");
            }

            if (q.Length != rowsR * rowsR)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "q");
            }

            var work = new double[rowsR * rowsR];
            QRFactor(r, rowsR, columnsR, q, work);
        }

        /// <summary>
        /// Computes the QR factorization of A.
        /// </summary>
        /// <param name="r">On entry, it is the M by N A matrix to factor. On exit,
        /// it is overwritten with the R matrix of the QR factorization. </param>
        /// <param name="rowsR">The number of rows in the A matrix.</param>
        /// <param name="columnsR">The number of columns in the A matrix.</param>
        /// <param name="q">On exit, A M by M matrix that holds the Q matrix of the 
        /// QR factorization.</param>
        /// <param name="work">The work array. The array must have a length of at least N,
        /// but should be N*blocksize. The blocksize is machine dependent. On exit, work[0] contains the optimal
        /// work size value.</param>
        /// <remarks>This is similar to the GEQRF and ORGQR LAPACK routines.</remarks>
        public virtual void QRFactor(double[] r, int rowsR, int columnsR, double[] q, double[] work)
        {
            if (r == null)
            {
                throw new ArgumentNullException("r");
            }

            if (q == null)
            {
                throw new ArgumentNullException("q");
            }

            if (work == null)
            {
                throw new ArgumentNullException("q");
            }

            if (r.Length != rowsR * columnsR)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "r");
            }

            if (q.Length != rowsR * rowsR)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "q");
            }

            if (work.Length < rowsR * rowsR)
            {
                work[0] = rowsR * rowsR;
                throw new ArgumentException(Resources.WorkArrayTooSmall, "work");
            }

            CommonParallel.For(0, rowsR, i => q[(i * rowsR) + i] = 1.0);

            var minmn = Math.Min(rowsR, columnsR);
            for (var i = 0; i < minmn; i++)
            {
                GenerateColumn(work, r, rowsR, i, i);
                ComputeQR(work, i, r, i, rowsR, i + 1, columnsR, Control.NumberOfParallelWorkerThreads);
            }

            for (var i = minmn - 1; i >= 0; i--)
            {
                ComputeQR(work, i, q, i, rowsR, i, rowsR, Control.NumberOfParallelWorkerThreads);
            }

            work[0] = rowsR * rowsR;
        }

        #region QR Factor Helper functions

        /// <summary>
        /// Perform calculation of Q or R
        /// </summary>
        /// <param name="work">Work array</param>
        /// <param name="workIndex">Index of column in work array</param>
        /// <param name="a">Q or R matrices</param>
        /// <param name="rowStart">The first row in </param>
        /// <param name="rowCount">The last row</param>
        /// <param name="columnStart">The first column</param>
        /// <param name="columnCount">The last column</param>
        /// <param name="availableCores">Number of available CPUs</param>
        private static void ComputeQR(double[] work, int workIndex, double[] a, int rowStart, int rowCount, int columnStart, int columnCount, int availableCores)
        {
            if (rowStart > rowCount || columnStart > columnCount)
            {
                return;
            }

            var tmpColCount = columnCount - columnStart;

            if ((availableCores > 1) && (tmpColCount > 200))
            {
                var tmpSplit = columnStart + (tmpColCount / 2);
                var tmpCores = availableCores / 2;

                CommonParallel.Invoke(
                    () => ComputeQR(work, workIndex, a, rowStart, rowCount, columnStart, tmpSplit, tmpCores),
                    () => ComputeQR(work, workIndex, a, rowStart, rowCount, tmpSplit, columnCount, tmpCores));
            }
            else
            {
                for (var j = columnStart; j < columnCount; j++)
                {
                    var scale = 0.0;
                    for (var i = rowStart; i < rowCount; i++)     
                    {
                        scale += work[(workIndex * rowCount) + i - rowStart] * a[(j * rowCount) + i];
                    }
                
                    for (var i = rowStart; i < rowCount; i++)    
                    {
                        a[(j * rowCount) + i] -= work[(workIndex * rowCount) + i - rowStart] * scale;
                    }
                }
            }
        }

        /// <summary>
        /// Generate column from initial matrix to work array
        /// </summary>
        /// <param name="work">Work array</param>
        /// <param name="a">Initial matrix</param>
        /// <param name="rowCount">The number of rows in matrix</param>
        /// <param name="row">The first row</param>
        /// <param name="column">Column index</param>
        private static void GenerateColumn(double[] work, double[] a, int rowCount, int row, int column)
        {
            var tmp = column * rowCount;
            var index = tmp + row;

            CommonParallel.For(
                row,
                rowCount,
                i =>
                {
                    var iIndex = tmp + i;
                    work[iIndex - row] = a[iIndex];
                    a[iIndex] = 0.0;
                });

            var norm = 0.0;
            for (var i = 0; i < rowCount - row; ++i)
            {
                var iindex = tmp + i;
                norm += work[iindex] * work[iindex];
            }

            norm = Math.Sqrt(norm);
            if (row == rowCount - 1  || norm == 0)
            {
                a[index] = -work[tmp];
                work[tmp] = Math.Sqrt(2.0);
                return;
            }

            var scale = 1.0 / norm;
            if (work[tmp] < 0.0)
            {
                scale *= -1.0;
            }

            a[index] = -1.0 / scale;
            CommonParallel.For(0, rowCount - row, i => work[tmp + i] *= scale);
            work[tmp] += 1.0;

            var s = Math.Sqrt(1.0 / work[tmp]);
            CommonParallel.For(0, rowCount - row, i => work[tmp + i] *= s);
        }

        #endregion

        /// <summary>
        /// Solves A*X=B for X using QR factorization of A.
        /// </summary>
        /// <param name="r">On entry, it is the M by N A matrix to factor. On exit,
        /// it is overwritten with the R matrix of the QR factorization. </param>
        /// <param name="rowsR">The number of rows in the A matrix.</param>
        /// <param name="columnsR">The number of columns in the A matrix.</param>
        /// <param name="q">On exit, A M by M matrix that holds the Q matrix of the 
        /// QR factorization.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        public virtual void QRSolve(double[] r, int rowsR, int columnsR, double[] q, double[] b, int columnsB, double[] x)
        {
            if (r == null)
            {
                throw new ArgumentNullException("r");
            }

            if (q == null)
            {
                throw new ArgumentNullException("q");
            }

            if (b == null)
            {
                throw new ArgumentNullException("q");
            }

            if (x == null)
            {
                throw new ArgumentNullException("q");
            }

            if (r.Length != rowsR * columnsR)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "r");
            }

            if (q.Length != rowsR * rowsR)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "q");
            }

            if (b.Length != rowsR * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            if (x.Length != columnsR * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "x");
            }

            var work = new double[rowsR * rowsR];
            QRSolve(r, rowsR, columnsR, q, b, columnsB, x, work);
        }

        /// <summary>
        /// Solves A*X=B for X using QR factorization of A.
        /// </summary>
        /// <param name="r">On entry, it is the M by N A matrix to factor. On exit,
        /// it is overwritten with the R matrix of the QR factorization. </param>
        /// <param name="rowsR">The number of rows in the A matrix.</param>
        /// <param name="columnsR">The number of columns in the A matrix.</param>
        /// <param name="q">On exit, A M by M matrix that holds the Q matrix of the 
        /// QR factorization.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        /// <param name="work">The work array. The array must have a length of at least N,
        /// but should be N*blocksize. The blocksize is machine dependent. On exit, work[0] contains the optimal
        /// work size value.</param>
        public virtual void QRSolve(double[] r, int rowsR, int columnsR, double[] q, double[] b, int columnsB, double[] x, double[] work)
        {
            if (r == null)
            {
                throw new ArgumentNullException("r");
            }

            if (q == null)
            {
                throw new ArgumentNullException("q");
            }

            if (b == null)
            {
                throw new ArgumentNullException("q");
            }

            if (x == null)
            {
                throw new ArgumentNullException("q");
            }

            if (r.Length != rowsR * columnsR)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "r");
            }

            if (q.Length != rowsR * rowsR)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "q");
            }

            if (b.Length != rowsR * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            if (x.Length != columnsR * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "x");
            }

            if (work.Length < rowsR * rowsR)
            {
                work[0] = rowsR * rowsR;
                throw new ArgumentException(Resources.WorkArrayTooSmall, "work");
            }

            QRFactor(r, rowsR, columnsR, q, work);
            QRSolveFactored(q, r, rowsR, columnsR, b, columnsB, x);

            work[0] = rowsR * rowsR;
        }

        /// <summary>
        /// Solves A*X=B for X using a previously QR factored matrix.
        /// </summary>
        /// <param name="q">The Q matrix obtained by calling <see cref="QRFactor(double[],int,int,double[])"/>.</param>
        /// <param name="r">The R matrix obtained by calling <see cref="QRFactor(double[],int,int,double[])"/>. </param>
        /// <param name="rowsR">The number of rows in the A matrix.</param>
        /// <param name="columnsR">The number of columns in the A matrix.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        public virtual void QRSolveFactored(double[] q, double[] r, int rowsR, int columnsR, double[] b, int columnsB, double[] x)
        {
            if (r == null)
            {
                throw new ArgumentNullException("r");
            }

            if (q == null)
            {
                throw new ArgumentNullException("q");
            }

            if (b == null)
            {
                throw new ArgumentNullException("q");
            }

            if (x == null)
            {
                throw new ArgumentNullException("q");
            }

            if (r.Length != rowsR * columnsR)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "r");
            }

            if (q.Length != rowsR * rowsR)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "q");
            }

            if (b.Length != rowsR * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            if (x.Length != columnsR * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "x");
            }

            var sol = new double[b.Length];

            // Copy B matrix to "sol", so B data will not be changed
            Buffer.BlockCopy(b, 0, sol, 0, b.Length * Constants.SizeOfDouble);

            // Compute Y = transpose(Q)*B
            var column = new double[rowsR];
            for (var j = 0; j < columnsB; j++)
            {
                var jm = j * rowsR;
                CommonParallel.For(0, rowsR, k => column[k] = sol[jm + k]);
                CommonParallel.For(
                    0, 
                    rowsR, 
                    i =>
                    {
                        var im = i * rowsR;
                        sol[jm + i] = CommonParallel.Aggregate(0, rowsR, k => q[im + k] * column[k]);
                    });
            }

            // Solve R*X = Y;
            for (var k = columnsR - 1; k >= 0; k--)
            {
                var km = k * rowsR;
                for (var j = 0; j < columnsB; j++)
                {
                    sol[(j * rowsR) + k] /= r[km + k];
                }

                for (var i = 0; i < k; i++)
                {
                    for (var j = 0; j < columnsB; j++)
                    {
                        var jm = j * rowsR;
                        sol[jm + i] -= sol[jm + k] * r[km + i];
                    }
                }
            }

            // Fill result matrix
            CommonParallel.For(
                0, 
                columnsR, 
                row =>
                {
                    for (var col = 0; col < columnsB; col++)
                    {
                        x[(col * columnsR) + row] = sol[row + (col * rowsR)];
                    }
                });
        }

        /// <summary>
        /// Computes the singular value decomposition of A.
        /// </summary>
        /// <param name="computeVectors">Compute the singular U and VT vectors or not.</param>
        /// <param name="a">On entry, the M by N matrix to decompose. On exit, A may be overwritten.</param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="s">The singular values of A in ascending value.</param>
        /// <param name="u">If <paramref name="computeVectors"/> is <c>true</c>, on exit U contains the left
        /// singular vectors.</param>
        /// <param name="vt">If <paramref name="computeVectors"/> is <c>true</c>, on exit VT contains the transposed
        /// right singular vectors.</param>
        /// <remarks>This is equivalent to the GESVD LAPACK routine.</remarks>
        public virtual void SingularValueDecomposition(bool computeVectors, double[] a, int rowsA, int columnsA, double[] s, double[] u, double[] vt)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            if (u == null)
            {
                throw new ArgumentNullException("u");
            }

            if (vt == null)
            {
                throw new ArgumentNullException("vt");
            }

            if (u.Length != rowsA * rowsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "u");
            }

            if (vt.Length != columnsA * columnsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "vt");
            }

            if (s.Length != Math.Min(rowsA, columnsA))
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "s");
            }

            // Actually "work = new double[aRows]" is acceptable size of work array. I set size proposed in method description
            var work = new double[Math.Max((3 * Math.Min(rowsA, columnsA)) + Math.Max(rowsA, columnsA), 5 * Math.Min(rowsA, columnsA))];
            SingularValueDecomposition(computeVectors, a, rowsA, columnsA, s, u, vt, work);
        }

        /// <summary>
        /// Computes the singular value decomposition of A.
        /// </summary>
        /// <param name="computeVectors">Compute the singular U and VT vectors or not.</param>
        /// <param name="a">On entry, the M by N matrix to decompose. On exit, A may be overwritten.</param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="s">The singular values of A in ascending value.</param>
        /// <param name="u">If <paramref name="computeVectors"/> is <c>true</c>, on exit U contains the left
        /// singular vectors.</param>
        /// <param name="vt">If <paramref name="computeVectors"/> is <c>true</c>, on exit VT contains the transposed
        /// right singular vectors.</param>
        /// <param name="work">The work array. For real matrices, the work array should be at least
        /// Max(3*Min(M, N) + Max(M, N), 5*Min(M,N)). For complex matrices, 2*Min(M, N) + Max(M, N).
        /// On exit, work[0] contains the optimal work size value.</param>
        /// <remarks>This is equivalent to the GESVD LAPACK routine.</remarks>
        public virtual void SingularValueDecomposition(bool computeVectors, double[] a, int rowsA, int columnsA, double[] s, double[] u, double[] vt, double[] work)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            if (u == null)
            {
                throw new ArgumentNullException("u");
            }

            if (vt == null)
            {
                throw new ArgumentNullException("vt");
            }

            if (work == null)
            {
                throw new ArgumentNullException("work");
            }

            if (u.Length != rowsA * rowsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "u");
            }

            if (vt.Length != columnsA * columnsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "vt");
            }

            if (s.Length != Math.Min(rowsA, columnsA))
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "s");
            }

            if (work.Length == 0)
            {
                throw new ArgumentException(Resources.ArgumentSingleDimensionArray, "work");
            }

            if (work.Length < rowsA)
            {
                work[0] = rowsA;
                throw new ArgumentException(Resources.WorkArrayTooSmall, "work");
            }

            const int Maxiter = 1000;

            var e = new double[columnsA];
            var v = new double[vt.Length];
            var stemp = new double[Math.Min(rowsA + 1, columnsA)];

            int i, j, l, lp1;

            var cs = 0.0;
            var sn = 0.0;
            double t;

            var ncu = rowsA;

            // Reduce matrix to bidiagonal form, storing the diagonal elements
            // in "s" and the super-diagonal elements in "e".
            var nct = Math.Min(rowsA - 1, columnsA);
            var nrt = Math.Max(0, Math.Min(columnsA - 2, rowsA));
            var lu = Math.Max(nct, nrt);

            for (l = 0; l < lu; l++)
            {
                lp1 = l + 1;
                if (l < nct)
                {
                    // Compute the transformation for the l-th column and
                    // place the l-th diagonal in vector s[l].
                    var l1 = l;
                    stemp[l] = Math.Sqrt(CommonParallel.Aggregate(l, rowsA, i1 => (a[(l1 * rowsA) + i1] * a[(l1 * rowsA) + i1])));

                    if (stemp[l] != 0.0)
                    {
                        if (a[(l * rowsA) + l] != 0.0)
                        {
                            stemp[l] = Math.Abs(stemp[l]) * (a[(l * rowsA) + l] / Math.Abs(a[(l * rowsA) + l]));
                        }

                        // A part of column "l" of Matrix A from row "l" to end multiply by 1.0 / s[l]
                        for (i = l; i < rowsA; i++)
                        {
                            a[(l * rowsA) + i] = a[(l * rowsA) + i] * (1.0 / stemp[l]);
                        }

                        a[(l * rowsA) + l] = 1.0 + a[(l * rowsA) + l];
                    }

                    stemp[l] = -stemp[l];
                }

                for (j = lp1; j < columnsA; j++)
                {
                    if (l < nct)
                    {
                        if (stemp[l] != 0.0)
                        {
                            // Apply the transformation.
                            t = 0.0;
                            for (i = l; i < rowsA; i++)
                            {
                                t += a[(j * rowsA) + i] * a[(l * rowsA) + i];
                            }

                            t = -t / a[(l * rowsA) + l];

                            for (var ii = l; ii < rowsA; ii++)
                            {
                                a[(j * rowsA) + ii] += t * a[(l * rowsA) + ii];
                            }
                        }
                    }

                    // Place the l-th row of matrix into "e" for the
                    // subsequent calculation of the row transformation.
                    e[j] = a[(j * rowsA) + l];
                }

                if (computeVectors && l < nct)
                {
                    // Place the transformation in "u" for subsequent back multiplication.
                    for (i = l; i < rowsA; i++)
                    {
                        u[(l * rowsA) + i] = a[(l * rowsA) + i];
                    }
                }

                if (l >= nrt)
                {
                    continue;
                }

                // Compute the l-th row transformation and place the l-th super-diagonal in e(l).
                var enorm = 0.0;
                for (i = lp1; i < e.Length; i++)
                {
                    enorm += e[i] * e[i];
                }

                e[l] = Math.Sqrt(enorm);
                if (e[l] != 0.0)
                {
                    if (e[lp1] != 0.0)
                    {
                        e[l] = Math.Abs(e[l]) * (e[lp1] / Math.Abs(e[lp1]));
                    }

                    // Scale vector "e" from "lp1" by 1.0 / e[l]
                    for (i = lp1; i < e.Length; i++)
                    {
                        e[i] = e[i] * (1.0 / e[l]);
                    }

                    e[lp1] = 1.0 + e[lp1];
                }

                e[l] = -e[l];

                if (lp1 < rowsA && e[l] != 0.0)
                {
                    // Apply the transformation.
                    for (i = lp1; i < rowsA; i++)
                    {
                        work[i] = 0.0;
                    }

                    for (j = lp1; j < columnsA; j++)
                    {
                        for (var ii = lp1; ii < rowsA; ii++)
                        {
                            work[ii] += e[j] * a[(j * rowsA) + ii];
                        }
                    }

                    for (j = lp1; j < columnsA; j++)
                    {
                        var ww = -e[j] / e[lp1];
                        for (var ii = lp1; ii < rowsA; ii++)
                        {
                            a[(j * rowsA) + ii] += ww * work[ii];
                        }
                    }
                }

                if (!computeVectors)
                {
                    continue;
                }

                // Place the transformation in v for subsequent back multiplication.
                for (i = lp1; i < columnsA; i++)
                {
                    v[(l * columnsA) + i] = e[i];
                }
            }

            // Set up the final bidiagonal matrix or order m.
            var m = Math.Min(columnsA, rowsA + 1);
            var nctp1 = nct + 1;
            var nrtp1 = nrt + 1;
            if (nct < columnsA)
            {
                stemp[nctp1 - 1] = a[((nctp1 - 1) * rowsA) + (nctp1 - 1)];
            }

            if (rowsA < m)
            {
                stemp[m - 1] = 0.0;
            }

            if (nrtp1 < m)
            {
                e[nrtp1 - 1] = a[((m - 1) * rowsA) + (nrtp1 - 1)];
            }

            e[m - 1] = 0.0;

            // If required, generate "u".
            if (computeVectors)
            {
                for (j = nctp1 - 1; j < ncu; j++)
                {
                    for (i = 0; i < rowsA; i++)
                    {
                        u[(j * rowsA) + i] = 0.0;
                    }

                    u[(j * rowsA) + j] = 1.0;
                }

                for (l = nct - 1; l >= 0; l--)
                {
                    if (stemp[l] != 0.0)
                    {
                        for (j = l + 1; j < ncu; j++)
                        {
                            t = 0.0;
                            for (i = l; i < rowsA; i++)
                            {
                                t += u[(j * rowsA) + i] * u[(l * rowsA) + i];
                            }

                            t = -t / u[(l * rowsA) + l];

                            for (var ii = l; ii < rowsA; ii++)
                            {
                                u[(j * rowsA) + ii] += t * u[(l * rowsA) + ii];
                            }
                        }

                        // A part of column "l" of matrix A from row "l" to end multiply by -1.0
                        for (i = l; i < rowsA; i++)
                        {
                            u[(l * rowsA) + i] = u[(l * rowsA) + i] * -1.0;
                        }

                        u[(l * rowsA) + l] = 1.0 + u[(l * rowsA) + l];
                        for (i = 0; i < l; i++)
                        {
                            u[(l * rowsA) + i] = 0.0;
                        }
                    }
                    else
                    {
                        for (i = 0; i < rowsA; i++)
                        {
                            u[(l * rowsA) + i] = 0.0;
                        }

                        u[(l * rowsA) + l] = 1.0;
                    }
                }
            }

            // If it is required, generate v.
            if (computeVectors)
            {
                for (l = columnsA - 1; l >= 0; l--)
                {
                    lp1 = l + 1;
                    if (l < nrt)
                    {
                        if (e[l] != 0.0)
                        {
                            for (j = lp1; j < columnsA; j++)
                            {
                                t = 0.0;
                                for (i = lp1; i < columnsA; i++)
                                {
                                    t += v[(j * columnsA) + i] * v[(l * columnsA) + i];
                                }

                                t = -t / v[(l * columnsA) + lp1];
                                for (var ii = l; ii < columnsA; ii++)
                                {
                                    v[(j * columnsA) + ii] += t * v[(l * columnsA) + ii];
                                }
                            }
                        }
                    }

                    for (i = 0; i < columnsA; i++)
                    {
                        v[(l * columnsA) + i] = 0.0;
                    }

                    v[(l * columnsA) + l] = 1.0;
                }
            }

            // Transform "s" and "e" so that they are double
            for (i = 0; i < m; i++)
            {
                double r;
                if (stemp[i] != 0.0)
                {
                    t = stemp[i];
                    r = stemp[i] / t;
                    stemp[i] = t;
                    if (i < m - 1)
                    {
                        e[i] = e[i] / r;
                    }

                    if (computeVectors)
                    {
                        // A part of column "i" of matrix U from row 0 to end multiply by r
                        for (j = 0; j < rowsA; j++)
                        {
                            u[(i * rowsA) + j] = u[(i * rowsA) + j] * r;
                        }
                    }
                }

                // Exit
                if (i == m - 1)
                {
                    break;
                }

                if (e[i] == 0.0)
                {
                    continue;
                }

                t = e[i];
                r = t / e[i];
                e[i] = t;
                stemp[i + 1] = stemp[i + 1] * r;
                if (!computeVectors)
                {
                    continue;
                }

                // A part of column "i+1" of matrix VT from row 0 to end multiply by r
                for (j = 0; j < columnsA; j++)
                {
                    v[((i + 1) * columnsA) + j] = v[((i + 1) * columnsA) + j] * r;
                }
            }

            // Main iteration loop for the singular values.
            var mn = m;
            var iter = 0;

            while (m > 0)
            {
                // Quit if all the singular values have been found.
                // If too many iterations have been performed throw exception.
                if (iter >= Maxiter)
                {
                    throw new ArgumentException(Resources.ConvergenceFailed);
                }

                // This section of the program inspects for negligible elements in the s and e arrays,  
                // on completion the variables kase and l are set as follows:
                // kase = 1: if mS[m] and e[l-1] are negligible and l < m
                // kase = 2: if mS[l] is negligible and l < m
                // kase = 3: if e[l-1] is negligible, l < m, and mS[l, ..., mS[m] are not negligible (qr step).
                // kase = 4: if e[m-1] is negligible (convergence).
                double ztest;
                double test;
                for (l = m - 2; l >= 0; l--)
                {
                    test = Math.Abs(stemp[l]) + Math.Abs(stemp[l + 1]);
                    ztest = test + Math.Abs(e[l]);
                    if (ztest.AlmostEqualInDecimalPlaces(test, 15))
                    {
                        e[l] = 0.0;
                        break;
                    }
                }

                int kase;
                if (l == m - 2)
                {
                    kase = 4;
                }
                else
                {
                    int ls;
                    for (ls = m - 1; ls > l; ls--)
                    {
                        test = 0.0;
                        if (ls != m - 1)
                        {
                            test = test + Math.Abs(e[ls]);
                        }

                        if (ls != l + 1)
                        {
                            test = test + Math.Abs(e[ls - 1]);
                        }

                        ztest = test + Math.Abs(stemp[ls]);
                        if (ztest.AlmostEqualInDecimalPlaces(test, 15))
                        {
                            stemp[ls] = 0.0;
                            break;
                        }
                    }

                    if (ls == l)
                    {
                        kase = 3;
                    }
                    else if (ls == m - 1)
                    {
                        kase = 1;
                    }
                    else
                    {
                        kase = 2;
                        l = ls;
                    }
                }

                l = l + 1;

                // Perform the task indicated by kase.
                int k;
                double f;
                switch (kase)
                {
                        // Deflate negligible s[m].
                    case 1:
                        f = e[m - 2];
                        e[m - 2] = 0.0;
                        double t1;
                        for (var kk = l; kk < m - 1; kk++)
                        {
                            k = m - 2 - kk + l;
                            t1 = stemp[k];

                            Drotg(ref t1, ref f, ref cs, ref sn);
                            stemp[k] = t1;
                            if (k != l)
                            {
                                f = -sn * e[k - 1];
                                e[k - 1] = cs * e[k - 1];
                            }

                            if (computeVectors)
                            {
                                // Rotate
                                for (i = 0; i < columnsA; i++)
                                {
                                    var z = (cs * v[(k * columnsA) + i]) + (sn * v[((m - 1) * columnsA) + i]);
                                    v[((m - 1) * columnsA) + i] = (cs * v[((m - 1) * columnsA) + i]) - (sn * v[(k * columnsA) + i]);
                                    v[(k * columnsA) + i] = z;
                                }
                            }
                        }

                        break;

                    // Split at negligible s[l].
                    case 2:
                        f = e[l - 1];
                        e[l - 1] = 0.0;
                        for (k = l; k < m; k++)
                        {
                            t1 = stemp[k];
                            Drotg(ref t1, ref f, ref cs, ref sn);
                            stemp[k] = t1;
                            f = -sn * e[k];
                            e[k] = cs * e[k];
                            if (computeVectors)
                            {
                                // Rotate
                                for (i = 0; i < rowsA; i++)
                                {
                                    var z = (cs * u[(k * rowsA) + i]) + (sn * u[((l - 1) * rowsA) + i]);
                                    u[((l - 1) * rowsA) + i] = (cs * u[((l - 1) * rowsA) + i]) - (sn * u[(k * rowsA) + i]);
                                    u[(k * rowsA) + i] = z;
                                }
                            }
                        }

                        break;

                        // Perform one qr step.
                    case 3:

                        // calculate the shift.
                        var scale = 0.0;
                        scale = Math.Max(scale, Math.Abs(stemp[m - 1]));
                        scale = Math.Max(scale, Math.Abs(stemp[m - 2]));
                        scale = Math.Max(scale, Math.Abs(e[m - 2]));
                        scale = Math.Max(scale, Math.Abs(stemp[l]));
                        scale = Math.Max(scale, Math.Abs(e[l]));
                        var sm = stemp[m - 1] / scale;
                        var smm1 = stemp[m - 2] / scale;
                        var emm1 = e[m - 2] / scale;
                        var sl = stemp[l] / scale;
                        var el = e[l] / scale;
                        var b = (((smm1 + sm) * (smm1 - sm)) + (emm1 * emm1)) / 2.0;
                        var c = (sm * emm1) * (sm * emm1);
                        var shift = 0.0;
                        if (b != 0.0 || c != 0.0)
                        {
                            shift = Math.Sqrt((b * b) + c);
                            if (b < 0.0)
                            {
                                shift = -shift;
                            }

                            shift = c / (b + shift);
                        }

                        f = ((sl + sm) * (sl - sm)) + shift;
                        var g = sl * el;

                        // Chase zeros
                        for (k = l; k < m - 1; k++)
                        {
                            Drotg(ref f, ref g, ref cs, ref sn);
                            if (k != l)
                            {
                                e[k - 1] = f;
                            }

                            f = (cs * stemp[k]) + (sn * e[k]);
                            e[k] = (cs * e[k]) - (sn * stemp[k]);
                            g = sn * stemp[k + 1];
                            stemp[k + 1] = cs * stemp[k + 1];
                            if (computeVectors)
                            {
                                for (i = 0; i < columnsA; i++)
                                {
                                    var z = (cs * v[(k * columnsA) + i]) + (sn * v[((k + 1) * columnsA) + i]);
                                    v[((k + 1) * columnsA) + i] = (cs * v[((k + 1) * columnsA) + i]) - (sn * v[(k * columnsA) + i]);
                                    v[(k * columnsA) + i] = z;
                                }
                            }

                            Drotg(ref f, ref g, ref cs, ref sn);
                            stemp[k] = f;
                            f = (cs * e[k]) + (sn * stemp[k + 1]);
                            stemp[k + 1] = -(sn * e[k]) + (cs * stemp[k + 1]);
                            g = sn * e[k + 1];
                            e[k + 1] = cs * e[k + 1];
                            if (computeVectors && k < rowsA)
                            {
                                for (i = 0; i < rowsA; i++)
                                {
                                    var z = (cs * u[(k * rowsA) + i]) + (sn * u[((k + 1) * rowsA) + i]);
                                    u[((k + 1) * rowsA) + i] = (cs * u[((k + 1) * rowsA) + i]) - (sn * u[(k * rowsA) + i]);
                                    u[(k * rowsA) + i] = z;
                                }
                            }
                        }

                        e[m - 2] = f;
                        iter = iter + 1;
                        break;

                        // Convergence
                    case 4:

                        // Make the singular value  positive
                        if (stemp[l] < 0.0)
                        {
                            stemp[l] = -stemp[l];
                            if (computeVectors)
                            {
                                // A part of column "l" of matrix VT from row 0 to end multiply by -1
                                for (i = 0; i < columnsA; i++)
                                {
                                    v[(l * columnsA) + i] = v[(l * columnsA) + i] * -1.0;
                                }
                            }
                        }

                        // Order the singular value.
                        while (l != mn - 1)
                        {
                            if (stemp[l] >= stemp[l + 1])
                            {
                                break;
                            }

                            t = stemp[l];
                            stemp[l] = stemp[l + 1];
                            stemp[l + 1] = t;
                            if (computeVectors && l < columnsA)
                            {
                                // Swap columns l, l + 1
                                for (i = 0; i < columnsA; i++)
                                {
                                    var z = v[(l * columnsA) + i];
                                    v[(l * columnsA) + i] = v[((l + 1) * columnsA) + i];
                                    v[((l + 1) * columnsA) + i] = z;
                                }
                            }

                            if (computeVectors && l < rowsA)
                            {
                                // Swap columns l, l + 1
                                for (i = 0; i < rowsA; i++)
                                {
                                    var z = u[(l * rowsA) + i];
                                    u[(l * rowsA) + i] = u[((l + 1) * rowsA) + i];
                                    u[((l + 1) * rowsA) + i] = z;
                                }
                            }

                            l = l + 1;
                        }

                        iter = 0;
                        m = m - 1;
                        break;
                }
            }

            if (computeVectors)
            {
                // Finally transpose "v" to get "vt" matrix 
                for (i = 0; i < columnsA; i++)
                {
                    for (j = 0; j < columnsA; j++)
                    {
                        vt[(j * columnsA) + i] = v[(i * columnsA) + j];
                    }
                }
            }

            // Copy stemp to s with size adjustment. We are using ported copy of linpack's svd code and it uses
            // a singular vector of length rows+1 when rows < columns. The last element is not used and needs to be removed.
            // We should port lapack's svd routine to remove this problem.
            Buffer.BlockCopy(stemp, 0, s, 0, Math.Min(rowsA, columnsA) * Constants.SizeOfDouble);

            // On return the first element of the work array stores the min size of the work array could have been
            // work[0] = Math.Max(3 * Math.Min(aRows, aColumns) + Math.Max(aRows, aColumns), 5 * Math.Min(aRows, aColumns));
            work[0] = rowsA;
        }

        /// <summary>
        /// Given the Cartesian coordinates (da, db) of a point p, these function return the parameters da, db, c, and s 
        /// associated with the Givens rotation that zeros the y-coordinate of the point.
        /// </summary>
        /// <param name="da">Provides the x-coordinate of the point p. On exit contains the parameter r associated with the Givens rotation</param>
        /// <param name="db">Provides the y-coordinate of the point p. On exit contains the parameter z associated with the Givens rotation</param>
        /// <param name="c">Contains the parameter c associated with the Givens rotation</param>
        /// <param name="s">Contains the parameter s associated with the Givens rotation</param>
        /// <remarks>This is equivalent to the DROTG LAPACK routine.</remarks>
        private static void Drotg(ref double da, ref double db, ref double c, ref double s)
        {
            double r, z;

            var roe = db;
            var absda = Math.Abs(da);
            var absdb = Math.Abs(db);
            if (absda > absdb)
            {
                roe = da;
            }

            var scale = absda + absdb;
            if (scale == 0.0)
            {
                c = 1.0;
                s = 0.0;
                r = 0.0;
                z = 0.0;
            }
            else
            {
                var sda = da / scale;
                var sdb = db / scale;
                r = scale * Math.Sqrt((sda * sda) + (sdb * sdb));
                if (roe < 0.0)
                {
                    r = -r;
                }

                c = da / r;
                s = db / r;
                z = 1.0;
                if (absda > absdb)
                {
                    z = s;
                }

                if (absdb >= absda && c != 0.0)
                {
                    z = 1.0 / c;
                }
            }

            da = r;
            db = z;
        }

        /// <summary>
        /// Solves A*X=B for X using the singular value decomposition of A.
        /// </summary>
        /// <param name="a">On entry, the M by N matrix to decompose. On exit, A may be overwritten.</param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="s">The singular values of A in ascending value.</param>
        /// <param name="u">On exit U contains the left singular vectors.</param>
        /// <param name="vt">On exit VT contains the transposed right singular vectors.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        public virtual void SvdSolve(double[] a, int rowsA, int columnsA, double[] s, double[] u, double[] vt, double[] b, int columnsB, double[] x)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            if (u == null)
            {
                throw new ArgumentNullException("u");
            }

            if (vt == null)
            {
                throw new ArgumentNullException("vt");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (u.Length != rowsA * rowsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "u");
            }

            if (vt.Length != columnsA * columnsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "vt");
            }

            if (s.Length != Math.Min(rowsA, columnsA))
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "s");
            }

            if (b.Length != rowsA * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            if (x.Length != columnsA * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            // Actually "work = new double[aRows]" is acceptable size of work array. I set size proposed in method description
            var work = new double[Math.Max((3 * Math.Min(rowsA, columnsA)) + Math.Max(rowsA, columnsA), 5 * Math.Min(rowsA, columnsA))];
            SvdSolve(a, rowsA, columnsA, s, u, vt, b, columnsB, x, work);
        }

        /// <summary>
        /// Solves A*X=B for X using the singular value decomposition of A.
        /// </summary>
        /// <param name="a">On entry, the M by N matrix to decompose. On exit, A may be overwritten.</param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="s">The singular values of A in ascending value.</param>
        /// <param name="u">On exit U contains the left singular vectors.</param>
        /// <param name="vt">On exit VT contains the transposed right singular vectors.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        /// <param name="work">The work array. For real matrices, the work array should be at least
        /// Max(3*Min(M, N) + Max(M, N), 5*Min(M,N)). For complex matrices, 2*Min(M, N) + Max(M, N).
        /// On exit, work[0] contains the optimal work size value.</param>
        public virtual void SvdSolve(double[] a, int rowsA, int columnsA, double[] s, double[] u, double[] vt, double[] b, int columnsB, double[] x, double[] work)
        {
            if (a == null)
            {
                throw new ArgumentNullException("a");
            }

            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            if (u == null)
            {
                throw new ArgumentNullException("u");
            }

            if (vt == null)
            {
                throw new ArgumentNullException("vt");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (u.Length != rowsA * rowsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "u");
            }

            if (vt.Length != columnsA * columnsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "vt");
            }

            if (s.Length != Math.Min(rowsA, columnsA))
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "s");
            }

            if (b.Length != rowsA * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            if (x.Length != columnsA * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            if (work.Length == 0)
            {
                throw new ArgumentException(Resources.ArgumentSingleDimensionArray, "work");
            }

            if (work.Length < rowsA)
            {
                work[0] = rowsA;
                throw new ArgumentException(Resources.WorkArrayTooSmall, "work");
            }

            SingularValueDecomposition(true, a, rowsA, columnsA, s, u, vt, work);
            SvdSolveFactored(rowsA, columnsA, s, u, vt, b, columnsB, x);
        }

        /// <summary>
        /// Solves A*X=B for X using a previously SVD decomposed matrix.
        /// </summary>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="s">The s values returned by <see cref="SingularValueDecomposition(bool,double[],int,int,double[],double[],double[])"/>.</param>
        /// <param name="u">The left singular vectors returned by  <see cref="SingularValueDecomposition(bool,double[],int,int,double[],double[],double[])"/>.</param>
        /// <param name="vt">The right singular  vectors returned by  <see cref="SingularValueDecomposition(bool,double[],int,int,double[],double[],double[])"/>.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        public virtual void SvdSolveFactored(int rowsA, int columnsA, double[] s, double[] u, double[] vt, double[] b, int columnsB, double[] x)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }

            if (u == null)
            {
                throw new ArgumentNullException("u");
            }

            if (vt == null)
            {
                throw new ArgumentNullException("vt");
            }

            if (b == null)
            {
                throw new ArgumentNullException("b");
            }

            if (x == null)
            {
                throw new ArgumentNullException("x");
            }

            if (u.Length != rowsA * rowsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "u");
            }

            if (vt.Length != columnsA * columnsA)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "vt");
            }

            if (s.Length != Math.Min(rowsA, columnsA))
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "s");
            }

            if (b.Length != rowsA * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            if (x.Length != columnsA * columnsB)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "b");
            }

            var mn = Math.Min(rowsA, columnsA);
            var tmp = new double[columnsA];

            for (var k = 0; k < columnsB; k++)
            {
                for (var j = 0; j < columnsA; j++)
                {
                    double value = 0;
                    if (j < mn)
                    {
                        for (var i = 0; i < rowsA; i++)
                        {
                            value += u[(j * rowsA) + i] * b[(k * rowsA) + i];
                        }

                        value /= s[j];
                    }

                    tmp[j] = value;
                }

                for (var j = 0; j < columnsA; j++)
                {
                    double value = 0;
                    for (var i = 0; i < columnsA; i++)
                    {
                        value += vt[(j * columnsA) + i] * tmp[i];
                    }

                    x[(k * columnsA) + j] = value;
                }
            }
        }
    }
}
