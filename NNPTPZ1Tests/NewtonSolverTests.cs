using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1.NewtonFractal.Tests
{
    [TestClass]
    public class NewtonSolverTests
    {
        private const double RootTolerance = 1e-6;

        // x^3 + 1, roots: -1, 1/2 +- (sqrt(3)/2)i
        private static readonly Polynomial Cubic = new Polynomial(
            new ComplexNumber(1, 0), ComplexNumber.Zero, ComplexNumber.Zero, new ComplexNumber(1, 0));

        // x^3 - 2x + 2, Newton's iteration started near 0 cycles between 0 and 1 forever
        private static readonly Polynomial CyclingCubic = new Polynomial(
            new ComplexNumber(2, 0), new ComplexNumber(-2, 0), ComplexNumber.Zero, new ComplexNumber(1, 0));

        [DataTestMethod]
        [DataRow(-2, 0, -1, 0)]
        [DataRow(1, 2, 0.5, 0.8660254037844386)]
        [DataRow(1, -2, 0.5, -0.8660254037844386)]
        public void FindRoot_ConvergesToNearestRoot(
            double startReal, double startImaginary, double rootReal, double rootImaginary)
        {
            NewtonResult result = new NewtonSolver(Cubic).FindRoot(new ComplexNumber(startReal, startImaginary));

            Assert.IsTrue(result.HasConverged);
            Assert.AreEqual(rootReal, result.Root.RealPart, RootTolerance);
            Assert.AreEqual(rootImaginary, result.Root.ImaginaryPart, RootTolerance);
            Assert.AreEqual(0, Cubic.Evaluate(result.Root).GetAbsoluteValue(), RootTolerance);
        }

        [TestMethod]
        public void FindRoot_StartingAtRoot_MakesOnlyRequiredSteps()
        {
            NewtonResult result = new NewtonSolver(Cubic).FindRoot(new ComplexNumber(-1, 0));

            Assert.AreEqual(NewtonSolver.RequiredSmallSteps, result.Iterations);
        }

        [TestMethod]
        public void FindRoot_StartingFarFromRoot_NeedsMoreIterations()
        {
            var solver = new NewtonSolver(Cubic);

            NewtonResult nearRoot = solver.FindRoot(new ComplexNumber(-1, 0));
            NewtonResult farFromRoot = solver.FindRoot(new ComplexNumber(100, 100));

            Assert.IsTrue(farFromRoot.Iterations > nearRoot.Iterations);
        }

        [TestMethod]
        public void FindRoot_NeverConverging_StopsAtIterationLimit()
        {
            NewtonResult result = new NewtonSolver(CyclingCubic).FindRoot(ComplexNumber.Zero);

            Assert.IsFalse(result.HasConverged);
            Assert.AreEqual(NewtonSolver.MaxIterations, result.Iterations);
        }

        [TestMethod]
        public void FindRoot_ZeroDerivative_TreatsUndefinedStepsAsSmall()
        {
            // Derivative of a constant polynomial is zero, so every step is NaN.
            var constant = new Polynomial(new ComplexNumber(1, 0));

            NewtonResult result = new NewtonSolver(constant).FindRoot(ComplexNumber.Zero);

            Assert.AreEqual(NewtonSolver.RequiredSmallSteps, result.Iterations);
            Assert.IsTrue(double.IsNaN(result.Root.RealPart));
        }

        [TestMethod]
        public void Constructor_NullPolynomial_Throws()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new NewtonSolver(null));
        }
    }
}
