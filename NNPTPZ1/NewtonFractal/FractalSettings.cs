using System;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1.NewtonFractal
{
    /// <summary>
    /// Image size, rendered area of the complex plane and output file of the fractal.
    /// </summary>
    public sealed class FractalSettings
    {
        public const string DefaultOutputPath = "../../../out.png";

        public FractalSettings(
            int width, int height, double xMin, double xMax, double yMin, double yMax, string outputPath)
        {
            if (width <= 0)
            {
                throw new ArgumentException("Width must be positive.", nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentException("Height must be positive.", nameof(height));
            }

            if (!IsFinite(xMin) || !IsFinite(xMax) || !IsFinite(yMin) || !IsFinite(yMax))
            {
                throw new ArgumentException("Bounds of the rendered area must be finite numbers.");
            }

            if (xMin >= xMax)
            {
                throw new ArgumentException("xMin must be less than xMax.", nameof(xMin));
            }

            if (yMin >= yMax)
            {
                throw new ArgumentException("yMin must be less than yMax.", nameof(yMin));
            }

            Width = width;
            Height = height;
            XMin = xMin;
            XMax = xMax;
            YMin = yMin;
            YMax = yMax;
            OutputPath = string.IsNullOrWhiteSpace(outputPath) ? DefaultOutputPath : outputPath;
        }

        public int Width { get; }

        public int Height { get; }

        public double XMin { get; }

        public double XMax { get; }

        public double YMin { get; }

        public double YMax { get; }

        public string OutputPath { get; }

        public double XStep => (XMax - XMin) / Width;

        public double YStep => (YMax - YMin) / Height;

        /// <summary>
        /// Maps a pixel of the image to the point of the complex plane.
        /// </summary>
        public ComplexNumber GetPointForPixel(int column, int row)
        {
            return new ComplexNumber(XMin + column * XStep, YMin + row * YStep);
        }

        private static bool IsFinite(double value)
        {
            return !double.IsNaN(value) && !double.IsInfinity(value);
        }
    }
}
