using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NNPTPZ1.Mathematics
{
    /// <summary>
    /// Polynomial with complex coefficients, ordered from the lowest power:
    /// <c>c0 + c1*x + c2*x^2 + ...</c>
    /// </summary>
    public sealed class Polynomial
    {
        private readonly IReadOnlyList<ComplexNumber> coefficients;

        public IReadOnlyList<ComplexNumber> Coefficients => coefficients;

        public Polynomial(params ComplexNumber[] coefficients)
            : this((IEnumerable<ComplexNumber>)coefficients)
        {
        }

        public Polynomial(IEnumerable<ComplexNumber> coefficients)
        {
            if (coefficients == null)
            {
                throw new ArgumentNullException(nameof(coefficients));
            }

            this.coefficients = coefficients.ToList().AsReadOnly();
        }

        public Polynomial Derive()
        {
            var derivedCoefficients = new List<ComplexNumber>();
            for (int power = 1; power < coefficients.Count; power++)
            {
                derivedCoefficients.Add(coefficients[power].Multiply(new ComplexNumber(power, 0)));
            }

            return new Polynomial(derivedCoefficients);
        }

        public ComplexNumber Evaluate(double x)
        {
            return Evaluate(new ComplexNumber(x, 0));
        }

        public ComplexNumber Evaluate(ComplexNumber x)
        {
            // Horner's method: c0 + x(c1 + x(c2 + ...))
            ComplexNumber result = ComplexNumber.Zero;
            for (int power = coefficients.Count - 1; power >= 0; power--)
            {
                result = result.Multiply(x).Add(coefficients[power]);
            }

            return result;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (int power = 0; power < coefficients.Count; power++)
            {
                if (power > 0)
                {
                    builder.Append(" + ");
                }

                builder.Append(coefficients[power]);
                builder.Append('x', power);
            }

            return builder.ToString();
        }
    }
}
