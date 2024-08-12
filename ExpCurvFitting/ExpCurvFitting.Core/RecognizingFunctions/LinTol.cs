using MathNet.Numerics.LinearAlgebra;
namespace ExpCurvFitting.Core.RecognizingFunctions;
public record LinTol
{
    public Matrix<double> XLowerBound { get; init; }
    public Matrix<double> XUpperBound { get; init; }
    public Vector<double> YLowerBound { get; init; }
    public Vector<double> YUpperBound { get; init; }

    public Matrix<double> XMid => 0.5 * (XLowerBound + XUpperBound);
    public Matrix<double> XRad => 0.5 * (XUpperBound - XLowerBound);
    public Vector<double> YMid => 0.5 * (YLowerBound + YUpperBound);
    public Vector<double> YRad => 0.5 * (YUpperBound - YLowerBound);

    public double TolValue(Vector<double> t)
    {
        return CalcGeneratrix(t).Min();
    }

    public Vector<double> Grad(Vector<double> t)
    {
        var result = Vector<double>.Build.Dense(t.Count);
        return result;
    }

    private Vector<double> CalcGeneratrix(Vector<double> t)
    {
        var result = Vector<double>.Build.Dense(YRad.Count);
        return result;
    }
}
