using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1.NewtonFractal.Tests
{
    [TestClass]
    public class RootRegistryTests
    {
        [TestMethod]
        public void GetOrAddRootIndex_NewRoots_GetIndexesFromZero()
        {
            var registry = new RootRegistry();

            Assert.AreEqual(0, registry.GetOrAddRootIndex(new ComplexNumber(-1, 0)));
            Assert.AreEqual(1, registry.GetOrAddRootIndex(new ComplexNumber(0.5, 0.87)));
            Assert.AreEqual(2, registry.GetOrAddRootIndex(new ComplexNumber(0.5, -0.87)));
            Assert.AreEqual(3, registry.Roots.Count);
        }

        [TestMethod]
        public void GetOrAddRootIndex_CloseRoot_ReturnsKnownIndex()
        {
            var registry = new RootRegistry();
            registry.GetOrAddRootIndex(new ComplexNumber(-1, 0));

            Assert.AreEqual(0, registry.GetOrAddRootIndex(new ComplexNumber(-1.05, 0.05)));
            Assert.AreEqual(1, registry.Roots.Count);
        }

        [TestMethod]
        public void Roots_AreReadOnly()
        {
            var registry = new RootRegistry();
            registry.GetOrAddRootIndex(new ComplexNumber(-1, 0));

            Assert.IsTrue(((ICollection<ComplexNumber>)registry.Roots).IsReadOnly);
            Assert.AreEqual(new ComplexNumber(-1, 0), registry.Roots[0]);
        }

        [TestMethod]
        public void GetOrAddRootIndex_UndefinedRoots_ShareOneIndex()
        {
            var registry = new RootRegistry();
            var undefined = new ComplexNumber(double.NaN, double.NaN);
            registry.GetOrAddRootIndex(new ComplexNumber(-1, 0));

            Assert.AreEqual(1, registry.GetOrAddRootIndex(undefined));
            Assert.AreEqual(1, registry.GetOrAddRootIndex(new ComplexNumber(double.NaN, 0)));
            Assert.AreEqual(0, registry.GetOrAddRootIndex(new ComplexNumber(-1, 0)));
            Assert.AreEqual(2, registry.Roots.Count);
        }
    }
}
