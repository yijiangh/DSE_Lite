// <copyright file="UserCholeskyTests.cs" company="Math.NET">
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

namespace MathNet.Numerics.UnitTests.LinearAlgebraTests.Single.Factorization
{
    using LinearAlgebra.Generic.Factorization;
    using MbUnit.Framework;

    public class UserCholeskyTests
    {
        [Test]
        [Row(1)]
        [Row(10)]
        [Row(100)]
        public void CanFactorizeIdentity(int order)
        {
            var I = UserDefinedMatrix.Identity(order);
            var factorC = I.Cholesky();

            Assert.AreEqual(I.RowCount, factorC.Factor.RowCount);
            Assert.AreEqual(I.ColumnCount, factorC.Factor.ColumnCount);

            for (var i = 0; i < factorC.Factor.RowCount; i++)
            {
                for (var j = 0; j < factorC.Factor.ColumnCount; j++)
                {
                    Assert.AreEqual(i == j ? 1.0 : 0.0, factorC.Factor[i, j]);
                }
            }
        }

        [Test]
        [ExpectedArgumentException]
        public void CholeskyFailsWithDiagonalNonPositiveDefiniteMatrix()
        {
            var I = UserDefinedMatrix.Identity(10);
            I[3, 3] = -4.0f;
            I.Cholesky();
        }

        [Test]
        [Row(3,5)]
        [Row(5,3)]
        [ExpectedArgumentException]
        public void CholeskyFailsWithNonSquareMatrix(int row, int col)
        {
            var I = new UserDefinedMatrix(row, col);
            I.Cholesky();
        }

        [Test]
        [Row(1)]
        [Row(10)]
        [Row(100)]
        public void IdentityDeterminantIsOne(int order)
        {
            var I = UserDefinedMatrix.Identity(order);
            var factorC = I.Cholesky();
            Assert.AreEqual(1.0, factorC.Determinant);
            Assert.AreEqual(0.0, factorC.DeterminantLn);
        }

        [Test]
        [Row(1)]
        [Row(2)]
        [Row(5)]
        [Row(10)]
        [Row(50)]
        [Row(100)]
        [MultipleAsserts]
        public void CanFactorizeRandomMatrix(int order)
        {
            var matrixX = MatrixLoader.GenerateRandomPositiveDefiniteUserDefinedMatrix(order);
            var chol = matrixX.Cholesky();
            var factorC = chol.Factor;

            // Make sure the Cholesky factor has the right dimensions.
            Assert.AreEqual(order, factorC.RowCount);
            Assert.AreEqual(order, factorC.ColumnCount);

            // Make sure the Cholesky factor is lower triangular.
            for (var i = 0; i < factorC.RowCount; i++) 
            {
                for (var j = i+1; j < factorC.ColumnCount; j++)
                {
                    Assert.AreEqual(0.0, factorC[i, j]);
                }
            }

            // Make sure the cholesky factor times it's transpose is the original matrix.
            var matrixXfromC = factorC * factorC.Transpose();
            for (var i = 0; i < matrixXfromC.RowCount; i++) 
            {
                for (var j = 0; j < matrixXfromC.ColumnCount; j++)
                {
                    Assert.AreApproximatelyEqual(matrixX[i,j], matrixXfromC[i, j], 10e-3f);
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
            var matrixA = MatrixLoader.GenerateRandomPositiveDefiniteUserDefinedMatrix(order);
            var matrixACopy = matrixA.Clone();
            var chol = matrixA.Cholesky();
            var b = MatrixLoader.GenerateRandomUserDefinedVector(order);
            var x = chol.Solve(b);

            Assert.AreEqual(b.Count, x.Count);

            var bReconstruct = matrixA * x;

            // Check the reconstruction.
            for (var i = 0; i < order; i++)
            {
                Assert.AreApproximatelyEqual(b[i], bReconstruct[i], 10e-3f);
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
        [Row(1,1)]
        [Row(2,4)]
        [Row(5,8)]
        [Row(10,3)]
        [Row(50,10)]
        [Row(100,100)]
        [MultipleAsserts]
        public void CanSolveForRandomMatrix(int row, int col)
        {
            var matrixA = MatrixLoader.GenerateRandomPositiveDefiniteUserDefinedMatrix(row);
            var matrixACopy = matrixA.Clone();
            var chol = matrixA.Cholesky();
            var matrixB = MatrixLoader.GenerateRandomUserDefinedMatrix(row, col);
            var matrixX = chol.Solve(matrixB);

            Assert.AreEqual(matrixB.RowCount, matrixX.RowCount);
            Assert.AreEqual(matrixB.ColumnCount, matrixX.ColumnCount);

            var matrixBReconstruct = matrixA * matrixX;

            // Check the reconstruction.
            for (var i = 0; i < matrixB.RowCount; i++)
            {
                for (var j = 0; j < matrixB.ColumnCount; j++)
                {
                    Assert.AreApproximatelyEqual(matrixB[i, j], matrixBReconstruct[i, j], 10e-3f);
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
            var matrixA = MatrixLoader.GenerateRandomPositiveDefiniteUserDefinedMatrix(order);
            var matrixACopy = matrixA.Clone();
            var chol = matrixA.Cholesky();
            var b = MatrixLoader.GenerateRandomUserDefinedVector(order);
            var bCopy = b.Clone();
            var x = new UserDefinedVector(order);
            chol.Solve(b, x);

            Assert.AreEqual(b.Count, x.Count);

            var bReconstruct = matrixA * x;

            // Check the reconstruction.
            for (var i = 0; i < order; i++)
            {
                Assert.AreApproximatelyEqual(b[i], bReconstruct[i], 10e-3f);
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
            for (var i = 0; i < order; i++)
            {
                Assert.AreEqual(bCopy[i], b[i]);
            }
        }

        [Test]
        [Row(1, 1)]
        [Row(2, 4)]
        [Row(5, 8)]
        [Row(10, 3)]
        [Row(50, 10)]
        [Row(100, 100)]
        [MultipleAsserts]
        public void CanSolveForRandomMatrixWhenResultMatrixGiven(int row, int col)
        {
            var matrixA = MatrixLoader.GenerateRandomPositiveDefiniteUserDefinedMatrix(row);
            var matrixACopy = matrixA.Clone();
            var chol = matrixA.Cholesky();
            var matrixB = MatrixLoader.GenerateRandomUserDefinedMatrix(row, col);
            var matrixBCopy = matrixB.Clone();
            var matrixX = new UserDefinedMatrix(row, col);
            chol.Solve(matrixB, matrixX);

            Assert.AreEqual(matrixB.RowCount, matrixX.RowCount);
            Assert.AreEqual(matrixB.ColumnCount, matrixX.ColumnCount);

            var matrixBReconstruct = matrixA * matrixX;

            // Check the reconstruction.
            for (var i = 0; i < matrixB.RowCount; i++)
            {
                for (var j = 0; j < matrixB.ColumnCount; j++)
                {
                    Assert.AreApproximatelyEqual(matrixB[i, j], matrixBReconstruct[i, j], 10e-3f);
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
