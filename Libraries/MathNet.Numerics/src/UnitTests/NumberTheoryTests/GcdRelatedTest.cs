// <copyright file="GcdRelatedTest.cs" company="Math.NET">
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

namespace MathNet.Numerics.UnitTests.NumberTheoryTests
{
    using System;
    using MbUnit.Framework;
    using NumberTheory;

    [TestFixture]
    public class GcdRelatedTest
    {
        [Test]
        public void GcdHandlesNormalInputCorrectly()
        {
            Assert.AreEqual(0, IntegerTheory.GreatestCommonDivisor(0, 0), "Gcd(0,0)");
            Assert.AreEqual(6, IntegerTheory.GreatestCommonDivisor(0, 6), "Gcd(0,6)");
            Assert.AreEqual(1, IntegerTheory.GreatestCommonDivisor(7, 13), "Gcd(7,13)");
            Assert.AreEqual(7, IntegerTheory.GreatestCommonDivisor(7, 14), "Gcd(7,14)");
            Assert.AreEqual(1, IntegerTheory.GreatestCommonDivisor(7, 15), "Gcd(7,15)");
            Assert.AreEqual(3, IntegerTheory.GreatestCommonDivisor(6, 15), "Gcd(6,15)");
        }

        [Test]
        public void GcdHandlesNegativeInputCorrectly()
        {
            Assert.AreEqual(5, IntegerTheory.GreatestCommonDivisor(-5, 0), "Gcd(-5,0)");
            Assert.AreEqual(5, IntegerTheory.GreatestCommonDivisor(0, -5), "Gcd(0, -5)");
            Assert.AreEqual(1, IntegerTheory.GreatestCommonDivisor(-7, 15), "Gcd(-7,15)");
            Assert.AreEqual(1, IntegerTheory.GreatestCommonDivisor(-7, -15), "Gcd(-7,-15)");
        }

        [Test]
        public void GcdSupportsLargeInput()
        {
            Assert.AreEqual(Int32.MaxValue, IntegerTheory.GreatestCommonDivisor(0, Int32.MaxValue), "Gcd(0,Int32Max)");
            Assert.AreEqual(Int64.MaxValue, IntegerTheory.GreatestCommonDivisor(0, Int64.MaxValue), "Gcd(0,Int64Max)");
            Assert.AreEqual(1, IntegerTheory.GreatestCommonDivisor(Int32.MaxValue, Int64.MaxValue), "Gcd(Int32Max,Int64Max)");
            Assert.AreEqual(1 << 18, IntegerTheory.GreatestCommonDivisor(1 << 18, 1 << 20), "Gcd(1>>18,1<<20)");
        }

        [Test]
        public void ExtendedGcdHandlesNormalInputCorrectly()
        {
            long x, y;

            Assert.AreEqual(3, IntegerTheory.ExtendedGreatestCommonDivisor(6, 15, out x, out y), "Egcd(6,15)");
            Assert.AreEqual(3, 6 * x + 15 * y, "Egcd(6,15) -> a*x+b*y");

            Assert.AreEqual(3, IntegerTheory.ExtendedGreatestCommonDivisor(-6, 15, out x, out y), "Egcd(-6,15)");
            Assert.AreEqual(3, -6 * x + 15 * y, "Egcd(-6,15) -> a*x+b*y");

            Assert.AreEqual(3, IntegerTheory.ExtendedGreatestCommonDivisor(-6, -15, out x, out y), "Egcd(-6,-15)");
            Assert.AreEqual(3, -6 * x + -15 * y, "Egcd(-6,-15) -> a*x+b*y");
        }

        [Test]
        public void ListGcdHandlesNormalInputCorrectly()
        {
            Assert.AreEqual(2, IntegerTheory.GreatestCommonDivisor(-10, 6, -8), "Gcd(-10,6,-8)");
            Assert.AreEqual(1, IntegerTheory.GreatestCommonDivisor(-10, 6, -8, 5, 9, 13), "Gcd(-10,6,-8,5,9,13)");
            Assert.AreEqual(5, IntegerTheory.GreatestCommonDivisor(-10, 20, 120, 60, -15, 1000), "Gcd(-10,20,120,60,-15,1000)");
            Assert.AreEqual(3, IntegerTheory.GreatestCommonDivisor(Int64.MaxValue - 1, Int64.MaxValue - 4, Int64.MaxValue - 7), "Gcd(Int64Max-1,Int64Max-4,Int64Max-7)");
            Assert.AreEqual(123, IntegerTheory.GreatestCommonDivisor(492, -2 * 492, 492 / 4), "Gcd(492, -984, 123)");
        }

        [Test]
        public void ListGcdHandlesSpecialInputCorrectly()
        {
            Assert.AreEqual(0, IntegerTheory.GreatestCommonDivisor(new long[0]), "Gcd()");
            Assert.AreEqual(100, IntegerTheory.GreatestCommonDivisor(-100), "Gcd(-100)");
        }

        [Test]
        public void ListGcdChecksForNullArguments()
        {
            Assert.Throws(
                typeof (ArgumentNullException),
                () => IntegerTheory.GreatestCommonDivisor((long[])null));
        }

        [Test]
        public void LcmHandlesNormalInputCorrectly()
        {
            Assert.AreEqual(10, IntegerTheory.LeastCommonMultiple(10, 10), "Lcm(10,10)");

            Assert.AreEqual(0, IntegerTheory.LeastCommonMultiple(0, 10), "Lcm(0,10)");
            Assert.AreEqual(0, IntegerTheory.LeastCommonMultiple(10, 0), "Lcm(10,0)");

            Assert.AreEqual(77, IntegerTheory.LeastCommonMultiple(11, 7), "Lcm(11,7)");
            Assert.AreEqual(33, IntegerTheory.LeastCommonMultiple(11, 33), "Lcm(11,33)");
            Assert.AreEqual(374, IntegerTheory.LeastCommonMultiple(11, 34), "Lcm(11,34)");
        }

        [Test]
        public void LcmHandlesNegativeInputCorrectly()
        {
            Assert.AreEqual(352, IntegerTheory.LeastCommonMultiple(11, -32), "Lcm(11,-32)");
            Assert.AreEqual(352, IntegerTheory.LeastCommonMultiple(-11, 32), "Lcm(-11,32)");
            Assert.AreEqual(352, IntegerTheory.LeastCommonMultiple(-11, -32), "Lcm(-11,-32)");
        }

        [Test]
        public void LcmSupportsLargeInput()
        {
            Assert.AreEqual(Int32.MaxValue, IntegerTheory.LeastCommonMultiple(Int32.MaxValue, Int32.MaxValue), "Lcm(Int32Max,Int32Max)");
            Assert.AreEqual(Int64.MaxValue, IntegerTheory.LeastCommonMultiple(Int64.MaxValue, Int64.MaxValue), "Lcm(Int64Max,Int64Max)");
            Assert.AreEqual(Int64.MaxValue, IntegerTheory.LeastCommonMultiple(-Int64.MaxValue, -Int64.MaxValue), "Lcm(-Int64Max,-Int64Max)");
            Assert.AreEqual(Int64.MaxValue, IntegerTheory.LeastCommonMultiple(-Int64.MaxValue, Int64.MaxValue), "Lcm(-Int64Max,Int64Max)");
        }

        [Test]
        public void ListLcmHandlesNormalInputCorrectly()
        {
            Assert.AreEqual(120, IntegerTheory.LeastCommonMultiple(-10, 6, -8), "Lcm(-10,6,-8)");
            Assert.AreEqual(4680, IntegerTheory.LeastCommonMultiple(-10, 6, -8, 5, 9, 13), "Lcm(-10,6,-8,5,9,13)");
            Assert.AreEqual(3000, IntegerTheory.LeastCommonMultiple(-10, 20, 120, 60, -15, 1000), "Lcm(-10,20,120,60,-15,1000)");
            Assert.AreEqual(984, IntegerTheory.LeastCommonMultiple(492, -2 * 492, 492 / 4), "Lcm(492, -984, 123)");
            Assert.AreEqual(2016, IntegerTheory.LeastCommonMultiple(32, 42, 36, 18), "Lcm(32,42,36,18)");
        }

        [Test]
        public void ListLcmHandlesSpecialInputCorrectly()
        {
            Assert.AreEqual(1, IntegerTheory.LeastCommonMultiple(new long[0]), "Lcm()");
            Assert.AreEqual(100, IntegerTheory.LeastCommonMultiple(-100), "Lcm(-100)");
        }

        [Test]
        public void ListLcmChecksForNullArguments()
        {
            Assert.Throws(
                typeof(ArgumentNullException),
                () => IntegerTheory.LeastCommonMultiple((long[])null));
        }
    }
}
