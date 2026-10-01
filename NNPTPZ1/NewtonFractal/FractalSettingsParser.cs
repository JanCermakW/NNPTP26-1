using System;
using System.Globalization;

namespace NNPTPZ1.NewtonFractal
{
    /// <summary>
    /// Creates <see cref="FractalSettings"/> from command line arguments.
    /// </summary>
    public static class FractalSettingsParser
    {
        public const string Usage =
            "Usage: NNPTPZ1 <width> <height> <xMin> <xMax> <yMin> <yMax> [outputPath]";

        private const int RequiredArgumentCount = 6;
        private const int MaxArgumentCount = RequiredArgumentCount + 1;

        /// <summary>
        /// Parses the arguments, numbers are expected in invariant culture (decimal point).
        /// </summary>
        /// <exception cref="ArgumentException">Arguments are missing or invalid.</exception>
        public static FractalSettings Parse(string[] args)
        {
            if (args == null || args.Length < RequiredArgumentCount || args.Length > MaxArgumentCount)
            {
                throw new ArgumentException(
                    $"Expected {RequiredArgumentCount} or {MaxArgumentCount} arguments, got {args?.Length ?? 0}.");
            }

            return new FractalSettings(
                ParseInt(args[0], "width"),
                ParseInt(args[1], "height"),
                ParseDouble(args[2], "xMin"),
                ParseDouble(args[3], "xMax"),
                ParseDouble(args[4], "yMin"),
                ParseDouble(args[5], "yMax"),
                args.Length > RequiredArgumentCount ? args[RequiredArgumentCount] : null);
        }

        private static int ParseInt(string value, string name)
        {
            int result;
            if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                throw new ArgumentException($"Argument '{name}' must be an integer, got '{value}'.");
            }

            return result;
        }

        private static double ParseDouble(string value, string name)
        {
            double result;
            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                throw new ArgumentException($"Argument '{name}' must be a number, got '{value}'.");
            }

            return result;
        }
    }
}
