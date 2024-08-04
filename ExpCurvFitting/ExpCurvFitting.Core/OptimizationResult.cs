using MathNet.Numerics.LinearAlgebra;
namespace ExpCurvFitting.Core;

public record OptimizationResult
{
    public double TolValue { get; init; }
    public Vector<double> A { get; init; }
    public Vector<double> B { get; init; }
    public double GradL2Norm { get; init; }
}
