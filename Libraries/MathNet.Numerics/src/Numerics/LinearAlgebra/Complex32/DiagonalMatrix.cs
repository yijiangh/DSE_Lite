// <copyright file="DiagonalMatrix.cs" company="Math.NET">
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

namespace MathNet.Numerics.LinearAlgebra.Complex32
{
    using System;
    using System.Linq;
    using Generic;
    using Numerics;
    using Properties;
    using Threading;

    /// <summary>
    /// A matrix type for diagonal matrices. 
    /// </summary>
    /// <remarks>
    /// Diagonal matrices can be non-square matrices but the diagonal always starts
    /// at element 0,0. A diagonal matrix will throw an exception if non diagonal
    /// entries are set. The exception to this is when the off diagonal elements are
    /// 0.0 or NaN; these settings will cause no change to the diagonal matrix.
    /// </remarks>
    public class DiagonalMatrix : Matrix
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiagonalMatrix"/> class. This matrix is square with a given size.
        /// </summary>
        /// <param name="order">the size of the square matrix.</param>
        /// <exception cref="ArgumentException">
        /// If <paramref name="order"/> is less than one.
        /// </exception>
        public DiagonalMatrix(int order)
            : base(order)
        {
            Data = new Complex32[order * order];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagonalMatrix"/> class.
        /// </summary>
        /// <param name="rows">
        /// The number of rows.
        /// </param>
        /// <param name="columns">
        /// The number of columns.
        /// </param>
        public DiagonalMatrix(int rows, int columns)
            : base(rows, columns)
        {
            Data = new Complex32[Math.Min(rows, columns)];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagonalMatrix"/> class with all entries set to a particular value.
        /// </summary>
        /// <param name="rows">
        /// The number of rows.
        /// </param>
        /// <param name="columns">
        /// The number of columns.
        /// </param>
        /// <param name="value">The value which we assign to each element of the matrix.</param>
        public DiagonalMatrix(int rows, int columns, Complex32 value)
            : base(rows, columns)
        {
            Data = new Complex32[Math.Min(rows, columns)];
            for (var i = 0; i < Data.Length; i++)
            {
                Data[i] = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagonalMatrix"/> class from a one dimensional array with diagonal elements. This constructor
        /// will reference the one dimensional array and not copy it.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        /// <param name="diagonalArray">The one dimensional array which contain diagonal elements.</param>
        public DiagonalMatrix(int rows, int columns, Complex32[] diagonalArray)
            : base(rows, columns)
        {
            Data = diagonalArray;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagonalMatrix"/> class from a 2D array. 
        /// </summary>
        /// <param name="array">The 2D array to create this matrix from.</param>
        /// <exception cref="IndexOutOfRangeException">When <paramref name="array"/> contains an off-diagonal element.</exception>
        /// <exception cref="IndexOutOfRangeException">Depending on the implementation, an <see cref="IndexOutOfRangeException"/>
        /// may be thrown if one of the indices is outside the dimensions of the matrix.</exception>
        public DiagonalMatrix(Complex32[,] array)
            : this(array.GetLength(0), array.GetLength(1))
        {
            var rows = array.GetLength(0);
            var columns = array.GetLength(1);

            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < columns; j++)
                {
                    if (i == j)
                    {
                        Data[i] = array[i, j];
                    }
                    else if (((array[i, j].Real != 0.0) && !double.IsNaN(array[i, j].Real)) || ((array[i, j].Imaginary != 0.0) && !double.IsNaN(array[i, j].Imaginary)))
                    {
                        throw new IndexOutOfRangeException("Cannot set an off-diagonal element in a diagonal matrix.");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the matrix's data.
        /// </summary>
        /// <value>The matrix's data.</value>
        internal Complex32[] Data
        {
            get;
            private set;
        }

        /// <summary>
        /// Retrieves the requested element without range checking.
        /// </summary>
        /// <param name="row">
        /// The row of the element.
        /// </param>
        /// <param name="column">
        /// The column of the element.
        /// </param>
        /// <returns>
        /// The requested element.
        /// </returns>
        /// <exception cref="IndexOutOfRangeException">Depending on the implementation, an <see cref="IndexOutOfRangeException"/>
        /// may be thrown if one of the indices is outside the dimensions of the matrix.</exception>
        public override Complex32 At(int row, int column)
        {
            return row == column ? Data[row] : 0.0f;
        }

        /// <summary>
        /// Sets the value of the given element.
        /// </summary>
        /// <param name="row">
        /// The row of the element.
        /// </param>
        /// <param name="column">
        /// The column of the element.
        /// </param>
        /// <param name="value">
        /// The value to set the element to.
        /// </param>
        /// <exception cref="IndexOutOfRangeException">When trying to set an off diagonal element.</exception>
        /// <exception cref="IndexOutOfRangeException">Depending on the implementation, an <see cref="IndexOutOfRangeException"/>
        /// may be thrown if one of the indices is outside the dimensions of the matrix.</exception>
        public override void At(int row, int column, Complex32 value)
        {
            if (row == column)
            {
                Data[row] = value;
            }
            else if (((value.Real != 0.0) && !double.IsNaN(value.Real)) || ((value.Imaginary != 0.0) && !double.IsNaN(value.Imaginary)))
            {
                throw new IndexOutOfRangeException("Cannot set an off-diagonal element in a diagonal matrix.");
            }
        }

        /// <summary>
        /// Creates a <c>DiagonalMatrix</c> for the given number of rows and columns.
        /// </summary>
        /// <param name="numberOfRows">
        /// The number of rows.
        /// </param>
        /// <param name="numberOfColumns">
        /// The number of columns.
        /// </param>
        /// <returns>
        /// A <c>DiagonalMatrix</c> with the given dimensions.
        /// </returns>
        public override Matrix<Complex32> CreateMatrix(int numberOfRows, int numberOfColumns)
        {
            return new DiagonalMatrix(numberOfRows, numberOfColumns);
        }

        /// <summary>
        /// Creates a <see cref="Vector{T}"/> with a the given dimension.
        /// </summary>
        /// <param name="size">The size of the vector.</param>
        /// <returns>
        /// A <see cref="Vector{T}"/> with the given dimension.
        /// </returns>
        public override Vector<Complex32> CreateVector(int size)
        {
            return new SparseVector(size);
        }

        /// <summary>
        /// Sets all values to zero.
        /// </summary>
        public override void Clear()
        {
            Array.Clear(Data, 0, Data.Length);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="obj">
        /// An object to compare with this object.
        /// </param>
        /// <returns>
        /// <c>true</c> if the current object is equal to the <paramref name="obj"/> parameter; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var diagonalMatrix = obj as DiagonalMatrix;

            if (diagonalMatrix == null)
            {
                return base.Equals(obj);
            }

            // Accept if the argument is the same object as this
            if (ReferenceEquals(this, diagonalMatrix))
            {
                return true;
            }

            if (diagonalMatrix.Data.Length != Data.Length)
            {
                return false;
            }

            // If all else fails, perform element wise comparison.
            return !Data.Where((t, i) => t != diagonalMatrix.Data[i]).Any();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            var hashNum = Math.Min(Data.Length, 25);
            long hash = 0;
            for (var i = 0; i < hashNum; i++)
            {
#if SILVERLIGHT
                hash ^= Precision.DoubleToInt64Bits(Data[i].GetHashCode());
#else
                hash ^= BitConverter.DoubleToInt64Bits(Data[i].GetHashCode());
#endif
            }

            return BitConverter.ToInt32(BitConverter.GetBytes(hash), 4);
        }

        #region Elementary operations

        /// <summary>
        /// Adds another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        /// <returns>The result of the addition.</returns>
        /// <exception cref="ArgumentNullException">If the other matrix is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the two matrices don't have the same dimensions.</exception>
        public override Matrix<Complex32> Add(Matrix<Complex32> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (other.RowCount != RowCount || other.ColumnCount != ColumnCount)
            {
                throw new ArgumentOutOfRangeException("other", Resources.ArgumentMatrixDimensions);
            }

            Matrix<Complex32> result;
            if (other is DiagonalMatrix)
            {
                result = new DenseMatrix(RowCount, ColumnCount);
            }
            else
            {
                result = new DiagonalMatrix(RowCount, ColumnCount);
            }

            Add(other, result);
            return result;
        }

        /// <summary>
        /// Adds another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        /// <param name="result">The matrix to store the result of the addition.</param>
        /// <exception cref="ArgumentNullException">If the other matrix is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the two matrices don't have the same dimensions.</exception>
        public override void Add(Matrix<Complex32> other, Matrix<Complex32> result)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (other.RowCount != RowCount || other.ColumnCount != ColumnCount)
            {
                throw new ArgumentOutOfRangeException("other", Resources.ArgumentMatrixDimensions);
            }

            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
            {
                throw new ArgumentOutOfRangeException("result", Resources.ArgumentMatrixDimensions);
            }

            var diagOther = other as DiagonalMatrix;
            var diagResult = result as DiagonalMatrix;

            if (diagOther == null || diagResult == null)
            {
                base.Add(other, result);
            }
            else
            {
                Control.LinearAlgebraProvider.AddArrays(Data, diagOther.Data, diagResult.Data);
            }
        }

        /// <summary>
        /// Subtracts another matrix from this matrix.
        /// </summary>
        /// <param name="other">The matrix to subtract.</param>
        /// <returns>The result of the subtraction.</returns>
        /// <exception cref="ArgumentNullException">If the other matrix is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the two matrices don't have the same dimensions.</exception>
        public override Matrix<Complex32> Subtract(Matrix<Complex32> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (other.RowCount != RowCount || other.ColumnCount != ColumnCount)
            {
                throw new ArgumentOutOfRangeException("other", Resources.ArgumentMatrixDimensions);
            }

            Matrix<Complex32> result;
            if (other is DiagonalMatrix)
            {
                result = new DenseMatrix(RowCount, ColumnCount);
            }
            else
            {
                result = new DiagonalMatrix(RowCount, ColumnCount);
            }

            Subtract(other, result);
            return result;
        }

        /// <summary>
        /// Subtracts another matrix from this matrix. 
        /// </summary>
        /// <param name="other">The matrix to subtract.</param>
        /// <param name="result">The matrix to store the result of the subtraction.</param>
        /// <exception cref="ArgumentNullException">If the other matrix is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If the two matrices don't have the same dimensions.</exception>
        public override void Subtract(Matrix<Complex32> other, Matrix<Complex32> result)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (other.RowCount != RowCount || other.ColumnCount != ColumnCount)
            {
                throw new ArgumentOutOfRangeException("other", Resources.ArgumentMatrixDimensions);
            }

            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
            {
                throw new ArgumentOutOfRangeException("result", Resources.ArgumentMatrixDimensions);
            }

            var diagOther = other as DiagonalMatrix;
            var diagResult = result as DiagonalMatrix;

            if (diagOther == null || diagResult == null)
            {
                base.Subtract(other, result);
            }
            else
            {
                Control.LinearAlgebraProvider.SubtractArrays(Data, diagOther.Data, diagResult.Data);
            }
        }

        /// <summary>
        /// Copies the values of the given array to the diagonal.
        /// </summary>
        /// <param name="source">The array to copy the values from. The length of the vector should be
        /// Min(Rows, Columns).</param>
        /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null" />.</exception>   
        /// <exception cref="ArgumentException">If the length of <paramref name="source"/> does not
        /// equal Min(Rows, Columns).</exception>
        /// <remarks>For non-square matrices, the elements of <paramref name="source"/> are copied to
        /// this[i,i].</remarks>
        public override void SetDiagonal(Complex32[] source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (source.Length != Data.Length)
            {
                throw new ArgumentException(Resources.ArgumentArraysSameLength, "source");
            }

            Array.Copy(source, Data, source.Length);
        }

        /// <summary>
        /// Copies the values of the given <see cref="Vector{T}"/> to the diagonal.
        /// </summary>
        /// <param name="source">The vector to copy the values from. The length of the vector should be
        /// Min(Rows, Columns).</param>
        /// <exception cref="ArgumentNullException">If <paramref name="source"/> is <see langword="null" />.</exception>   
        /// <exception cref="ArgumentException">If the length of <paramref name="source"/> does not
        /// equal Min(Rows, Columns).</exception>
        /// <remarks>For non-square matrices, the elements of <paramref name="source"/> are copied to
        /// this[i,i].</remarks>
        public override void SetDiagonal(Vector<Complex32> source)
        {
            var denseSource = source as DenseVector;
            if (denseSource == null)
            {
                base.SetDiagonal(source);
                return;
            }

            if (Data.Length != denseSource.Data.Length)
            {
                throw new ArgumentException(Resources.ArgumentVectorsSameLength, "source");
            }

            Array.Copy(denseSource.Data, Data, denseSource.Data.Length);
        }

        /// <summary>
        /// Multiplies each element of the matrix by a scalar and places results into the result matrix.
        /// </summary>
        /// <param name="scalar">The scalar to multiply the matrix with.</param>
        /// <param name="result">The matrix to store the result of the multiplication.</param>
        /// <exception cref="ArgumentNullException">If the result matrix is <see langword="null" />.</exception> 
        /// <exception cref="ArgumentException">If the result matrix's dimensions are not the same as this matrix.</exception>
        public override void Multiply(Complex32 scalar, Matrix<Complex32> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (scalar.IsZero())
            {
                result.Clear();
                return;
            }

            if (scalar.IsOne())
            {
                CopyTo(result);
                return;
            }

            var diagResult = result as DiagonalMatrix;
            if (diagResult == null)
            {
                base.Multiply(scalar, result);
            }
            else
            {
                CopyTo(diagResult);
                Control.LinearAlgebraProvider.ScaleArray(scalar, diagResult.Data);
            }
        }

        /// <summary>
        /// Multiplies this diagonal matrix with another diagonal matrix and places the results into the result diagonal matrix.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        /// <exception cref="ArgumentNullException">If the other matrix is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">If the result matrix is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If <strong>this.Columns != other.Rows</strong>.</exception>
        /// <exception cref="ArgumentException">If the result matrix's dimensions are not the this.Rows x other.Columns.</exception>
        public override void Multiply(Matrix<Complex32> other, Matrix<Complex32> result)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (ColumnCount != other.RowCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions);
            }

            if (result.RowCount != RowCount || result.ColumnCount != other.ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions);
            }

            var m = other as DiagonalMatrix;
            var r = result as DiagonalMatrix;

            if (m == null || r == null)
            {
                base.Multiply(other, result);
            }
            else
            {
                var thisDataCopy = new Complex32[r.Data.Length];
                var otherDataCopy = new Complex32[r.Data.Length];
                Array.Copy(Data, thisDataCopy, (r.Data.Length > Data.Length) ? Data.Length : r.Data.Length);
                Array.Copy(m.Data, otherDataCopy, (r.Data.Length > m.Data.Length) ? m.Data.Length : r.Data.Length);

                Control.LinearAlgebraProvider.PointWiseMultiplyArrays(thisDataCopy, otherDataCopy, r.Data);
            }
        }

        /// <summary>
        /// Multiplies this matrix with another matrix and returns the result.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <exception cref="ArgumentException">If <strong>this.Columns != other.Rows</strong>.</exception>        
        /// <exception cref="ArgumentNullException">If the other matrix is <see langword="null" />.</exception>
        /// <returns>The result of multiplication.</returns>
        public override Matrix<Complex32> Multiply(Matrix<Complex32> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException("other");
            }

            if (ColumnCount != other.RowCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions);
            }

            var m = other as DiagonalMatrix;
            if (m == null)
            {
                return base.Multiply(other);
            }

            var result = (DiagonalMatrix)CreateMatrix(RowCount, other.ColumnCount);
            Multiply(other, result);
            return result;
        }

        /// <summary>
        /// Multiplies this matrix with a vector and places the results into the result matrix.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="rightSide"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="result"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If <strong>result.Count != this.RowCount</strong>.</exception>
        /// <exception cref="ArgumentException">If <strong>this.ColumnCount != <paramref name="rightSide"/>.Count</strong>.</exception>
        public override void Multiply(Vector<Complex32> rightSide, Vector<Complex32> result)
        {
            if (rightSide == null)
            {
                throw new ArgumentNullException("rightSide");
            }

            if (ColumnCount != rightSide.Count)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions, "rightSide");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (RowCount != result.Count)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions, "result");
            }

            if (ReferenceEquals(rightSide, result))
            {
                var tmp = result.CreateVector(result.Count);
                Multiply(rightSide, tmp);
                tmp.CopyTo(result);
            }
            else
            {
                // Clear the result vector
                result.Clear();

                // Multiply the elements in the vector with the corresponding diagonal element in this.
                for (var r = 0; r < Data.Length; r++)
                {
                    result[r] = Data[r] * rightSide[r];
                }
            }
        }

        /// <summary>
        /// Left multiply a matrix with a vector ( = vector * matrix ) and place the result in the result vector.
        /// </summary>
        /// <param name="leftSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="leftSide"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">If the result matrix is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If <strong>result.Count != this.ColumnCount</strong>.</exception>
        /// <exception cref="ArgumentException">If <strong>this.RowCount != <paramref name="leftSide"/>.Count</strong>.</exception>
        public override void LeftMultiply(Vector<Complex32> leftSide, Vector<Complex32> result)
        {
            if (leftSide == null)
            {
                throw new ArgumentNullException("leftSide");
            }

            if (RowCount != leftSide.Count)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions, "leftSide");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (ColumnCount != result.Count)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions, "result");
            }

            if (ReferenceEquals(leftSide, result))
            {
                var tmp = result.CreateVector(result.Count);
                LeftMultiply(leftSide, tmp);
                tmp.CopyTo(result);
            }
            else
            {
                // Clear the result vector
                result.Clear();

                // Multiply the elements in the vector with the corresponding diagonal element in this.
                for (var r = 0; r < Data.Length; r++)
                {
                    result[r] = Data[r] * leftSide[r];
                }
            }
        }

        /// <summary>
        /// Computes the determinant of this matrix.
        /// </summary>
        /// <returns>The determinant of this matrix.</returns>
        public override Complex32 Determinant()
        {
            if (RowCount != ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixSquare);
            }

            return Data.Aggregate(Complex32.One, (current, t) => current * t);
        }

        /// <summary>
        /// Returns the elements of the diagonal in a <see cref="DenseVector"/>.
        /// </summary>
        /// <returns>The elements of the diagonal.</returns>
        /// <remarks>For non-square matrices, the method returns Min(Rows, Columns) elements where
        /// i == j (i is the row index, and j is the column index).</remarks>
        public override Vector<Complex32> Diagonal()
        {
            // TODO: Should we return reference to array? In current implementation we return copy of array, so changes in DenseVector will
            // not influence onto diagonal elements
            return new DenseVector((Complex32[])Data.Clone());
        }

        /// <summary>
        /// Multiplies this diagonal matrix with transpose of another diagonal matrix and places the results into the result diagonal matrix.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        /// <exception cref="ArgumentNullException">If the other matrix is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">If the result matrix is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If <strong>this.Columns != other.Rows</strong>.</exception>
        /// <exception cref="ArgumentException">If the result matrix's dimensions are not the this.Rows x other.Columns.</exception>
        public override void TransposeAndMultiply(Matrix<Complex32> other, Matrix<Complex32> result)
        {
            var otherDiagonal = other as DiagonalMatrix;
            var resultDiagonal = result as DiagonalMatrix;

            if (otherDiagonal == null || resultDiagonal == null)
            {
                base.TransposeAndMultiply(other, result);
                return;
            }

            Multiply(otherDiagonal.Transpose(), result);
        }

        /// <summary>
        /// Multiplies this matrix with transpose of another matrix and returns the result.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <exception cref="ArgumentException">If <strong>this.Columns != other.Rows</strong>.</exception>        
        /// <exception cref="ArgumentNullException">If the other matrix is <see langword="null" />.</exception>
        /// <returns>The result of multiplication.</returns>
        public override Matrix<Complex32> TransposeAndMultiply(Matrix<Complex32> other)
        {
            var otherDiagonal = other as DiagonalMatrix;
            if (otherDiagonal == null)
            {
                return base.TransposeAndMultiply(other);
            }

            if (ColumnCount != otherDiagonal.ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions);
            }

            var result = (DiagonalMatrix)CreateMatrix(RowCount, other.RowCount);
            TransposeAndMultiply(other, result);
            return result;
        }

        #endregion

        /// <summary>
        /// Copies the elements of this matrix to the given matrix.
        /// </summary>
        /// <param name="target">
        /// The matrix to copy values into.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If target is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If this and the target matrix do not have the same dimensions..
        /// </exception>
        public override void CopyTo(Matrix<Complex32> target)
        {
            var diagonalTarget = target as DiagonalMatrix;

            if (diagonalTarget == null)
            {
                base.CopyTo(target);
                return;
            }

            if (ReferenceEquals(this, target))
            {
                return;
            }

            if (RowCount != target.RowCount || ColumnCount != target.ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions, "target");
            }

            Array.Copy(Data, diagonalTarget.Data, Data.Length);
        }

        /// <summary>
        /// Returns the transpose of this matrix.
        /// </summary>        
        /// <returns>The transpose of this matrix.</returns>
        public override Matrix<Complex32> Transpose()
        {
            var ret = new DiagonalMatrix(ColumnCount, RowCount);
            Array.Copy(Data, ret.Data, Data.Length);
            return ret;
        }

        /// <summary>
        /// Copies the requested column elements into the given vector.
        /// </summary>
        /// <param name="columnIndex">The column to copy elements from.</param>
        /// <param name="rowIndex">The row to start copying from.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <param name="result">The <see cref="Vector{T}"/> to copy the column into.</param>
        /// <exception cref="ArgumentNullException">If the result <see cref="Vector{T}"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="columnIndex"/> is negative,
        /// or greater than or equal to the number of columns.</exception>        
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="rowIndex"/> is negative,
        /// or greater than or equal to the number of rows.</exception>        
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="rowIndex"/> + <paramref name="length"/>  
        /// is greater than or equal to the number of rows.</exception>
        /// <exception cref="ArgumentException">If <paramref name="length"/> is not positive.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <strong>result.Count &lt; length</strong>.</exception>
        public override void Column(int columnIndex, int rowIndex, int length, Vector<Complex32> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (columnIndex >= ColumnCount || columnIndex < 0)
            {
                throw new ArgumentOutOfRangeException("columnIndex");
            }

            if (rowIndex >= RowCount || rowIndex < 0)
            {
                throw new ArgumentOutOfRangeException("rowIndex");
            }

            if (rowIndex + length > RowCount)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            if (length < 1)
            {
                throw new ArgumentException(Resources.ArgumentMustBePositive, "length");
            }

            if (result.Count < length)
            {
                throw new ArgumentException(Resources.ArgumentVectorsSameLength, "result");
            }

            // Clear the result and copy the diagonal entry.
            result.Clear();
            if (columnIndex >= rowIndex && columnIndex < rowIndex + length && columnIndex < Data.Length)
            {
                result[columnIndex - rowIndex] = Data[columnIndex];
            }
        }

        /// <summary>
        /// Copies the requested row elements into a new <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="rowIndex">The row to copy elements from.</param>
        /// <param name="columnIndex">The column to start copying from.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <param name="result">The <see cref="Vector{T}"/> to copy the column into.</param>
        /// <exception cref="ArgumentNullException">If the result <see cref="Vector{T}"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="rowIndex"/> is negative,
        /// or greater than or equal to the number of columns.</exception>        
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="columnIndex"/> is negative,
        /// or greater than or equal to the number of rows.</exception>        
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="columnIndex"/> + <paramref name="length"/>  
        /// is greater than or equal to the number of rows.</exception>
        /// <exception cref="ArgumentException">If <paramref name="length"/> is not positive.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <strong>result.Count &lt; length</strong>.</exception>
        public override void Row(int rowIndex, int columnIndex, int length, Vector<Complex32> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (rowIndex >= RowCount || rowIndex < 0)
            {
                throw new ArgumentOutOfRangeException("rowIndex");
            }

            if (columnIndex >= ColumnCount || columnIndex < 0)
            {
                throw new ArgumentOutOfRangeException("columnIndex");
            }

            if (columnIndex + length > ColumnCount)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            if (length < 1)
            {
                throw new ArgumentException(Resources.ArgumentMustBePositive, "length");
            }

            if (result.Count < length)
            {
                throw new ArgumentException(Resources.ArgumentVectorsSameLength, "result");
            }

            // Clear the result and copy the diagonal entry.
            result.Clear();
            if (rowIndex >= columnIndex && rowIndex < columnIndex + length && rowIndex < Data.Length)
            {
                result[rowIndex - columnIndex] = Data[rowIndex];
            }
        }

        /// <summary>Calculates the L1 norm.</summary>
        /// <returns>The L1 norm of the matrix.</returns>
        public override Complex32 L1Norm()
        {
            return Data.Aggregate(float.NegativeInfinity, (current, t) => Math.Max(current, t.Magnitude));
        }

        /// <summary>Calculates the L2 norm.</summary>
        /// <returns>The L2 norm of the matrix.</returns>   
        public override Complex32 L2Norm()
        {
            return Data.Aggregate(float.NegativeInfinity, (current, t) => Math.Max(current, t.Magnitude));
        }

        /// <summary>Calculates the Frobenius norm of this matrix.</summary>
        /// <returns>The Frobenius norm of this matrix.</returns>
        public override Complex32 FrobeniusNorm()
        {
            var norm = Data.Sum(t => t.Magnitude * t.Magnitude);
            return Convert.ToSingle(Math.Sqrt(norm));
        }

        /// <summary>Calculates the infinity norm of this matrix.</summary>
        /// <returns>The infinity norm of this matrix.</returns>   
        public override Complex32 InfinityNorm()
        {
            return L1Norm();
        }

        /// <summary>Calculates the condition number of this matrix.</summary>
        /// <returns>The condition number of the matrix.</returns>
        public override Complex32 ConditionNumber()
        {
            var maxSv = float.NegativeInfinity;
            var minSv = float.PositiveInfinity;
            foreach (var t in Data)
            {
                maxSv = Math.Max(maxSv, t.Magnitude);
                minSv = Math.Min(minSv, t.Magnitude);
            }

            return maxSv / minSv;
        }

        /// <summary>Computes the inverse of this matrix.</summary>
        /// <exception cref="ArgumentException">If <see cref="DiagonalMatrix"/> is not a square matrix.</exception>
        /// <exception cref="ArgumentException">If <see cref="DiagonalMatrix"/> is singular.</exception>
        /// <returns>The inverse of this matrix.</returns>
        public override Matrix<Complex32> Inverse()
        {
            if (RowCount != ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixSquare);
            }

            var inverse = (DiagonalMatrix)Clone();
            for (var i = 0; i < Data.Length; i++)
            {
                if (Data[i] != 0.0f)
                {
                    inverse.Data[i] = 1.0f / Data[i];
                }
                else
                {
                    throw new ArgumentException(Resources.ArgumentMatrixNotSingular);
                }
            }

            return inverse;
        }

        /// <summary>
        /// Returns a new matrix containing the lower triangle of this matrix.
        /// </summary>
        /// <returns>The lower triangle of this matrix.</returns>  
        public override Matrix<Complex32> LowerTriangle()
        {
            return Clone();
        }

        /// <summary>
        /// Puts the lower triangle of this matrix into the result matrix.
        /// </summary>
        /// <param name="result">Where to store the lower triangle.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="result"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If the result matrix's dimensions are not the same as this matrix.</exception>
        public override void LowerTriangle(Matrix<Complex32> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions, "result");
            }

            if (ReferenceEquals(this, result))
            {
                return;
            }

            result.Clear();
            for (var i = 0; i < Data.Length; i++)
            {
                result[i, i] = Data[i];
            }
        }

        /// <summary>
        /// Returns a new matrix containing the lower triangle of this matrix. The new matrix
        /// does not contain the diagonal elements of this matrix.
        /// </summary>
        /// <returns>The lower triangle of this matrix.</returns>
        public override Matrix<Complex32> StrictlyLowerTriangle()
        {
            return new DiagonalMatrix(RowCount, ColumnCount);
        }

        /// <summary>
        /// Puts the strictly lower triangle of this matrix into the result matrix.
        /// </summary>
        /// <param name="result">Where to store the lower triangle.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="result"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If the result matrix's dimensions are not the same as this matrix.</exception>
        public override void StrictlyLowerTriangle(Matrix<Complex32> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions, "result");
            }

            result.Clear();
        }

        /// <summary>
        /// Returns a new matrix containing the upper triangle of this matrix.
        /// </summary>
        /// <returns>The upper triangle of this matrix.</returns>   
        public override Matrix<Complex32> UpperTriangle()
        {
            return Clone();
        }

        /// <summary>
        /// Puts the upper triangle of this matrix into the result matrix.
        /// </summary>
        /// <param name="result">Where to store the lower triangle.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="result"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If the result matrix's dimensions are not the same as this matrix.</exception>
        public override void UpperTriangle(Matrix<Complex32> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions, "result");
            }

            result.Clear();
            for (var i = 0; i < Data.Length; i++)
            {
                result[i, i] = Data[i];
            }
        }

        /// <summary>
        /// Returns a new matrix containing the upper triangle of this matrix. The new matrix
        /// does not contain the diagonal elements of this matrix.
        /// </summary>
        /// <returns>The upper triangle of this matrix.</returns>
        public override Matrix<Complex32> StrictlyUpperTriangle()
        {
            return new DiagonalMatrix(RowCount, ColumnCount);
        }

        /// <summary>
        /// Puts the strictly upper triangle of this matrix into the result matrix.
        /// </summary>
        /// <param name="result">Where to store the lower triangle.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="result"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If the result matrix's dimensions are not the same as this matrix.</exception>
        public override void StrictlyUpperTriangle(Matrix<Complex32> result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions, "result");
            }

            result.Clear();
        }

        /// <summary>
        /// Creates a matrix that contains the values from the requested sub-matrix.
        /// </summary>
        /// <param name="rowIndex">The row to start copying from.</param>
        /// <param name="rowLength">The number of rows to copy. Must be positive.</param>
        /// <param name="columnIndex">The column to start copying from.</param>
        /// <param name="columnLength">The number of columns to copy. Must be positive.</param>
        /// <returns>The requested sub-matrix.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If: <list><item><paramref name="rowIndex"/> is
        /// negative, or greater than or equal to the number of rows.</item>
        /// <item><paramref name="columnIndex"/> is negative, or greater than or equal to the number 
        /// of columns.</item>
        /// <item><c>(columnIndex + columnLength) &gt;= Columns</c></item>
        /// <item><c>(rowIndex + rowLength) &gt;= Rows</c></item></list></exception>        
        /// <exception cref="ArgumentException">If <paramref name="rowLength"/> or <paramref name="columnLength"/>
        /// is not positive.</exception>
        public override Matrix<Complex32> SubMatrix(int rowIndex, int rowLength, int columnIndex, int columnLength)
        {
            if (rowIndex >= RowCount || rowIndex < 0)
            {
                throw new ArgumentOutOfRangeException("rowIndex");
            }

            if (columnIndex >= ColumnCount || columnIndex < 0)
            {
                throw new ArgumentOutOfRangeException("columnIndex");
            }

            if (rowLength < 1)
            {
                throw new ArgumentException(Resources.ArgumentMustBePositive, "rowLength");
            }

            if (columnLength < 1)
            {
                throw new ArgumentException(Resources.ArgumentMustBePositive, "columnLength");
            }

            var colMax = columnIndex + columnLength;
            var rowMax = rowIndex + rowLength;

            if (rowMax > RowCount)
            {
                throw new ArgumentOutOfRangeException("rowLength");
            }

            if (colMax > ColumnCount)
            {
                throw new ArgumentOutOfRangeException("columnLength");
            }

            var result = new SparseMatrix(rowLength, columnLength);

            if (rowIndex > columnIndex && columnIndex + columnLength > rowIndex)
            {
                for (var i = 0; rowIndex - columnIndex + i < Math.Min(columnLength, rowLength); i++)
                {
                    result[i, rowIndex - columnIndex + i] = Data[rowIndex + i];
                }
            }
            else if (rowIndex < columnIndex && rowIndex + rowLength > columnIndex)
            {
                for (var i = 0; rowIndex - columnIndex + i < Math.Min(columnLength, rowLength); i++)
                {
                    result[columnIndex - rowIndex + i, i] = Data[columnIndex + i];
                }
            }
            else
            {
                for (var i = 0; i < Math.Min(columnLength, rowLength); i++)
                {
                    result[i, i] = Data[rowIndex + i];
                }
            }

            return result;
        }

        /// <summary>
        /// Returns this matrix as a multidimensional array.
        /// </summary>
        /// <returns>A multidimensional containing the values of this matrix.</returns>    
        public override Complex32[,] ToArray()
        {
            var result = new Complex32[RowCount, ColumnCount];
            for (var i = 0; i < Data.Length; i++)
            {
                result[i, i] = Data[i];
            }

            return result;
        }

        /// <summary>
        /// Creates a new  <see cref="SparseMatrix"/> and inserts the given column at the given index.
        /// </summary>
        /// <param name="columnIndex">The index of where to insert the column.</param>
        /// <param name="column">The column to insert.</param>
        /// <returns>A new <see cref="SparseMatrix"/> with the inserted column.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="column "/> is <see langword="null" />. </exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="columnIndex"/> is &lt; zero or &gt; the number of columns.</exception>
        /// <exception cref="ArgumentException">If the size of <paramref name="column"/> != the number of rows.</exception>
        public override Matrix<Complex32> InsertColumn(int columnIndex, Vector<Complex32> column)
        {
            if (column == null)
            {
                throw new ArgumentNullException("column");
            }

            if (columnIndex < 0 || columnIndex > ColumnCount)
            {
                throw new ArgumentOutOfRangeException("columnIndex");
            }

            if (column.Count != RowCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixSameRowDimension, "column");
            }

            var result = new SparseMatrix(RowCount, ColumnCount + 1);

            for (var i = 0; i < columnIndex; i++)
            {
                result.SetColumn(i, Column(i));
            }

            result.SetColumn(columnIndex, column);

            for (var i = columnIndex + 1; i < ColumnCount + 1; i++)
            {
                result.SetColumn(i, Column(i - 1));
            }

            return result;
        }

        /// <summary>
        /// Creates a new  <see cref="SparseMatrix"/> and inserts the given row at the given index.
        /// </summary>
        /// <param name="rowIndex">The index of where to insert the row.</param>
        /// <param name="row">The row to insert.</param>
        /// <returns>A new  <see cref="SparseMatrix"/> with the inserted column.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="row"/> is <see langword="null" />. </exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="rowIndex"/> is &lt; zero or &gt; the number of rows.</exception>
        /// <exception cref="ArgumentException">If the size of <paramref name="row"/> != the number of columns.</exception>
        public override Matrix<Complex32> InsertRow(int rowIndex, Vector<Complex32> row)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row");
            }

            if (rowIndex < 0 || rowIndex > RowCount)
            {
                throw new ArgumentOutOfRangeException("rowIndex");
            }

            if (row.Count != ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixSameRowDimension, "row");
            }

            var result = new SparseMatrix(RowCount + 1, ColumnCount);

            for (var i = 0; i < rowIndex; i++)
            {
                result.SetRow(i, Row(i));
            }

            result.SetRow(rowIndex, row);

            for (var i = rowIndex + 1; i < RowCount; i++)
            {
                result.SetRow(i, Row(i - 1));
            }

            return result;
        }

        /// <summary>
        /// Stacks this matrix on top of the given matrix and places the result into the result <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="lower">The matrix to stack this matrix upon.</param>
        /// <returns>The combined <see cref="SparseMatrix"/>.</returns>
        /// <exception cref="ArgumentNullException">If lower is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If <strong>upper.Columns != lower.Columns</strong>.</exception>
        public override Matrix<Complex32> Stack(Matrix<Complex32> lower)
        {
            if (lower == null)
            {
                throw new ArgumentNullException("lower");
            }

            if (lower.ColumnCount != ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixSameColumnDimension, "lower");
            }

            var result = new SparseMatrix(RowCount + lower.RowCount, ColumnCount);
            Stack(lower, result);
            return result;
        }

        /// <summary>
        /// Stacks this matrix on top of the given matrix and places the result into the result <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="lower">The matrix to stack this matrix upon.</param>
        /// <param name="result">The combined <see cref="SparseMatrix"/>.</param>
        /// <exception cref="ArgumentNullException">If lower is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If <strong>upper.Columns != lower.Columns</strong>.</exception>
        public override void Stack(Matrix<Complex32> lower, Matrix<Complex32> result)
        {
            if (lower == null)
            {
                throw new ArgumentNullException("lower");
            }

            if (lower.ColumnCount != ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixSameColumnDimension, "lower");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (result.RowCount != (RowCount + lower.RowCount) || result.ColumnCount != ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions, "result");
            }

            // Clear the result matrix
            result.Clear();

            // Copy the diagonal part into the result matrix.
            for (var i = 0; i < Data.Length; i++)
            {
                result[i, i] = Data[i];
            }

            // Copy the lower matrix into the result matrix.
            for (var i = 0; i < lower.RowCount; i++)
            {
                for (var j = 0; j < lower.ColumnCount; j++)
                {
                    result[i + RowCount, j] = lower[i, j];
                }
            }
        }

        /// <summary>
        ///  Concatenates this matrix with the given matrix.
        /// </summary>
        /// <param name="right">The matrix to concatenate.</param>
        /// <returns>The combined <see cref="SparseMatrix"/>.</returns>
        public override Matrix<Complex32> Append(Matrix<Complex32> right)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            if (right.RowCount != RowCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixSameRowDimension);
            }

            var result = new SparseMatrix(RowCount, ColumnCount + right.ColumnCount);
            Append(right, result);
            return result;
        }

        /// <summary>
        /// Concatenates this matrix with the given matrix and places the result into the result <see cref="SparseMatrix"/>.
        /// </summary>
        /// <param name="right">The matrix to concatenate.</param>
        /// <param name="result">The combined <see cref="SparseMatrix"/>.</param>
        public override void Append(Matrix<Complex32> right, Matrix<Complex32> result)
        {
            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            if (right.RowCount != RowCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixSameRowDimension);
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (result.ColumnCount != (ColumnCount + right.ColumnCount) || result.RowCount != RowCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixSameColumnDimension);
            }

            // Clear the result matrix
            result.Clear();

            // Copy the diagonal part into the result matrix.
            for (var i = 0; i < Data.Length; i++)
            {
                result[i, i] = Data[i];
            }

            // Copy the lower matrix into the result matrix.
            for (var i = 0; i < right.RowCount; i++)
            {
                for (var j = 0; j < right.ColumnCount; j++)
                {
                    result[i, j + RowCount] = right[i, j];
                }
            }
        }

        /// <summary>
        /// Diagonally stacks his matrix on top of the given matrix. The new matrix is a M-by-N matrix, 
        /// where M = this.Rows + lower.Rows and N = this.Columns + lower.Columns.
        /// The values of off the off diagonal matrices/blocks are set to zero.
        /// </summary>
        /// <param name="lower">The lower, right matrix.</param>
        /// <exception cref="ArgumentNullException">If lower is <see langword="null" />.</exception>
        /// <returns>the combined matrix</returns>
        public override Matrix<Complex32> DiagonalStack(Matrix<Complex32> lower)
        {
            if (lower == null)
            {
                throw new ArgumentNullException("lower");
            }

            var result = new SparseMatrix(RowCount + lower.RowCount, ColumnCount + lower.ColumnCount);
            DiagonalStack(lower, result);
            return result;
        }

        /// <summary>
        /// Diagonally stacks his matrix on top of the given matrix and places the combined matrix into the result matrix.
        /// </summary>
        /// <param name="lower">The lower, right matrix.</param>
        /// <param name="result">The combined matrix</param>
        /// <exception cref="ArgumentNullException">If lower is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException">If the result matrix is <see langword="null" />.</exception>
        /// <exception cref="ArgumentException">If the result matrix's dimensions are not (this.Rows + lower.rows) x (this.Columns + lower.Columns).</exception>
        public override void DiagonalStack(Matrix<Complex32> lower, Matrix<Complex32> result)
        {
            if (lower == null)
            {
                throw new ArgumentNullException("lower");
            }

            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (result.RowCount != RowCount + lower.RowCount || result.ColumnCount != ColumnCount + lower.ColumnCount)
            {
                throw new ArgumentException(Resources.ArgumentMatrixDimensions, "result");
            }

            // Clear the result matrix
            result.Clear();

            // Copy the diagonal part into the result matrix.
            for (var i = 0; i < Data.Length; i++)
            {
                result[i, i] = Data[i];
            }

            // Copy the lower matrix into the result matrix.
            CommonParallel.For(0, lower.RowCount, i => CommonParallel.For(0, lower.ColumnCount, j => result.At(i + RowCount, j + ColumnCount, lower.At(i, j))));
        }

        /// <summary>
        /// Permute the columns of a matrix according to a permutation.
        /// </summary>
        /// <param name="p">The column permutation to apply to this matrix.</param>
        /// <exception cref="InvalidOperationException">Always thrown</exception>
        /// <remarks>Permutation in diagonal matrix are senseless, because of matrix nature</remarks>
        public override void PermuteColumns(Permutation p)
        {
            throw new InvalidOperationException("Permutations in diagonal matrix are not allowed");
        }

        /// <summary>
        /// Permute the rows of a matrix according to a permutation.
        /// </summary>
        /// <param name="p">The row permutation to apply to this matrix.</param>
        /// <exception cref="InvalidOperationException">Always thrown</exception>
        /// <remarks>Permutation in diagonal matrix are senseless, because of matrix nature</remarks>
        public override void PermuteRows(Permutation p)
        {
            throw new InvalidOperationException("Permutations in diagonal matrix are not allowed");
        }

        #region Static constructors for special matrices.

        /// <summary>
        /// Initializes a square <see cref="DiagonalMatrix"/> with all zero's except for ones on the diagonal.
        /// </summary>
        /// <param name="order">the size of the square matrix.</param>
        /// <returns>A diagonal identity matrix.</returns>
        /// <exception cref="ArgumentException">
        /// If <paramref name="order"/> is less than one.
        /// </exception>
        public static DiagonalMatrix Identity(int order)
        {
            var m = new DiagonalMatrix(order);
            for (var i = 0; i < order; i++)
            {
                m.Data[i] = 1.0f;
            }

            return m;
        }

        #endregion
    }
}
