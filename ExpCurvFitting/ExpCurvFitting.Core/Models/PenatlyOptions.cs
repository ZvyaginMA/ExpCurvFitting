using MathNet.Numerics.LinearAlgebra;
namespace ExpCurvFitting.Core.Models;
public record PenatlyOptions
{
    public Vector<double> ALb { get; init; }
    public Vector<double> AUb { get; init; }
    public Vector<double> BLb { get; init; }
    public Vector<double> BUb { get; init; }
    public double CostA { get; init; } = 100;
    public double CostB { get; init; } = 100;
}
