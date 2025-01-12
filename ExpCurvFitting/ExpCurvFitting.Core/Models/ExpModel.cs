using ExpCurvFitting.Core.RecognizingFunctions;
using ExpCurvFitting.Core.Optimization;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
namespace ExpCurvFitting.Core.Models;
public record ExpModel
{
    public OptimizationResult FittingResult { get; private set; }

    public async Task Fit(Vector<double> xLb, Vector<double> xUb, Vector<double> yLb, Vector<double> yUb, PenatlyOptions penatlyOptions)
    {
        var tol = new ExpTolWithPenatly(xLb, xUb, yLb, yUb, penatlyOptions);
        FittingResult = (await tol.MultistartOptimization(new RalgbSubgradientMinimizer(1e-12, 10000), 100, penatlyOptions.ALb.Count * 2)).ToOptimizationResult(penatlyOptions.ALb.Count);
        FittingResult.Rmse = CalcRmse(xLb, xUb, yLb, yUb);
    }

    public double CalcRmse(Vector<double> xLb, Vector<double> xUb, Vector<double> yLb, Vector<double> yUb)
    {
        var yPredict = Pridict(0.5 * (xLb + xUb));
        var yMid = 0.5 * (yLb + yUb);
        return Math.Pow((yPredict - yMid).DotProduct(yPredict - yMid)/ yMid.Count, 0.5);
    }

    public async Task FitForNonIntervalX(IEnumerable<double> x, IEnumerable<double> yMid, IEnumerable<double> yRad, PenatlyOptions penatlyOptions)
    {
        var xx = new DenseVector(x.ToArray());
        var yyMid = new DenseVector(yMid.ToArray());
        var yyRad = new DenseVector(yRad.ToArray());
        await Fit(xx, xx, yyMid - yyRad, yyMid + yyRad, penatlyOptions);
    }

    public double Pridict(double x)
    {
        return FittingResult.A.DotProduct((-x * FittingResult.B).PointwiseExp());
    }

    public Vector<double> Pridict(Vector<double> x)
    {
        var result = Vector.Build.Dense(x.Count);
        for (int i = 0; i < x.Count; i++)
        {
            result[i] = FittingResult.A.DotProduct((-x[i] * FittingResult.B).PointwiseExp());
        }
        return result;
    }

    public record Result
    {
        public double TolValue { get; init; }
        public double RmseForCenter { get; init; }
        public double TimeCalculation { get; init; }
        public double[] A { get; init; }
        public double[] B { get; init; }

        public IEnumerable<(double, double)> GetPoints()
        {
            for (int i = 0; i < A.Length; i++)
            {
                yield return new(A[i], B[i]);
            }
        }
    }

    public Result GetResult()
    {
        return new Result
        {
            TolValue = FittingResult.TolValue,
            A = FittingResult.A.AsArray(),
            B = FittingResult.B.AsArray(),
            RmseForCenter = FittingResult.Rmse,
            TimeCalculation = FittingResult.TimeCalculation.TotalSeconds
        };
    }
}
