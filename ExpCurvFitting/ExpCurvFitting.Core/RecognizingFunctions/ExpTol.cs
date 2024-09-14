using ExpCurvFitting.Core.Models;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace ExpCurvFitting.Core.RecognizingFunctions;
public record ExpTol
{
    public Vector<double> XLowerBound { get; init; }
    public Vector<double> XUpperBound { get; init; }
    public Vector<double> YLowerBound { get; init; }
    public Vector<double> YUpperBound { get; init; }

    public Vector<double> XMid => 0.5 * (XLowerBound + XUpperBound);
    public Vector<double> XRad => 0.5 * (XUpperBound - XLowerBound);
    public Vector<double> YMid => 0.5 * (YLowerBound + YUpperBound);
    public Vector<double> YRad => 0.5 * (YUpperBound - YLowerBound);
    public ExpTol(Vector<double> xLowerBound, Vector<double> xUpperBound, Vector<double> yLowerBound, Vector<double> yUpperBound)
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
    public virtual double TolValue(Vector<double> x0)
    {
        return TolValue(x0.SubVector(0, x0.Count / 2), x0.SubVector(x0.Count / 2, x0.Count / 2));
    }
    public virtual Vector<double> Grad(Vector<double> x0)
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

    public BaseOptimizationResult Optimization(IUnconstrainedMinimizer minimizer, Vector<double> x0)
    {
        Func<Vector<double>, (double, Vector<double>)> functional = (x) => new(-TolValue(x), -Grad(x));
        var objective = ObjectiveFunction.Gradient(functional);
        var result = minimizer.FindMinimum(objective, x0);
        return new BaseOptimizationResult
        {
            TolValue = -result.FunctionInfoAtMinimum.Value,
            MinimizingPoint = result.MinimizingPoint,
            GradL2Norm = result.FunctionInfoAtMinimum.Gradient.L2Norm(),
            MinYRad = YRad.Min()
        };
    }

    public async Task<BaseOptimizationResult> MultistartOptimization(IUnconstrainedMinimizer minimizer, int countStarts, int countVariables)
    {
        var concurrentBag = new ConcurrentBag<BaseOptimizationResult>();
        var tasks = new List<Task>();
        var random = new Random();
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        for (int i = 0; i < countStarts; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                try
                {
                    var initPoints = Vector<double>.Build.DenseOfEnumerable(Enumerable.Range(0, countVariables).Select(i => random.NextDouble()));
                    vector.Select(i => random.NextDouble());
                    var initPoints = Vector<double>.Build.DenseOfEnumerable(vector.Select(i => random.NextDouble()));
                    var currentResult = Optimization(minimizer, initPoints);
                    concurrentBag.Add(currentResult);
                }
                catch (ArithmeticException ex)
                {
                    // 
                }
            }));
        }
        await Task.WhenAll(tasks);
        stopwatch.Stop();
        var result = concurrentBag.OrderByDescending(r => r.TolValue).First();
        return result with
        {
            TimeCalculation = stopwatch.Elapsed,
        }; 
    }
    #endregion
}
