using ExpCurvFitting.Core.Models;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
namespace ExpCurvFitting.Core.RecognizingFunctions;

public interface ITol
{
    Task<OptimizationResult> MultistartOptimization(IUnconstrainedMinimizer minimizer, int countStarts, int countExp);
    OptimizationResult Optimization(IUnconstrainedMinimizer minimizer, Vector<double> x0);
}
