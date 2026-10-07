using System;
using System.Globalization;

namespace NNPTPZ1.Mathematics
{
    /// <summary>
    /// Immutable complex number in the algebraic form <c>a + bi</c>.
    /// </summary>
    public readonly struct ComplexNumber : IEquatable<ComplexNumber>
    {
        public static readonly ComplexNumber Zero = new ComplexNumber(0, 0);

        public double RealPart { get; }

        public double ImaginaryPart { get; }

        public bool IsNaN => double.IsNaN(RealPart) || double.IsNaN(ImaginaryPart);

        public ComplexNumber(double realPart, double imaginaryPart)
        {
            RealPart = realPart;
            ImaginaryPart = imaginaryPart;
        }

        public ComplexNumber Add(ComplexNumber other)
        {
            return new ComplexNumber(RealPart + other.RealPart, ImaginaryPart + other.ImaginaryPart);
        }

        public ComplexNumber Subtract(ComplexNumber other)
        {
            return new ComplexNumber(RealPart - other.RealPart, ImaginaryPart - other.ImaginaryPart);
        }

        public ComplexNumber Multiply(ComplexNumber other)
        {
            // (a + bi)(c + di) = (ac - bd) + (ad + bc)i
            return new ComplexNumber(
                RealPart * other.RealPart - ImaginaryPart * other.ImaginaryPart,
                RealPart * other.ImaginaryPart + ImaginaryPart * other.RealPart);
        }

        public ComplexNumber Divide(ComplexNumber other)
        {
            // (a + bi) / (c + di) = ((a + bi)(c - di)) / (c^2 + d^2)
            ComplexNumber numerator = Multiply(other.Conjugate());
            double denominator = other.GetSquaredAbsoluteValue();

            return new ComplexNumber(numerator.RealPart / denominator, numerator.ImaginaryPart / denominator);
        }

        public ComplexNumber Conjugate()
        {
            return new ComplexNumber(RealPart, -ImaginaryPart);
        }

        public double GetSquaredAbsoluteValue()
        {
            return RealPart * RealPart + ImaginaryPart * ImaginaryPart;
        }

        public double GetAbsoluteValue()
        {
            return Math.Sqrt(GetSquaredAbsoluteValue());
        }

        public double GetAngleInRadians()
        {
            return Math.Atan2(ImaginaryPart, RealPart);
        }

        public double GetSquaredDistanceTo(ComplexNumber other)
        {
            return Subtract(other).GetSquaredAbsoluteValue();
        }

        public bool Equals(ComplexNumber other)
        {
            return RealPart.Equals(other.RealPart) && ImaginaryPart.Equals(other.ImaginaryPart);
        }

        public override bool Equals(object obj)
        {
            return obj is ComplexNumber other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (RealPart.GetHashCode() * 397) ^ ImaginaryPart.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0} + {1}i)", RealPart, ImaginaryPart);
        }
    }
}
