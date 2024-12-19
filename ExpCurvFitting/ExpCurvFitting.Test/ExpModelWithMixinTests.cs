using System.Diagnostics;
using ExpCurvFitting.Core.FunctionalExtensions;
using ExpCurvFitting.Core.Models;
using ExpCurvFitting.Core.RecognizingFunctions;
using FluentAssertions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ExpCurvFitting.Test;

public class ExpModelWithMixinTests
{
    [Fact]
    public async Task CurveFit_ForExpModel_WithPowerMixin_Success()
    {
        var xMid = new DenseVector([0.12, 0.2, 0.6, 0.7, 0.9, 1.4, 1.6, 1.9, 2.4]);
        var yLb = new DenseVector([2.564, 1.0253, 0.2068, 0.1517, 0.0753, -0.017, -0.0343, -0.0501, -0.0623]);
        var yUb = new DenseVector([2.704, 1.1653, 0.3468, 0.2917, 0.2153, 0.123, 0.1057, 0.0899, 0.0777]);

        var mixins = new List<IIntervalExtensions>()
        {
            new MonotonicFunction((t) => Math.Pow(t, -2.5)),
        };

        var model = new ExpWithMixinModel(mixins);

        var aLb = new DenseVector([0.0001]);
        var aUb = new DenseVector([4.0]);
        var bLb = new DenseVector([0.0001]);
        var bUb = new DenseVector([4.0]);
        var cLb = new DenseVector([0.0001]);
        var cUb = new DenseVector([4.0]);

        var penatlyOptions = new PenatlyOptionsWithMixin()
        {
            ALb = aLb,
            AUb = aUb,
            BLb = bLb,
            BUb = bUb,
            CLb = cLb,
            CUb = cUb,
            CostA = 2,
            CostB = 3,
            CostC = 4,
        };

        await model.Fit(
            xMid,
            xMid,
            yLb,
            yUb,
            penatlyOptions,
            new OptimizationOptions());

        model.FittingResult.Should().NotBeNull();
        model.FittingResult.A[0].Should().BeInRange(0.799, 0.8);
        model.FittingResult.B[0].Should().BeInRange(1.999, 2.0);
        model.FittingResult.C[0].Should().BeInRange(0.01, 0.01001);
    }

    [Fact]
    public async Task CurveFit_ForQubicModel_Success()
    {
        var points = new[] { -2.0, -1.5, -1.0, 0.5, 0.0, 1.0, 2.7, 2.8, 3.5, 5.0, 6.0, 7.1 };
        var coeffs = new[] { 1, 1.1, -2.5, -10.0001 };
        Func<double, double> f = (x) => coeffs[0] + coeffs[1] * x + coeffs[2] * x * x + coeffs[3] * x * x * x;
        var y = points.Select(t => f(t)).ToArray();

        var xMid = new DenseVector(points);
        var yLb = new DenseVector(y) - 0.1;
        var yUb = new DenseVector(y) + 0.1;

        var mixins = new List<IIntervalExtensions>()
        {
            new MonotonicFunction((t) => 1),
            new MonotonicFunction((t) => t),
            new EvenFunction((t) => t * t),
            new MonotonicFunction((t) => t * t * t),
        };

        var model = new ExpWithMixinModel(mixins);

        var aLb = new DenseVector([0.0000]);
        var aUb = new DenseVector([0.0000]);
        var bLb = new DenseVector([0.0000]);
        var bUb = new DenseVector([0.0000]);
        var cLb = new DenseVector([-15.0, -15.0, -15.0, -15.0]);
        var cUb = new DenseVector([15.0, 15.0, 15.0, 15.0]);

        var penatlyOptions = new PenatlyOptionsWithMixin()
        {
            ALb = aLb,
            AUb = aUb,
            BLb = bLb,
            BUb = bUb,
            CLb = cLb,
            CUb = cUb,
            CostA = 2,
            CostB = 3,
            CostC = 4,
        };

        await model.Fit(
            xMid,
            xMid,
            yLb,
            yUb,
            penatlyOptions,
            new OptimizationOptions());

        model.FittingResult.Should().NotBeNull();
        model.FittingResult.A[0].Should().BeInRange(-0.0001, 0.0001);
        model.FittingResult.B[0].Should().BeInRange(-0.0001, 0.0001);
        model.FittingResult.C[0].Should().BeInRange(coeffs[0] - 1e-5, coeffs[0] + 1e-5);
        model.FittingResult.C[1].Should().BeInRange(coeffs[1] - 1e-5, coeffs[1] + 1e-5);
        model.FittingResult.C[2].Should().BeInRange(coeffs[2] - 1e-5, coeffs[2] + 1e-5);
        model.FittingResult.C[3].Should().BeInRange(coeffs[3] - 1e-5, coeffs[3] + 1e-5);
    }

    [Fact]
    public async Task CurveFit_ForQubicModel2_Success()
    {
        var points = new[] {1.0};
        var coeffs = new[] {-2.5 };
        Func<double, double> f = (x) => coeffs[0] * x * x;
        var y = points.Select(t => f(t)).ToArray();

        var xLb = new DenseVector(points) ;
        var xUb = new DenseVector(points) ;
        var yLb = new DenseVector(y);
        var yUb = new DenseVector(y);

        var mixins = new List<IIntervalExtensions>()
        {
            new EvenFunction((t) => t * t),
        };

        var model = new ExpWithMixinModel(mixins);

        var aLb = new DenseVector([0.0000]);
        var aUb = new DenseVector([0.0000]);
        var bLb = new DenseVector([0.0000]);
        var bUb = new DenseVector([0.0000]);
        var cLb = new DenseVector([-15.0]);
        var cUb = new DenseVector([0.0]);

        var penatlyOptions = new PenatlyOptionsWithMixin()
        {
            ALb = aLb,
            AUb = aUb,
            BLb = bLb,
            BUb = bUb,
            CLb = cLb,
            CUb = cUb,
            CostA = 2,
            CostB = 3,
            CostC = 4,
        };

        
        var tol = new ExpTolWithPenatlyAndMixin(xLb, xUb, yLb, yUb, penatlyOptions, mixins);
        var t = tol.TolValue(aLb, bLb, new DenseVector([-2.51]));
        var t2 = tol.GradA(aLb, bLb, new DenseVector([-2.50]));
        var t3 = tol.GradB(aLb, bLb, new DenseVector([-2.50]));
        var t4 = tol.GradC(aLb, bLb, new DenseVector([-2.50]));

        
        await model.Fit(
            xLb,
            xUb,
            yLb,
            yUb,
            penatlyOptions,
            new OptimizationOptions());

        var r = tol.CalcRmse(aLb, bLb, new DenseVector(coeffs));
        model.FittingResult.Should().NotBeNull();
        model.FittingResult.A[0].Should().BeInRange(-0.0001, 0.0001);
        model.FittingResult.B[0].Should().BeInRange(-0.0001, 0.0001);
        model.FittingResult.C[0].Should().BeInRange(coeffs[0] - 1e-5, coeffs[0] + 1e-5);
    }

    
}