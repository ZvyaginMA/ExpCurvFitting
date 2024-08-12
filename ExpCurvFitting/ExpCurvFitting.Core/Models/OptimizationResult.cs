using MathNet.Numerics.LinearAlgebra;
namespace ExpCurvFitting.Core.Models;

public record OptimizationResult
{
    public double TolValue { get; init; }
    public Vector<double> A { get; init; }
    public Vector<double> B { get; init; }
    public double GradL2Norm { get; init; }
    public double Rmse { get; set; }
    public TimeSpan TimeCalculation { get; set; }
}
