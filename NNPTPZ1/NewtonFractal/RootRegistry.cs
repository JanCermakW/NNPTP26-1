using System.Collections.Generic;
using NNPTPZ1.Mathematics;

namespace NNPTPZ1.NewtonFractal
{
    /// <summary>
    /// Keeps distinct roots found so far and assigns each of them a stable index.
    /// </summary>
    public sealed class RootRegistry
    {
        /// <summary>Squared distance under which two roots are considered the same.</summary>
        private const double SameRootSquaredDistance = 0.01;

        private readonly List<ComplexNumber> roots = new List<ComplexNumber>();

        public IReadOnlyList<ComplexNumber> Roots { get; }

        public RootRegistry()
        {
            Roots = roots.AsReadOnly();
        }

        /// <summary>
        /// Returns the index of an already known root close to <paramref name="root"/>,
        /// or registers it as a new root and returns its index.
        /// All undefined (NaN) roots share a single index.
        /// </summary>
        public int GetOrAddRootIndex(ComplexNumber root)
        {
            for (int index = 0; index < roots.Count; index++)
            {
                if (IsSameRoot(roots[index], root))
                {
                    return index;
                }
            }

            roots.Add(root);
            return roots.Count - 1;
        }

        private static bool IsSameRoot(ComplexNumber knownRoot, ComplexNumber root)
        {
            if (knownRoot.IsNaN || root.IsNaN)
            {
                return knownRoot.IsNaN && root.IsNaN;
            }

            return knownRoot.GetSquaredDistanceTo(root) <= SameRootSquaredDistance;
        }
    }
}
