﻿using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using System.Collections.Concurrent;

namespace ExpCurvFitting.Core;
public record Tol : ITol
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

    public virtual double TolValue(Vector<double> a, Vector<double> b)
    {
        return CalcGeneratrix(a, b).Min();
    }


    private Vector<double> CalcGeneratrix(Vector<double> a, Vector<double> b)
    {
        var result = Vector<double>.Build.Dense(YRad.Count);
        for (int i = 0; i < YRad.Count; i++)
        {
            var eLb = (-XLowerBound[i] * b).PointwiseExp().DotProduct(a);
            var eUb = (-XUpperBound[i] * b).PointwiseExp().DotProduct(a);
            result[i] = YRad[i] - 0.5 * (eLb - eUb)
            - Math.Abs(YMid[i] - 0.5 * (eLb + eUb));
        }
        return result;
    }

    public virtual Vector<double> GradA(Vector<double> a, Vector<double> b)
    {
        var indexMin = CalcGeneratrix(a, b).MinimumIndex();
        var eLb = (-XLowerBound[indexMin] * b).PointwiseExp();
        var eUb = (-XUpperBound[indexMin] * b).PointwiseExp();
        var grad = -0.5 * (eLb - eUb)
                   - 0.5 * (eLb + eUb)
                   * Math.Sign(0.5 * (eLb + eUb).DotProduct(a) - YMid[indexMin]);
        return grad;
    }
    public virtual Vector<double> GradB(Vector<double> a, Vector<double> b)
    {
        var indexMin = CalcGeneratrix(a, b).MinimumIndex();
        var eLb = (-XLowerBound[indexMin] * b).PointwiseExp();
        var eUb = (-XUpperBound[indexMin] * b).PointwiseExp();
        var grad = 0.5 * (XLowerBound[indexMin] * eLb - XUpperBound[indexMin] * eUb).PointwiseMultiply(a)
                 + 0.5 * (XLowerBound[indexMin] * eLb + XUpperBound[indexMin] * eUb).PointwiseMultiply(a)
                 * Math.Sign(0.5 * (eLb + eUb).DotProduct(a) - YMid[indexMin]);
        return grad;
    }
    public double TolValue(Vector<double> x0)
    {
        return TolValue(x0.SubVector(0, x0.Count / 2), x0.SubVector(x0.Count / 2, x0.Count / 2));
    }
    public Vector<double> Grad(Vector<double> x0)
    {
        var result = Vector<double>.Build.Dense(x0.Count);
        result.SetSubVector(0, x0.Count / 2, GradA(x0));
        result.SetSubVector(x0.Count / 2, x0.Count / 2, GradB(x0));
        return result;
    }
    private Vector<double> GradA(Vector<double> x0)
    {
        return GradA(x0.SubVector(0, x0.Count / 2), x0.SubVector(x0.Count / 2, x0.Count / 2));
    }
    private Vector<double> GradB(Vector<double> x0)
    {
        return GradB(x0.SubVector(0, x0.Count / 2), x0.SubVector(x0.Count / 2, x0.Count / 2));
    }

    #region Optimization

    public OptimizationResult Optimization(IUnconstrainedMinimizer minimizer, Vector<double> x0)
    {
        Func<Vector<double>, (double, Vector<double>)> functional = (x) => new(-TolValue(x), -Grad(x));
        var objective = ObjectiveFunction.Gradient(functional);
        var result = minimizer.FindMinimum(objective, x0);
        return new OptimizationResult
        {
            TolValue = -result.FunctionInfoAtMinimum.Value,
            A = result.MinimizingPoint.SubVector(0, x0.Count / 2),
            B = result.MinimizingPoint.SubVector(x0.Count / 2, x0.Count / 2),
            GradL2Norm = result.FunctionInfoAtMinimum.Gradient.L2Norm()
        };
    }

    public OptimizationResult Optimization(IUnconstrainedMinimizer minimizer, Vector<double> A, Vector<double> B)
    {
        var x0 = Vector<double>.Build.Dense(A.Count * 2);
        x0.SetSubVector(0, A.Count, A);
        x0.SetSubVector(A.Count, A.Count, B);
        return Optimization(minimizer, x0);
    }

    public async Task<OptimizationResult> MultistartOptimization(IUnconstrainedMinimizer minimizer, int countStarts, int countExp)
    {
        var concurrentBag = new ConcurrentBag<OptimizationResult>();
        var tasks = new List<Task>();
        for (int i = 0; i < countStarts; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                var initPoints = Vector<double>.Build.Random(countExp * 2);
                var currentResult = Optimization(minimizer, initPoints);
                concurrentBag.Add(currentResult);
            }));
        }
        await Task.WhenAll(tasks);

        var result = concurrentBag.OrderByDescending(r => r.TolValue).First();
        return result;
    }
    #endregion
}
