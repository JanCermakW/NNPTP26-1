using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NNPTPZ1.Mathematics.Tests
{
    [TestClass]
    public class PolynomialTests
    {
        // 1 + x^2
        private static Polynomial CreateQuadratic()
        {
            return new Polynomial(new ComplexNumber(1, 0), ComplexNumber.Zero, new ComplexNumber(1, 0));
        }

        [DataTestMethod]
        [DataRow(0, 1)]
        [DataRow(1, 2)]
        [DataRow(2, 5)]
        [DataRow(-3, 10)]
        public void Evaluate_RealPoint_ReturnsValue(double x, double expected)
        {
            Assert.AreEqual(new ComplexNumber(expected, 0), CreateQuadratic().Evaluate(new ComplexNumber(x, 0)));
            Assert.AreEqual(new ComplexNumber(expected, 0), CreateQuadratic().Evaluate(x));
        }

        [TestMethod]
        public void Evaluate_AtRoot_ReturnsZero()
        {
            Assert.AreEqual(ComplexNumber.Zero, CreateQuadratic().Evaluate(new ComplexNumber(0, 1)));
        }

        [TestMethod]
        public void Evaluate_ComplexCoefficients_ReturnsValue()
        {
            // (1 + i) + (2 - i)x at x = i: 1 + i + 2i + 1 = 2 + 3i
            var polynomial = new Polynomial(new ComplexNumber(1, 1), new ComplexNumber(2, -1));

            Assert.AreEqual(new ComplexNumber(2, 3), polynomial.Evaluate(new ComplexNumber(0, 1)));
        }

        [TestMethod]
        public void Evaluate_WithoutCoefficients_ReturnsZero()
        {
            Assert.AreEqual(ComplexNumber.Zero, new Polynomial().Evaluate(5));
        }

        [TestMethod]
        public void Derive_ReturnsDerivative()
        {
            // (1 + x^3)' = 3x^2
            var polynomial = new Polynomial(
                new ComplexNumber(1, 0), ComplexNumber.Zero, ComplexNumber.Zero, new ComplexNumber(1, 0));

            Polynomial derivative = polynomial.Derive();

            CollectionAssert.AreEqual(
                new[] { ComplexNumber.Zero, ComplexNumber.Zero, new ComplexNumber(3, 0) },
                derivative.Coefficients.ToList());
        }

        [TestMethod]
        public void Derive_Constant_ReturnsEmptyPolynomial()
        {
            Assert.AreEqual(0, new Polynomial(new ComplexNumber(7, 0)).Derive().Coefficients.Count);
        }

        [TestMethod]
        public void ToString_ReturnsAllTerms()
        {
            Assert.AreEqual("(1 + 0i) + (0 + 0i)x + (1 + 0i)xx", CreateQuadratic().ToString());
        }

        [TestMethod]
        public void Coefficients_AreReadOnlyCopy()
        {
            var source = new[] { new ComplexNumber(1, 0), new ComplexNumber(2, 0) };
            var polynomial = new Polynomial(source);

            source[0] = new ComplexNumber(5, 0);

            Assert.AreEqual(new ComplexNumber(1, 0), polynomial.Coefficients[0]);
            Assert.IsTrue(((ICollection<ComplexNumber>)polynomial.Coefficients).IsReadOnly);
        }

        [TestMethod]
        public void Constructor_NullCoefficients_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new Polynomial((ComplexNumber[])null));
        }
    }
}
