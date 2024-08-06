using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
namespace ExpCurvFitting.Core;
public record ExpModel
{
    public ExpModel()
    {
    }

    public async Task<Result> Fit(Vector<double> xLb, Vector<double> xUb, Vector<double> yLb, Vector<double> yUb, int numberOfExp)
    {
        var tol = new Tol(xLb, xUb, yLb, yUb);
        var result = await tol.MultistartOptimization2(new RalgbSubgradientMinimizer(1e-12, 10000),100, numberOfExp);
        return await Task.FromResult(new Result
        {
            TolValue = result.TolValue,
            A = result.A.AsArray(),
            B = result.B.AsArray(),
            Rmse = result.Rmse,
        });
    }

    public async Task<Result> Fit(IEnumerable<double> x, IEnumerable<double> yMid, IEnumerable<double> yRad, int numberOfExp)
    {
        var xx = new DenseVector(x.ToArray());
        var yyMid = new DenseVector(x.ToArray());
        var yyRad = new DenseVector(x.ToArray());
        return await Fit(xx, xx, yyMid - yyMid, yyMid + yyMid, numberOfExp);
    }

    public void Pridict()
    {
        
    }

    public record Result
    {
        public double TolValue { get; init; }
        public double Rmse { get; init; }
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
}
