using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NNPTPZ1.Mathematics.Tests
{
    [TestClass]
    public class ComplexNumberTests
    {
        private const double Delta = 1e-12;

        [TestMethod]
        public void Add_ReturnsSumOfParts()
        {
            var a = new ComplexNumber(10, 20);
            var b = new ComplexNumber(1, 2);

            Assert.AreEqual(new ComplexNumber(11, 22), a.Add(b));
        }

        [TestMethod]
        public void Add_Zero_ReturnsSameValue()
        {
            var a = new ComplexNumber(1, -1);

            Assert.AreEqual(a, a.Add(ComplexNumber.Zero));
        }

        [TestMethod]
        public void Subtract_ReturnsDifferenceOfParts()
        {
            var a = new ComplexNumber(10, 20);
            var b = new ComplexNumber(1, 2);

            Assert.AreEqual(new ComplexNumber(9, 18), a.Subtract(b));
        }

        [TestMethod]
        public void Multiply_ReturnsProduct()
        {
            var a = new ComplexNumber(1, 2);
            var b = new ComplexNumber(3, 4);

            // (1 + 2i)(3 + 4i) = 3 + 4i + 6i + 8i^2 = -5 + 10i
            Assert.AreEqual(new ComplexNumber(-5, 10), a.Multiply(b));
        }

        [TestMethod]
        public void Multiply_ImaginaryUnitSquared_ReturnsMinusOne()
        {
            var i = new ComplexNumber(0, 1);

            Assert.AreEqual(new ComplexNumber(-1, 0), i.Multiply(i));
        }

        [TestMethod]
        public void Divide_ReturnsQuotient()
        {
            var a = new ComplexNumber(-5, 10);
            var b = new ComplexNumber(3, 4);

            Assert.AreEqual(new ComplexNumber(1, 2), a.Divide(b));
        }

        [TestMethod]
        public void Divide_ByZero_ReturnsNaN()
        {
            ComplexNumber result = new ComplexNumber(1, 1).Divide(ComplexNumber.Zero);

            Assert.IsTrue(double.IsNaN(result.RealPart));
            Assert.IsTrue(double.IsNaN(result.ImaginaryPart));
        }

        [TestMethod]
        public void Conjugate_NegatesImaginaryPart()
        {
            Assert.AreEqual(new ComplexNumber(3, -4), new ComplexNumber(3, 4).Conjugate());
        }

        [TestMethod]
        public void GetAbsoluteValue_ReturnsMagnitude()
        {
            var number = new ComplexNumber(3, 4);

            Assert.AreEqual(25, number.GetSquaredAbsoluteValue(), Delta);
            Assert.AreEqual(5, number.GetAbsoluteValue(), Delta);
        }

        [TestMethod]
        public void GetSquaredDistanceTo_ReturnsSquaredDistance()
        {
            var a = new ComplexNumber(1, 1);
            var b = new ComplexNumber(4, 5);

            Assert.AreEqual(25, a.GetSquaredDistanceTo(b), Delta);
        }

        [DataTestMethod]
        [DataRow(1, 0, 0)]
        [DataRow(0, 1, Math.PI / 2)]
        [DataRow(-1, 0, Math.PI)]
        [DataRow(0, -1, -Math.PI / 2)]
        [DataRow(-1, -1, -3 * Math.PI / 4)]
        public void GetAngleInRadians_ReturnsAngleInAllQuadrants(double realPart, double imaginaryPart, double expected)
        {
            Assert.AreEqual(expected, new ComplexNumber(realPart, imaginaryPart).GetAngleInRadians(), Delta);
        }

        [DataTestMethod]
        [DataRow(double.NaN, 0, true)]
        [DataRow(0, double.NaN, true)]
        [DataRow(1, 2, false)]
        public void IsNaN_DetectsUndefinedParts(double realPart, double imaginaryPart, bool expected)
        {
            Assert.AreEqual(expected, new ComplexNumber(realPart, imaginaryPart).IsNaN);
        }

        [TestMethod]
        public void Equals_ComparesValues()
        {
            var a = new ComplexNumber(1, 2);
            var b = new ComplexNumber(1, 2);

            Assert.IsTrue(a.Equals(b));
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
            Assert.IsFalse(a.Equals(new ComplexNumber(2, 1)));
            Assert.IsFalse(a.Equals(null));
            Assert.IsFalse(a.Equals("(1 + 2i)"));
        }

        [DataTestMethod]
        [DataRow(10, 20, "(10 + 20i)")]
        [DataRow(1, -1, "(1 + -1i)")]
        [DataRow(0, 0, "(0 + 0i)")]
        [DataRow(1.5, 0.25, "(1.5 + 0.25i)")]
        public void ToString_ReturnsAlgebraicForm(double realPart, double imaginaryPart, string expected)
        {
            Assert.AreEqual(expected, new ComplexNumber(realPart, imaginaryPart).ToString());
        }
    }
}
