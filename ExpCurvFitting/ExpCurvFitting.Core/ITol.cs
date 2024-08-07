using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;

namespace ExpCurvFitting.Core
{
    public interface ITol
    {
        Task<OptimizationResult> MultistartOptimization(IUnconstrainedMinimizer minimizer, int countStarts, int countExp);
        OptimizationResult Optimization(IUnconstrainedMinimizer minimizer, Vector<double> x0);
    }
}