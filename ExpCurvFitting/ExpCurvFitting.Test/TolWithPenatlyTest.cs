using ExpCurvFitting.Core;
using MathNet.Numerics.LinearAlgebra.Double;
using FluentAssertions;
using ExpCurvFitting.Core.RecognizingFunctions;

namespace ExpCurvFitting.Test
{
    public class TolWithPenatlyTest
    {
        [Fact]
        public void SuccessOptimization()
        {
            var xLb = new DenseVector([1.0, 2.0, 3.0, 4, 5, 6]);
            var xUb = new DenseVector([1.0, 2.0, 3.0, 4, 5, 6]);
            var yLb = new DenseVector([2.51, 2.04, 1.67, 1.37, 1.12, 0.93]) - 0.1;
            var yUb = new DenseVector([2.51, 2.04, 1.67, 1.37, 1.12, 0.93]) + 0.1;

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
                CostA = 5,
                CostB = 5,
            };

            var tol = new TolWithPenatly(xLb, xUb, yLb, yUb, penatlyOptions);
            var result = tol.Optimization(new RalgbSubgradientMinimizer(1e-5, 10000), new DenseVector([1.0, 2.0, 5.0, 4.0]));
            result.TolValue.Should().BeInRange(0.09, 0.1);
        }

        [Theory]
        [InlineData(new double[] { 10.0, 10.0 }, new double[] { 20.0, 20.0 }, new double[] { 15.0, 15.0 }, new double[] { 20.0, 20.0 })]
        [InlineData(new double[] { -10.0, -7.0 }, new double[] { -8, -6 }, new double[] { 0, 1 }, new double[] { 2, 1.0001 })]
        [InlineData(new double[] { -10.0001, -8.0001 }, new double[] { -10.0, -8.0 }, new double[] {0, 0 }, new double[] { 1000.0, 1000.0 })]
        public void SuccessOptimization2(double[] aLbValues, double[] aUbValues, double[] bLbValues, double[] bUbValues)
        {
            var xLb = new DenseVector([1.0, 2.0, 3.0, 4, 5, 6]);
            var xUb = new DenseVector([1.0, 2.0, 3.0, 4, 5, 6]);
            var yLb = new DenseVector([2.51, 2.04, 1.67, 1.37, 1.12, 0.93]) - 0.1;
            var yUb = new DenseVector([2.51, 2.04, 1.67, 1.37, 1.12, 0.93]) + 0.1;

            var aLb = new DenseVector(aLbValues);
            var aUb = new DenseVector(aUbValues);
            var bLb = new DenseVector(bLbValues);
            var bUb = new DenseVector(bUbValues);

            var penatlyOptions = new PenatlyOptions()
            {
                ALb = aLb,
                AUb = aUb,
                BLb = bLb,
                BUb = bUb,
                CostA = 5,
                CostB = 5,
            };

            var tol = new TolWithPenatly(xLb, xUb, yLb, yUb, penatlyOptions);
            var result = tol.Optimization(new RalgbSubgradientMinimizer(1e-5, 10000), new DenseVector([1.0, 2.0, 5.0, 4.0]));
            result.A[0].Should().BeInRange(penatlyOptions.ALb[0], penatlyOptions.AUb[0]);
            result.A[1].Should().BeInRange(penatlyOptions.ALb[1], penatlyOptions.AUb[1]);
            result.B[0].Should().BeInRange(penatlyOptions.BLb[0], penatlyOptions.BUb[0]);
            result.B[1].Should().BeInRange(penatlyOptions.BLb[1], penatlyOptions.BUb[1]);
        }
    }
}
