using ExpCurvFitting.Core.Models;
using FluentAssertions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ExpCurvFitting.Test;

public class ExpModelWithMixinTests
{
    [Fact]
    public async Task CurveFit_ForExpModel_WithPowerMixin_Success()
    {
        var xMid = new DenseVector([0.12, 0.2 , 0.6 , 0.7 , 0.9 , 1.4 , 1.6 , 1.9 , 2.4]);
        var yLb = new DenseVector([2.564, 1.0253, 0.2068, 0.1517, 0.0753, -0.017, -0.0343, -0.0501, -0.0623]);
        var yUb = new DenseVector([2.704, 1.1653, 0.3468, 0.2917, 0.2153, 0.123, 0.1057, 0.0899, 0.0777]);
        
        var mixins = new List<Func<double, double>>()
        {
            (t) => Math.Pow(t, -2.5), 
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
}