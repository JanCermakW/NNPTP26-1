using NNPTPZ1.Mathematics;

namespace NNPTPZ1.NewtonFractal
{
    /// <summary>
    /// Root found by <see cref="NewtonSolver"/> together with the number of iterations it took.
    /// </summary>
    public sealed class NewtonResult
    {
        /// <summary>Found root, or the last approximation when the iteration has not converged.</summary>
        public ComplexNumber Root { get; }

        public int Iterations { get; }

        /// <summary>False when the iteration was stopped by the iteration limit.</summary>
        public bool HasConverged { get; }

        public NewtonResult(ComplexNumber root, int iterations, bool hasConverged)
        {
            Root = root;
            Iterations = iterations;
            HasConverged = hasConverged;
        }
    }
}
