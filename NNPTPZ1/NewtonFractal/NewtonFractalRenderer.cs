using System;
using System.Collections.Generic;
using System.Drawing;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1.NewtonFractal
{
    /// <summary>
    /// Renders the Newton fractal of a polynomial, see https://en.wikipedia.org/wiki/Newton_fractal
    /// </summary>
    public sealed class NewtonFractalRenderer
    {
        private readonly FractalSettings settings;
        private readonly NewtonSolver solver;
        private readonly RootRegistry rootRegistry = new RootRegistry();

        /// <summary>Distinct roots found while computing the pixels so far.</summary>
        public IReadOnlyList<ComplexNumber> FoundRoots => rootRegistry.Roots;

        public NewtonFractalRenderer(FractalSettings settings, Polynomial polynomial)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
            solver = new NewtonSolver(polynomial);
        }

        /// <summary>
        /// Computes the color of a pixel. Roots get their colors in the order in which they are found,
        /// so colors are consistent within one renderer instance.
        /// </summary>
        public Color GetPixelColor(int column, int row)
        {
            NewtonResult result = solver.FindRoot(settings.GetPointForPixel(column, row));
            if (!result.HasConverged)
            {
                return FractalColorizer.NotConvergedColor;
            }

            int rootIndex = rootRegistry.GetOrAddRootIndex(result.Root);

            return FractalColorizer.GetRootColor(rootIndex, result.Iterations);
        }

        public Bitmap Render()
        {
            var bitmap = new Bitmap(settings.Width, settings.Height);
            for (int row = 0; row < settings.Height; row++)
            {
                for (int column = 0; column < settings.Width; column++)
                {
                    bitmap.SetPixel(column, row, GetPixelColor(column, row));
                }
            }

            return bitmap;
        }
    }
}
