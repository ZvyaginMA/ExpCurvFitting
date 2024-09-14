using MathNet.Numerics.LinearAlgebra;

namespace ExpCurvFitting.Core.Models;

public record OptimizationWithMixinResult : BaseOptimizationResult
{
    public Vector<double> A { get; init; }
    public Vector<double> B { get; init; }
    public Vector<double> C { get; init; }
}