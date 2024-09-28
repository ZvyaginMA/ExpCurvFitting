using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;

namespace ExpCurvFitting.Core.Optimization;

public interface IMinimizer
{
    MinimizationResult FindMinimum(IObjectiveFunction objective, 
        Vector<double> initialGuess,
        CancellationToken cancellationToken);
}