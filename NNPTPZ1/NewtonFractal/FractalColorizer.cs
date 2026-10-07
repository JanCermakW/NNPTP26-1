using System;
using System.Drawing;

namespace NNPTPZ1.NewtonFractal
{
    /// <summary>
    /// Colors a pixel by the root it converged to, darker the more iterations it took.
    /// </summary>
    public static class FractalColorizer
    {
        private const int DarkeningPerIteration = 2;

        public static readonly Color NotConvergedColor = Color.Black;

        private static readonly Color[] Palette =
        {
            Color.Red, Color.Blue, Color.Green, Color.Yellow, Color.Orange,
            Color.Fuchsia, Color.Gold, Color.Cyan, Color.Magenta
        };

        public static Color GetRootColor(int rootIndex, int iterations)
        {
            Color baseColor = Palette[rootIndex % Palette.Length];
            int darkening = iterations * DarkeningPerIteration;

            return Color.FromArgb(
                Darken(baseColor.R, darkening),
                Darken(baseColor.G, darkening),
                Darken(baseColor.B, darkening));
        }

        private static int Darken(int channel, int darkening)
        {
            return Math.Max(0, channel - darkening);
        }
    }
}
