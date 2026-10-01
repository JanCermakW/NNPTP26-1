using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1.NewtonFractal.Tests
{
    [TestClass]
    public class NewtonFractalRendererTests
    {
        // x^3 + 1
        private static readonly Polynomial Cubic = new Polynomial(
            new ComplexNumber(1, 0), ComplexNumber.Zero, ComplexNumber.Zero, new ComplexNumber(1, 0));

        [TestMethod]
        public void GetPixelColor_PixelsNearDifferentRoots_HaveDifferentColors()
        {
            var settings = new FractalSettings(3, 3, -3, 3, -3, 3, null);
            var renderer = new NewtonFractalRenderer(settings, Cubic);

            // points (-3, -1), (1, 1) and (1, -3) converge to roots -1, 1/2 + 0.87i and 1/2 - 0.87i
            Color realRootColor = renderer.GetPixelColor(0, 1);
            Color positiveImaginaryRootColor = renderer.GetPixelColor(2, 2);
            Color negativeImaginaryRootColor = renderer.GetPixelColor(2, 0);

            Assert.AreEqual(3, renderer.FoundRoots.Count);
            Assert.AreNotEqual(realRootColor.ToArgb(), positiveImaginaryRootColor.ToArgb());
            Assert.AreNotEqual(realRootColor.ToArgb(), negativeImaginaryRootColor.ToArgb());
            Assert.AreNotEqual(positiveImaginaryRootColor.ToArgb(), negativeImaginaryRootColor.ToArgb());
        }

        [TestMethod]
        public void GetPixelColor_SamePixel_ReturnsSameColor()
        {
            var renderer = new NewtonFractalRenderer(new FractalSettings(3, 3, -3, 3, -3, 3, null), Cubic);

            Assert.AreEqual(renderer.GetPixelColor(1, 1).ToArgb(), renderer.GetPixelColor(1, 1).ToArgb());
            Assert.AreEqual(1, renderer.FoundRoots.Count);
        }

        [TestMethod]
        public void GetPixelColor_NotConvergingPoint_IsNotRegisteredAsRoot()
        {
            // x^3 - 2x + 2, Newton's iteration started near 0 never converges
            var cyclingCubic = new Polynomial(
                new ComplexNumber(2, 0), new ComplexNumber(-2, 0), ComplexNumber.Zero, new ComplexNumber(1, 0));
            var renderer = new NewtonFractalRenderer(new FractalSettings(1, 1, 0, 1, 0, 1, null), cyclingCubic);

            Color color = renderer.GetPixelColor(0, 0);

            Assert.AreEqual(FractalColorizer.NotConvergedColor.ToArgb(), color.ToArgb());
            Assert.AreEqual(0, renderer.FoundRoots.Count);
        }

        [TestMethod]
        public void Render_NonSquareImage_ColorsEveryPixel()
        {
            var renderer = new NewtonFractalRenderer(new FractalSettings(4, 2, -3, 3, -3, 3, null), Cubic);

            using (Bitmap bitmap = renderer.Render())
            {
                Assert.AreEqual(4, bitmap.Width);
                Assert.AreEqual(2, bitmap.Height);
                for (int row = 0; row < bitmap.Height; row++)
                {
                    for (int column = 0; column < bitmap.Width; column++)
                    {
                        Color expected = renderer.GetPixelColor(column, row);
                        Assert.AreEqual(expected.ToArgb(), bitmap.GetPixel(column, row).ToArgb());
                    }
                }
            }
        }

        [TestMethod]
        public void Constructor_NullArguments_Throw()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new NewtonFractalRenderer(null, Cubic));
            Assert.ThrowsException<ArgumentNullException>(
                () => new NewtonFractalRenderer(new FractalSettings(1, 1, 0, 1, 0, 1, null), null));
        }
    }
}
