using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NNPTPZ1.NewtonFractal.Tests
{
    [TestClass]
    public class FractalColorizerTests
    {
        [TestMethod]
        public void GetRootColor_WithoutIterations_ReturnsPaletteColor()
        {
            Assert.AreEqual(Color.FromArgb(255, 0, 0).ToArgb(), FractalColorizer.GetRootColor(0, 0).ToArgb());
        }

        [TestMethod]
        public void GetRootColor_DarkensByIterations()
        {
            Assert.AreEqual(Color.FromArgb(195, 0, 0).ToArgb(), FractalColorizer.GetRootColor(0, 30).ToArgb());
        }

        [TestMethod]
        public void GetRootColor_ManyIterations_ReturnsBlack()
        {
            Assert.AreEqual(Color.Black.ToArgb(), FractalColorizer.GetRootColor(1, 500).ToArgb());
        }

        [TestMethod]
        public void GetRootColor_WrapsAroundPalette()
        {
            Assert.AreEqual(FractalColorizer.GetRootColor(0, 0).ToArgb(), FractalColorizer.GetRootColor(9, 0).ToArgb());
        }
    }
}
