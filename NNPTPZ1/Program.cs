using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using NNPTPZ1.Mathematics;
using NNPTPZ1.NewtonFractal;

namespace NNPTPZ1
{
    /// <summary>
    /// Produces an image of the Newton fractal of polynomial <c>x^3 + 1</c>.
    /// </summary>
    internal static class Program
    {
        private const int SuccessExitCode = 0;
        private const int InvalidArgumentsExitCode = 1;
        private const int ImageFailedExitCode = 2;

        private static int Main(string[] args)
        {
            FractalSettings settings;
            try
            {
                settings = FractalSettingsParser.Parse(args);
            }
            catch (ArgumentException exception)
            {
                Console.Error.WriteLine(exception.Message);
                Console.Error.WriteLine(FractalSettingsParser.Usage);
                return InvalidArgumentsExitCode;
            }

            Polynomial polynomial = CreatePolynomial();
            Console.WriteLine(polynomial);
            Console.WriteLine(polynomial.Derive());

            var renderer = new NewtonFractalRenderer(settings, polynomial);
            try
            {
                using (Bitmap bitmap = renderer.Render())
                {
                    bitmap.Save(settings.OutputPath, ImageFormat.Png);
                }
            }
            catch (Exception exception) when (IsImageFailure(exception))
            {
                Console.Error.WriteLine($"Cannot create image '{settings.OutputPath}': {exception.Message}");
                return ImageFailedExitCode;
            }

            return SuccessExitCode;
        }

        /// <summary>
        /// Failures reported by GDI+ when the bitmap is too large or the output path is invalid.
        /// </summary>
        private static bool IsImageFailure(Exception exception)
        {
            return exception is ExternalException
                || exception is ArgumentException
                || exception is NotSupportedException
                || exception is OutOfMemoryException;
        }

        private static Polynomial CreatePolynomial()
        {
            return new Polynomial(
                new ComplexNumber(1, 0),
                ComplexNumber.Zero,
                ComplexNumber.Zero,
                new ComplexNumber(1, 0));
        }
    }
}
