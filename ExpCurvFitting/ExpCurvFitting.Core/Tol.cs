using MathNet.Numerics.LinearAlgebra;

namespace ExpCurvFitting.Core;
public record Tol
{
    public Vector<double> XLowerBound { get; set; }
    public Vector<double> XUpperBound { get; set; }
    public Vector<double> YLowerBound { get; set; }
    public Vector<double> YUpperBound { get; set; }

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
        return 0;
    }

    public Vector<double> GradA(Vector<double> a, Vector<double> b)
    {
        return null;
    }
    public Vector<double> GradB(Vector<double> a, Vector<double> b)
    {
        return null;
    }
    public Vector<double> Grad(Vector<double> x0)
    {
        return null;
    }
    public double TolValue(Vector<double> x0)
    {
        return 0;
    }
}
