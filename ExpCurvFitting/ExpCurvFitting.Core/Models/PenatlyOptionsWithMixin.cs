using MathNet.Numerics.LinearAlgebra;

namespace ExpCurvFitting.Core.Models;

public class PenatlyOptionsWithMixin
{
    public Vector<double> ALb { get; init; }
    public Vector<double> AUb { get; init; }
    public Vector<double> BLb { get; init; }
    public Vector<double> BUb { get; init; }
    public Vector<double> CLb { get; init; }
    public Vector<double> CUb { get; init; }
    public double CostA { get; init; } = 100;
    public double CostB { get; init; } = 100;
    public double CostC { get; init; } = 100;
}