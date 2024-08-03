using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;

namespace ExpCurvFitting.Core;
public record Tol
{
    public Vector<double> XLowerBound { get; init; }
    public Vector<double> XUpperBound { get; init; }
    public Vector<double> YLowerBound { get; init; }
    public Vector<double> YUpperBound { get; init; }

    public Vector<double> XMid => 0.5 * (XLowerBound + XUpperBound);
    public Vector<double> XRad => 0.5 * (XUpperBound - XLowerBound);
    public Vector<double> YMid => 0.5 * (YLowerBound + YUpperBound);
    public Vector<double> YRad => 0.5 * (YUpperBound - YLowerBound);
    public Tol(Vector<double> xLowerBound, Vector<double> xUpperBound, Vector<double> yLowerBound, Vector<double> yUpperBound)
    {
        XLowerBound = xLowerBound;
        XUpperBound = xUpperBound;
        YLowerBound = yLowerBound;
        YUpperBound = yUpperBound;
    }

    public double TolValue(Vector<double> a, Vector<double> b)
    {
        return CalcGeneratrix(a, b).Min();
    }

    private Vector<double> CalcGeneratrix(Vector<double> a, Vector<double> b)
    {
        var result = Vector<double>.Build.Dense(YRad.Count);
        for(int i = 0; i < YRad.Count; i++)
        {
            var eLb = (-XLowerBound[i] * b).PointwiseExp().DotProduct(a);
            var eUb = (-XUpperBound[i] * b).PointwiseExp().DotProduct(a);
            result[i] = YRad[i] - 0.5 * (eLb - eUb)
            -  Math.Abs(YMid[i] - 0.5 * (eLb + eUb));
        }
        return result;
    }

    public Vector<double> GradA(Vector<double> a, Vector<double> b)
    {
        var indexMin = CalcGeneratrix(a, b).MinimumIndex();
        var eLb = (-XLowerBound[indexMin] * b).PointwiseExp();
        var eUb = (-XUpperBound[indexMin] * b).PointwiseExp();
        var grad = - 0.5 * (eLb - eUb) 
                   - 0.5 * (eLb + eUb) 
                   * Math.Sign(0.5 * (eLb + eUb).DotProduct(a) - YMid[indexMin]);
        return grad;
    }
    public Vector<double> GradB(Vector<double> a, Vector<double> b)
    {
        var indexMin = CalcGeneratrix(a, b).MinimumIndex();
        var eLb = (-XLowerBound[indexMin] * b).PointwiseExp();
        var eUb = (-XUpperBound[indexMin] * b).PointwiseExp();
        var grad = 0.5 * (XLowerBound[indexMin] * eLb - XUpperBound[indexMin] * eUb).PointwiseMultiply(a)
                 + 0.5 * (XLowerBound[indexMin] * eLb + XUpperBound[indexMin] * eUb).PointwiseMultiply(a) 
                 * Math.Sign(0.5 * (eLb + eUb).DotProduct(a) - YMid[indexMin]);
        return grad;
    }
    public Vector<double> Grad(Vector<double> x0)
    {
        return null;
    }
    public double TolValue(Vector<double> x0)
    {
        return 0;
    }

    public void Optimization(IUnconstrainedMinimizer minimizer, Vector<double> x0)
    {

    }
}
