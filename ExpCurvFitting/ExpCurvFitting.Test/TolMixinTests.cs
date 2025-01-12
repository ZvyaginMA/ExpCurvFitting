using ExpCurvFitting.Core.FunctionalExtension;
using ExpCurvFitting.Core.Models;
using ExpCurvFitting.Core.Optimization;
using ExpCurvFitting.Core.RecognizingFunctions;
using FluentAssertions;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Optimization;

namespace ExpCurvFitting.Test;

public class TolMixinTests
{
    [Fact]
    public async Task Test()
    {
        var x = new DenseVector([1.0, 2.0, 3, 4, 5, 6]);
        var yMid = new DenseVector([2.51, 2.04, 1.67, 1.37, 1.12, 0.93]);
        var yRad = new DenseVector([1.0, 1.0, 1.0, 1.0, 1.0, 1.0]) * 0.01;
        
        var aLb = new DenseVector([0.001, 0.001]);
        var aUb = new DenseVector([10.0, 10.0]);
        var bLb = new DenseVector([0.001, 0.001]);
        var bUb = new DenseVector([10.0, 10.0]);
        var cLb = new DenseVector([0.0]);
        var cUb = new DenseVector([2.0]);
        
        var penatlyOptions = new PenatlyOptionsWithMixin()
        {
            ALb = aLb,
            AUb = aUb,
            BLb = bLb,
            BUb = bUb,
            CLb = cLb,
            CUb = cUb, 
            CostA = 10,
            CostB = 10,
            CostC = 20,
        };

        var mixins = new List<IIntervalExtension>()
        {
            new MonotonicFunction((t) => Math.Pow(t, -2.5)),
        };

        var tol = new ExpTolWithPenatlyAndMixin(
            x, x, yMid - yRad, yMid + yRad,
            penatlyOptions,
            mixins);
        
        var a = new DenseVector([1.5, 2.5]);
        var b = new DenseVector([1.5, 2.5]);
        var c = new DenseVector([1.5]);
        var tolValue = tol.TolValue(a, b, c);
        tolValue.Should().BeInRange(-1.7, -1.6);
        
        
        var result = tol.Optimization(
            new RalgbSubgradientMinimizer(1e-10, 10000),
            new DenseVector([1.5, 2.5, 1.5, 2.5, 1.5]),
            CancellationToken.None)
            .ToOptimizationWithMixinResult(2, 1);
        result.TolValue.Should().BeInRange(0.005, 0.006);
        
        var multistarResult = (await tol.MultistartOptimization(new RalgbSubgradientMinimizer(1e-10, 10000),500, 5)).ToOptimizationWithMixinResult(2, 1);
        multistarResult.TolValue.Should().BeInRange(0.006, 0.007);
    }
    
    //[Fact]
    public async Task TestStopedCalc()
    {
        var x = new DenseVector([1.0, 2.0, 3, 4, 5, 6]);
        var yMid = new DenseVector([2.51, 2.04, 1.67, 1.37, 1.12, 0.93]);
        var yRad = new DenseVector([1.0, 1.0, 1.0, 1.0, 1.0, 1.0]) * 0.01;
        
        var aLb = new DenseVector([0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001]);
        var aUb = new DenseVector([10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0]);
        var bLb = new DenseVector([0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001, 0.001]);
        var bUb = new DenseVector([10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0, 10.0]);
        var cLb = new DenseVector([0.0]);
        var cUb = new DenseVector([2.0]);
        
        var penatlyOptions = new PenatlyOptionsWithMixin()
        {
            ALb = aLb,
            AUb = aUb,
            BLb = bLb,
            BUb = bUb,
            CLb = cLb,
            CUb = cUb, 
            CostA = 10,
            CostB = 10,
            CostC = 20,
        };

        var mixins = new List<IIntervalExtension>()
        {
            new MonotonicFunction((t) => Math.Pow(t, -2.5)),
        };

        var tol = new ExpTolWithPenatlyAndMixin(
            x, x, yMid - yRad, yMid + yRad,
            penatlyOptions,
            mixins);

        var cts = new CancellationTokenSource();
        cts.CancelAfter(10);
        var multistarResult = (await tol.MultistartOptimization(
            new RalgbSubgradientMinimizer(1e-13, 10000),
            500, 12+12+1,
            cts.Token)).ToOptimizationWithMixinResult(12, 1);

        multistarResult.ReasonForExit.Should().Be(ExitCondition.ManuallyStopped);
        multistarResult.TimeCalculation.Should().BeLessThan(new TimeSpan(0, 0, 0, 0, 2000));
    }
}