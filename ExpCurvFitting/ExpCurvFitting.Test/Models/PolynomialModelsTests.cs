using ExpCurvFitting.Core.FunctionalExtension;
using ExpCurvFitting.Core.Models;
using FluentAssertions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ExpCurvFitting.Test;

public class PolynomialModelsTests
{
    /// <summary>
    /// Search for a linear function among polynomials of degree no higher than two.
    /// </summary>
    /// <param name="coeffs">Polynomial coefficients from the linear member</param>
    /// <param name="radData">x and y error radius</param>
    /// <param name="exceptedError">Expected error in the estimation coefficients </param>
    /// <param name="higherEstRmse">higher estimation rmse</param>
    /// <param name="lowerEstTolVal">lower estimation tol</param>
    [Theory]
    [InlineData(new[] { -1.2, 0.7 }, 1e-3, 1e-3, 1e-5, -1e-5)] 
    [InlineData(new[] { 21.0, -8 }, 1e-2, 1e-1, 1e-5, -1e-5)] 
    public async Task LinearModel(double[] coeffs, double radData, double exceptedError, double higherEstRmse, double lowerEstTolVal)
    {
        var points = new[] { -2.0, 0.5, 1, 1.2 };
        Func<double, double> f = (x) => coeffs[0] + coeffs[1] * x;
        var y = points.Select(t => f(t)).ToArray();

        var xLb = new DenseVector(points) + radData;
        var xUb = new DenseVector(points) + radData;
        var yLb = new DenseVector(y) + radData;
        var yUb = new DenseVector(y) + radData;

        var mixins = new List<IIntervalExtension>()
        {
            new MonotonicFunction((t) => 1),
            new MonotonicFunction((t) => t),
            new UnimodalFunction((t) => t * t),
        };

        var model = new ExpWithMixinModel(mixins);

        var aLb = new DenseVector([0.0000]);
        var aUb = new DenseVector([0.0000]);
        var bLb = new DenseVector([0.0000]);
        var bUb = new DenseVector([0.0000]);
        var cLb = new DenseVector([-100.0, -100.0, -100.0]);
        var cUb = new DenseVector([100.1, 100.1, 100.1]);

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
            xLb,
            xUb,
            yLb,
            yUb,
            penatlyOptions,
            new OptimizationOptions());

        model.FittingResult.TolValue.Should().BeGreaterThan(lowerEstTolVal);
        model.FittingResult.Rmse.Should().BeLessThan(higherEstRmse);
        model.FittingResult.A[0].Should().BeInRange(-0.0001, 0.0001);
        model.FittingResult.B[0].Should().BeInRange(-0.0001, 0.0001);
        model.FittingResult.C[0].Should().BeInRange(coeffs[0] - exceptedError, coeffs[0] + exceptedError);
        model.FittingResult.C[1].Should().BeInRange(coeffs[1] - exceptedError, coeffs[1] + exceptedError);
        model.FittingResult.C[2].Should().BeInRange(-0.0001, 0.0001);
    }

    /// <summary>
    /// Search for a quadratic function among polynomials of degree no higher than three.
    /// </summary>
    /// <param name="coeffs">Polynomial coefficients from the linear member</param>
    /// <param name="radData">x and y error radius</param>
    /// <param name="exceptedError">Expected error in the estimation coefficients </param>
    /// <param name="higherEstRmse">higher estimation rmse</param>
    /// <param name="lowerEstTolVal">lower estimation tol</param>
    [Theory]
    [InlineData(new[] { -1.2, -1.3, 2.1 }, 1e-2, 1e-1, 1e-5, -1e-5)] 
    [InlineData(new[] { 5, -4, 8.0 }, 1e-2, 2e-1, 1e-5, -1e-5)] 
    public async Task Quadratic(double[] coeffs, double radData, double exceptedError, double higherEstRmse, double lowerEstTolVal)
    {
        var points = new[] { 1.0, 1.5, 2.1, 3.2, 3.3, 3.8, 4.1, 4.7, 4.9 };
        Func<double, double> f = (x) => coeffs[0] + coeffs[1] * x + coeffs[2] * x * x;
        var y = points.Select(t => f(t)).ToArray();

        var xLb = new DenseVector(points) + radData;
        var xUb = new DenseVector(points) + radData;
        var yLb = new DenseVector(y) + radData;
        var yUb = new DenseVector(y) + radData;

        var mixins = new List<IIntervalExtension>()
        {
            new MonotonicFunction((t) => 1),
            new MonotonicFunction((t) => t),
            new UnimodalFunction((t) => t * t),
            new MonotonicFunction((t) => t * t * t),
        };

        var model = new ExpWithMixinModel(mixins);

        var aLb = new DenseVector([0.0000]);
        var aUb = new DenseVector([0.0000]);
        var bLb = new DenseVector([0.0000]);
        var bUb = new DenseVector([0.0000]);
        var cLb = new DenseVector([-100.0, -100.0, -100.0, -100.0]);
        var cUb = new DenseVector([100.1, 100.1, 100.1, 100.1]);

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
            xLb,
            xUb,
            yLb,
            yUb,
            penatlyOptions,
            new OptimizationOptions());

        model.FittingResult.TolValue.Should().BeGreaterThan(lowerEstTolVal);
        model.FittingResult.Rmse.Should().BeLessThan(higherEstRmse);
        model.FittingResult.A[0].Should().BeInRange(-0.0001, 0.0001);
        model.FittingResult.B[0].Should().BeInRange(-0.0001, 0.0001);
        model.FittingResult.C[0].Should().BeInRange(coeffs[0] - exceptedError, coeffs[0] + exceptedError);
        model.FittingResult.C[1].Should().BeInRange(coeffs[1] - exceptedError, coeffs[1] + exceptedError);
        model.FittingResult.C[2].Should().BeInRange(coeffs[2] - exceptedError, coeffs[2] + exceptedError);
        model.FittingResult.C[3].Should().BeInRange(-0.0001, 0.0001);
    }
    
    /// <summary>
    /// Search for a polynomial 4 degree function among polynomials of degree no higher than 5.
    /// </summary>
    /// <param name="coeffs">Polynomial coefficients from the linear member</param>
    /// <param name="radData">x and y error radius</param>
    /// <param name="exceptedError">Expected error in the estimation coefficients </param>
    /// <param name="higherEstRmse">higher estimation rmse</param>
    /// <param name="lowerEstTolVal">lower estimation tol</param>
    [Theory]
    [InlineData(new[] { 1, -2, -3, 4.0, -5 }, 1e-2, 2.001e-1, 1e-6, -1e-5)] 
    [InlineData(new[] { -0.1, -1.2, 0.7, -0.9, 1.1 }, 1e-2, 5e-2, 1e-6, -1e-5)] 
    public async Task FourDegreePolu(double[] coeffs, double radData, double exceptedError, double higherEstRmse, double lowerEstTolVal)
    {
        var points = new[] { 1.0, 1.5, 2.1, 3.2, 3.3, 3.8, 4.1, 4.7, 4.9 };
        Func<double, double> f = (x) => coeffs[0] + coeffs[1] * x + coeffs[2] * x * x + coeffs[3] * x * x * x + coeffs[4] * x * x * x * x;
        var y = points.Select(t => f(t)).ToArray();

        var xLb = new DenseVector(points) + radData;
        var xUb = new DenseVector(points) + radData;
        var yLb = new DenseVector(y) + radData;
        var yUb = new DenseVector(y) + radData;

        var mixins = new List<IIntervalExtension>()
        {
            new MonotonicFunction((t) => 1),
            new MonotonicFunction((t) => t),
            new UnimodalFunction((t) => Math.Pow(t, 2)),
            new MonotonicFunction((t) => Math.Pow(t, 3)),
            new UnimodalFunction((t) => Math.Pow(t, 4)),
            new MonotonicFunction((t) => Math.Pow(t, 5)),
        };

        var model = new ExpWithMixinModel(mixins);

        var aLb = new DenseVector([0.0000]);
        var aUb = new DenseVector([0.0000]);
        var bLb = new DenseVector([0.0000]);
        var bUb = new DenseVector([0.0000]);
        var cLb = new DenseVector([-100.0, -100.0, -100.0, -100.0, -100.0, -100.0]);
        var cUb = new DenseVector([100.1, 100.1, 100.1, 100.1, 100.0, 100.0]);

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
            xLb,
            xUb,
            yLb,
            yUb,
            penatlyOptions,
            new OptimizationOptions());

        model.FittingResult.TolValue.Should().BeGreaterThan(lowerEstTolVal);
        model.FittingResult.Rmse.Should().BeLessThan(higherEstRmse);
        model.FittingResult.A[0].Should().BeInRange(-0.0001, 0.0001);
        model.FittingResult.B[0].Should().BeInRange(-0.0001, 0.0001);
        model.FittingResult.C[0].Should().BeInRange(coeffs[0] - exceptedError, coeffs[0] + exceptedError);
        model.FittingResult.C[1].Should().BeInRange(coeffs[1] - exceptedError, coeffs[1] + exceptedError);
        model.FittingResult.C[2].Should().BeInRange(coeffs[2] - exceptedError, coeffs[2] + exceptedError);
        model.FittingResult.C[3].Should().BeInRange(coeffs[3] - exceptedError, coeffs[3] + exceptedError);
        model.FittingResult.C[4].Should().BeInRange(coeffs[4] - exceptedError, coeffs[4] + exceptedError);
        model.FittingResult.C[5].Should().BeInRange(-0.0001, 0.0001);
    }
}