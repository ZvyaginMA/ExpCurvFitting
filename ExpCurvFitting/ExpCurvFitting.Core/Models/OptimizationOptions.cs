namespace ExpCurvFitting.Core.Models;

public class OptimizationOptions
{
    public int CountMultistarts = 100;
    public int MaximumIterations = 10000;
    public double GradientTolerance = 1e-9;
}