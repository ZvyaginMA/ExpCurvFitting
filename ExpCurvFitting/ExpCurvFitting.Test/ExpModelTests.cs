using ExpCurvFitting.Core;
using FluentAssertions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ExpCurvFitting.Test
{
    public class ExpModelTests
    {
        [Fact]
        public async Task Test()
        {
            var x = new DenseVector([1.0, 2.0, 3, 4, 5, 6]);
            var yMid = new DenseVector([2.51, 2.04, 1.67, 1.37, 1.12, 0.93]);
            var yRad = new DenseVector([1.0, 1.0, 1, 1, 1, 1]) * 0.01;
            var model = new ExpModel();

            var aLb = new DenseVector([0.0, 0.0]);
            var aUb = new DenseVector([10.0, 10.0]);
            var bLb = new DenseVector([0.0, 0.0]);
            var bUb = new DenseVector([10.0, 10.0]);

            var penatlyOptions = new PenatlyOptions()
            {
                ALb = aLb,
                AUb = aUb,
                BLb = bLb,
                BUb = bUb,
            };

            await model.FitForNonIntervalX(x, yMid, yRad, penatlyOptions);
            model.FittingResult.TolValue.Should().BeInRange(0.006, 0.007);
            model.FittingResult.Rmse.Should().BeInRange(0.002, 0.003);
        }
    }
}
