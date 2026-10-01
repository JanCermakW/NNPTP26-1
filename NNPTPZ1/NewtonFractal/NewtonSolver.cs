using System;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1.NewtonFractal
{
    /// <summary>
    /// Finds a root of a polynomial using Newton's iteration <c>x = x - p(x) / p'(x)</c>.
    /// </summary>
    public sealed class NewtonSolver
    {
        /// <summary>Number of small steps that must be made before the result is accepted.</summary>
        public const int RequiredSmallSteps = 30;

        /// <summary>Upper bound of all steps, protects against points that never converge.</summary>
        public const int MaxIterations = 1000;

        /// <summary>Squared step size from which a step is considered large.</summary>
        private const double LargeStepThreshold = 0.5;

        /// <summary>Replacement of zero components of the initial guess, where the derivative may vanish.</summary>
        private const double ZeroReplacement = 0.0001;

        private readonly Polynomial polynomial;
        private readonly Polynomial derivative;

        public NewtonSolver(Polynomial polynomial)
        {
            this.polynomial = polynomial ?? throw new ArgumentNullException(nameof(polynomial));
            derivative = polynomial.Derive();
        }

        public NewtonResult FindRoot(ComplexNumber initialGuess)
        {
            ComplexNumber current = AvoidZeroComponents(initialGuess);
            int smallSteps = 0;
            int iterations = 0;

            while (smallSteps < RequiredSmallSteps && iterations < MaxIterations)
            {
                ComplexNumber step = polynomial.Evaluate(current).Divide(derivative.Evaluate(current));
                current = current.Subtract(step);
                iterations++;

                if (IsSmallStep(step))
                {
                    smallSteps++;
                }
            }

            return new NewtonResult(current, iterations, smallSteps == RequiredSmallSteps);
        }

        /// <summary>
        /// A step is small when it is below the threshold or undefined (NaN when the derivative is zero).
        /// </summary>
        private static bool IsSmallStep(ComplexNumber step)
        {
            double squaredStepSize = step.GetSquaredAbsoluteValue();

            return double.IsNaN(squaredStepSize) || squaredStepSize < LargeStepThreshold;
        }

        private static ComplexNumber AvoidZeroComponents(ComplexNumber point)
        {
            double realPart = point.RealPart == 0 ? ZeroReplacement : point.RealPart;
            double imaginaryPart = point.ImaginaryPart == 0 ? ZeroReplacement : point.ImaginaryPart;

            return new ComplexNumber(realPart, imaginaryPart);
        }
    }
}
