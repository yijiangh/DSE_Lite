// <copyright file="QRTests.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
// http://mathnetnumerics.codeplex.com
//
// Copyright (c) 2009-2010 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

namespace MathNet.Numerics.UnitTests.LinearAlgebraTests.Complex32.Factorization
{
    using Numerics;
    using LinearAlgebra.Generic.Factorization;
    using MbUnit.Framework;
    using LinearAlgebra.Complex32;
    using LinearAlgebra.Complex32.Factorization;

    public class QRTests
    {

        [Test]
        [ExpectedArgumentNullException]
        public void ConstructorNull()
        {
            new DenseQR(null);
        }

        [Test]
        [ExpectedArgumentException]
        public void WideMatrixThrowsInvalidMatrixOperationException()
        {
            new DenseQR(new DenseMatrix(3, 4));
        }
        
        [Test]
        [Row(1)]
        [Row(10)]
        [Row(100)]
        public void CanFactorizeIdentity(int order)
        {
            var I = DenseMatrix.Identity(order);
            var factorQR = I.QR();

            Assert.AreEqual(I.RowCount, factorQR.R.RowCount);
            Assert.AreEqual(I.ColumnCount, factorQR.R.ColumnCount);

            for (var i = 0; i < factorQR.R.RowCount; i++)
            {
                for (var j = 0; j < factorQR.R.ColumnCount; j++)
                {
                    if (i == j)
                    {
                        Assert.AreEqual(-Complex32.One, factorQR.R[i, j]);
                    }
                    else
                    {
                        Assert.AreEqual(Complex32.Zero, factorQR.R[i, j]);
                    }
                }
            }
        }

      
        [Test]
        [Row(1)]
        [Row(10)]
        [Row(100)]
        public void IdentityDeterminantIsOne(int order)
        {
            var I = DenseMatrix.Identity(order);
            var factorQR = I.QR();
            Assert.AreEqual(1.0f, factorQR.Determinant);
        }

        [Test]
        [Row(1,1)]
        [Row(2,2)]
        [Row(5,5)]
        [Row(10,6)]
        [Row(50,48)]
        [Row(100,98)]
        [MultipleAsserts]
        public void CanFactorizeRandomMatrix(int row, int column)
        {
            var matrixA = MatrixLoader.GenerateRandomDenseMatrix(row, column);
            var factorQR = matrixA.QR();

            // Make sure the R has the right dimensions.
            Assert.AreEqual(row, factorQR.R.RowCount);
            Assert.AreEqual(column, factorQR.R.ColumnCount);

            // Make sure the Q has the right dimensions.
            Assert.AreEqual(row, factorQR.Q.RowCount);
            Assert.AreEqual(row, factorQR.Q.ColumnCount);

            // Make sure the R factor is upper triangular.
            for (var i = 0; i < factorQR.R.RowCount; i++) 
            {
                for (var j = 0; j < factorQR.R.ColumnCount; j++)
                {
                    if (i > j)
                    {
                        Assert.AreEqual(Complex32.Zero, factorQR.R[i, j]);
                    }
                }
            }

            // Make sure the Q*R is the original matrix.
            var matrixQfromR = factorQR.Q * factorQR.R;
            for (var i = 0; i < matrixQfromR.RowCount; i++) 
            {
                for (var j = 0; j < matrixQfromR.ColumnCount; j++)
                {
                    Assert.AreApproximatelyEqual(matrixA[i, j].Real, matrixQfromR[i, j].Real, 1e-3f);
                    Assert.AreApproximatelyEqual(matrixA[i, j].Imaginary, matrixQfromR[i, j].Imaginary, 1e-3f);
                }
            }

            // Make sure the Q is unitary --> (Q*)x(Q) = I
            var matrixQсtQ = factorQR.Q.ConjugateTranspose() * factorQR.Q;
            for (var i = 0; i < matrixQсtQ.RowCount; i++)
            {
                for (var j = 0; j < matrixQсtQ.ColumnCount; j++)
                {
                    if (i == j)
                    {
                        Assert.AreApproximatelyEqual(matrixQсtQ[i, j].Real, 1.0f, 1e-3f);
                        Assert.AreApproximatelyEqual(matrixQсtQ[i, j].Imaginary, 0.0f, 1e-3f);
                    }
                    else
                    {
                        Assert.AreApproximatelyEqual(matrixQсtQ[i, j].Real, 0.0f, 1e-3f);
                        Assert.AreApproximatelyEqual(matrixQсtQ[i, j].Imaginary, 0.0f, 1e-3f);
                    }
                }
            }
        }

        [Test]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(50)]
        [Row(100)]
        [MultipleAsserts]
        public void CanSolveForRandomVector(int order)
        {
            var matrixA = MatrixLoader.GenerateRandomDenseMatrix(order, order);
            var matrixACopy = matrixA.Clone();
            var factorQR = matrixA.QR();

            var vectorb = MatrixLoader.GenerateRandomDenseVector(order);
            var resultx = factorQR.Solve(vectorb);

            Assert.AreEqual(matrixA.ColumnCount, resultx.Count);

            var bReconstruct = matrixA * resultx;

            // Check the reconstruction.
            for (var i = 0; i < order; i++)
            {
                Assert.AreApproximatelyEqual(vectorb[i].Real, bReconstruct[i].Real, 1e-3f);
                Assert.AreApproximatelyEqual(vectorb[i].Imaginary, bReconstruct[i].Imaginary, 1e-3f);
            }

            // Make sure A didn't change.
            for (var i = 0; i < matrixA.RowCount; i++)
            {
                for (var j = 0; j < matrixA.ColumnCount; j++)
                {
                    Assert.AreEqual(matrixACopy[i, j], matrixA[i, j]);
                }
            }
        }

        [Test]
        [Row(1)]
        [Row(4)]
        [Row(8)]
        [Row(10)]
        [Row(50)]
        [Row(100)]
        [MultipleAsserts]
        public void CanSolveForRandomMatrix(int order)
        {
            var matrixA = MatrixLoader.GenerateRandomDenseMatrix(order, order);
            var matrixACopy = matrixA.Clone();
            var factorQR = matrixA.QR();

            var matrixB = MatrixLoader.GenerateRandomDenseMatrix(order, order);
            var matrixX = factorQR.Solve(matrixB);

            // The solution X row dimension is equal to the column dimension of A
            Assert.AreEqual(matrixA.ColumnCount, matrixX.RowCount);
            // The solution X has the same number of columns as B
            Assert.AreEqual(matrixB.ColumnCount, matrixX.ColumnCount);

            var matrixBReconstruct = matrixA * matrixX;

            // Check the reconstruction.
            for (var i = 0; i < matrixB.RowCount; i++)
            {
                for (var j = 0; j < matrixB.ColumnCount; j++)
                {
                    Assert.AreApproximatelyEqual(matrixB[i, j].Real, matrixBReconstruct[i, j].Real, 1e-3f);
                    Assert.AreApproximatelyEqual(matrixB[i, j].Imaginary, matrixBReconstruct[i, j].Imaginary, 1e-3f);
                }
            }

            // Make sure A didn't change.
            for (var i = 0; i < matrixA.RowCount; i++)
            {
                for (var j = 0; j < matrixA.ColumnCount; j++)
                {
                    Assert.AreEqual(matrixACopy[i, j], matrixA[i, j]);
                }
            }
        }

        [Test]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(50)]
        [Row(100)]
        [MultipleAsserts]
        public void CanSolveForRandomVectorWhenResultVectorGiven(int order)
        {
            var matrixA = MatrixLoader.GenerateRandomDenseMatrix(order, order);
            var matrixACopy = matrixA.Clone();
            var factorQR = matrixA.QR();
            var vectorb = MatrixLoader.GenerateRandomDenseVector(order);
            var vectorbCopy = vectorb.Clone();
            var resultx = new DenseVector(order);
            factorQR.Solve(vectorb,resultx);

            Assert.AreEqual(vectorb.Count, resultx.Count);

            var bReconstruct = matrixA * resultx;

            // Check the reconstruction.
            for (var i = 0; i < vectorb.Count; i++)
            {
                Assert.AreApproximatelyEqual(vectorb[i].Real, bReconstruct[i].Real, 1e-3f);
                Assert.AreApproximatelyEqual(vectorb[i].Imaginary, bReconstruct[i].Imaginary, 1e-3f);
            }

            // Make sure A didn't change.
            for (var i = 0; i < matrixA.RowCount; i++)
            {
                for (var j = 0; j < matrixA.ColumnCount; j++)
                {
                    Assert.AreEqual(matrixACopy[i, j], matrixA[i, j]);
                }
            }

            // Make sure b didn't change.
            for (var i = 0; i < vectorb.Count; i++)
            {
                Assert.AreEqual(vectorbCopy[i], vectorb[i]);
            }
        }

        [Test]
        [Row(1)]
        [Row(4)]
        [Row(8)]
        [Row(10)]
        [Row(50)]
        [Row(100)]
        [MultipleAsserts]
        public void CanSolveForRandomMatrixWhenResultMatrixGiven(int order)
        {
            var matrixA = MatrixLoader.GenerateRandomDenseMatrix(order, order);
            var matrixACopy = matrixA.Clone();
            var factorQR = matrixA.QR();

            var matrixB = MatrixLoader.GenerateRandomDenseMatrix(order, order);
            var matrixBCopy = matrixB.Clone();

            var matrixX = new DenseMatrix(order, order);
            factorQR.Solve(matrixB,matrixX);

            // The solution X row dimension is equal to the column dimension of A
            Assert.AreEqual(matrixA.ColumnCount, matrixX.RowCount);
            // The solution X has the same number of columns as B
            Assert.AreEqual(matrixB.ColumnCount, matrixX.ColumnCount);

            var matrixBReconstruct = matrixA * matrixX;

            // Check the reconstruction.
            for (var i = 0; i < matrixB.RowCount; i++)
            {
                for (var j = 0; j < matrixB.ColumnCount; j++)
                {
                    Assert.AreApproximatelyEqual(matrixB[i, j].Real, matrixBReconstruct[i, j].Real, 1e-3f);
                    Assert.AreApproximatelyEqual(matrixB[i, j].Imaginary, matrixBReconstruct[i, j].Imaginary, 1e-3f);
                }
            }

            // Make sure A didn't change.
            for (var i = 0; i < matrixA.RowCount; i++)
            {
                for (var j = 0; j < matrixA.ColumnCount; j++)
                {
                    Assert.AreEqual(matrixACopy[i, j], matrixA[i, j]);
                }
            }

            // Make sure B didn't change.
            for (var i = 0; i < matrixB.RowCount; i++)
            {
                for (var j = 0; j < matrixB.ColumnCount; j++)
                {
                    Assert.AreEqual(matrixBCopy[i, j], matrixB[i, j]);
                }
            }
        }
    }
}
