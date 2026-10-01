using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NNPTPZ1.NewtonFractal.Tests
{
    [TestClass]
    public class FractalSettingsParserTests
    {
        [TestMethod]
        public void Parse_ValidArguments_ReturnsSettings()
        {
            FractalSettings settings = FractalSettingsParser.Parse(
                new[] { "300", "200", "-1.5", "1.5", "-1", "1", "fractal.png" });

            Assert.AreEqual(300, settings.Width);
            Assert.AreEqual(200, settings.Height);
            Assert.AreEqual(-1.5, settings.XMin);
            Assert.AreEqual(1.5, settings.XMax);
            Assert.AreEqual(-1, settings.YMin);
            Assert.AreEqual(1, settings.YMax);
            Assert.AreEqual("fractal.png", settings.OutputPath);
        }

        [TestMethod]
        public void Parse_WithoutOutputPath_UsesDefault()
        {
            FractalSettings settings = FractalSettingsParser.Parse(new[] { "10", "10", "-1", "1", "-1", "1" });

            Assert.AreEqual(FractalSettings.DefaultOutputPath, settings.OutputPath);
        }

        [TestMethod]
        public void Parse_TooFewArguments_Throws()
        {
            Assert.ThrowsException<ArgumentException>(() => FractalSettingsParser.Parse(new[] { "10", "10" }));
        }

        [TestMethod]
        public void Parse_TooManyArguments_Throws()
        {
            Assert.ThrowsException<ArgumentException>(
                () => FractalSettingsParser.Parse(new[] { "10", "10", "-1", "1", "-1", "1", "out.png", "extra" }));
        }

        [TestMethod]
        public void Parse_NullArguments_Throws()
        {
            Assert.ThrowsException<ArgumentException>(() => FractalSettingsParser.Parse(null));
        }

        [DataTestMethod]
        [DataRow("abc", "10", "-1", "1", "-1", "1")]
        [DataRow("10", "1.5", "-1", "1", "-1", "1")]
        [DataRow("10", "10", "x", "1", "-1", "1")]
        [DataRow("10", "10", "-1,5", "1", "-1", "1")]
        [DataRow("10", "10", "NaN", "1", "-1", "1")]
        [DataRow("10", "10", "-1", "Infinity", "-1", "1")]
        public void Parse_InvalidNumber_Throws(
            string width, string height, string xMin, string xMax, string yMin, string yMax)
        {
            Assert.ThrowsException<ArgumentException>(
                () => FractalSettingsParser.Parse(new[] { width, height, xMin, xMax, yMin, yMax }));
        }
    }
}
