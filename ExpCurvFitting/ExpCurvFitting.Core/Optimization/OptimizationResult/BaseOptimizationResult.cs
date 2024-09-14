using MathNet.Numerics.LinearAlgebra;

namespace ExpCurvFitting.Core.Models;

public record BaseOptimizationResult
{
    public double TolValue { get; init; }
    public Vector<double> MinimizingPoint { get; init; }
    public double GradL2Norm { get; init; }
    public double Rmse { get; set; }
    public TimeSpan TimeCalculation { get; set; }
    public double MinYRad { get; init; }

    public OptimizationWithMixinResult ToOptimizationWithMixinResult(int countExp, int countMixins)
    {
        if (countExp * 2 + countMixins != MinimizingPoint.Count)
        {
            throw new ArgumentException("неправильные параметры countExp, countMixin");
        }
        var a = MinimizingPoint.SubVector(0, countExp);
        var b = MinimizingPoint.SubVector(countExp, countExp);
        var c = MinimizingPoint.SubVector(countExp * 2, countMixins);
        return new()
        {
            TolValue = TolValue,
            Rmse = Rmse,
            TimeCalculation = TimeCalculation,
            GradL2Norm = GradL2Norm,
            MinYRad = MinYRad,
            A = a,
            B = b,
            C = c,
        };
    }
    
    public OptimizationResult ToOptimizationResult(int countExp)
    {
        if (countExp * 2 != MinimizingPoint.Count)
        {
            throw new ArgumentException("неправильные параметры countExp");
        }
        var a = MinimizingPoint.SubVector(0, countExp);
        var b = MinimizingPoint.SubVector(countExp, countExp);
        return new()
        {
            TolValue = TolValue,
            Rmse = Rmse,
            TimeCalculation = TimeCalculation,
            GradL2Norm = GradL2Norm,
            MinYRad = MinYRad,
            A = a,
            B = b,
        };
    }
}