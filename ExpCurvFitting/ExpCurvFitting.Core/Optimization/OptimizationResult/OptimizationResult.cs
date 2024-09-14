using MathNet.Numerics.LinearAlgebra;
namespace ExpCurvFitting.Core.Models;

public record OptimizationResult : BaseOptimizationResult
{
    public Vector<double> A { get; init; } 
    public Vector<double> B { get; init; }
}
