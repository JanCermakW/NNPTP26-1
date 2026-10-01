using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1.NewtonFractal.Tests
{
    [TestClass]
    public class FractalSettingsTests
    {
        [TestMethod]
        public void Steps_AreAreaSizeDividedByImageSize()
        {
            var settings = new FractalSettings(300, 200, -1.5, 1.5, -1, 1, "fractal.png");

            Assert.AreEqual(0.01, settings.XStep, 1e-12);
            Assert.AreEqual(0.01, settings.YStep, 1e-12);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        public void Constructor_MissingOutputPath_UsesDefault(string outputPath)
        {
            var settings = new FractalSettings(10, 10, -1, 1, -1, 1, outputPath);

            Assert.AreEqual(FractalSettings.DefaultOutputPath, settings.OutputPath);
        }

        [DataTestMethod]
        [DataRow(0, 10, -1, 1, -1, 1)]
        [DataRow(10, -5, -1, 1, -1, 1)]
        [DataRow(10, 10, 1, -1, -1, 1)]
        [DataRow(10, 10, -1, 1, 1, 1)]
        [DataRow(10, 10, double.NaN, 1, -1, 1)]
        [DataRow(10, 10, -1, double.PositiveInfinity, -1, 1)]
        [DataRow(10, 10, -1, 1, double.NegativeInfinity, 1)]
        [DataRow(10, 10, -1, 1, -1, double.NaN)]
        public void Constructor_InvalidValues_Throws(
            int width, int height, double xMin, double xMax, double yMin, double yMax)
        {
            Assert.ThrowsException<ArgumentException>(
                () => new FractalSettings(width, height, xMin, xMax, yMin, yMax, null));
        }

        [TestMethod]
        public void GetPointForPixel_MapsColumnToRealAndRowToImaginaryPart()
        {
            var settings = new FractalSettings(4, 2, -2, 2, -1, 1, null);

            Assert.AreEqual(new ComplexNumber(-2, -1), settings.GetPointForPixel(0, 0));
            Assert.AreEqual(new ComplexNumber(1, 0), settings.GetPointForPixel(3, 1));
        }
    }
}
