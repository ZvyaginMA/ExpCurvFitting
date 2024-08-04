using ExpCurvFitting.Core;
using FluentAssertions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ExpCurvFitting.Test
{
    public class TolTests
    {
        [Fact]
        public void SuccessCalcProperties()
        {
            var xLb = new DenseVector([1.0, 2.0]);
            var xUb = new DenseVector([1.0, 2.0]);
            var yLb = new DenseVector([1.0, 2.0]);
            var yUb = new DenseVector([1.0, 2.0]);
            var tol = new Tol(xLb, xUb, yLb, yUb);

            var a = new DenseVector([1.0, 2.0]);
            var b = new DenseVector([1.0, 2.0]);

            var tolResult = tol.TolValue(a, b);
            var gradAResult = tol.GradA(a, b);
            var gradBResult = tol.GradB(a, b);
            tolResult.Should().BeInRange(-1.82804, -1.82803);
            gradAResult[0].Should().BeInRange(0.135335, 0.135336);
            gradAResult[1].Should().BeInRange(0.018315, 0.018316);
            gradBResult[0].Should().BeInRange(-0.270671, -0.270670);
            gradBResult[1].Should().BeInRange(-0.073263, -0.073262);
        }

        [Fact]
        public void SuccessOptimization()
        {
            var xLb = new DenseVector([1.0, 2.0, 3.0, 4, 5, 6]);
            var xUb = new DenseVector([1.0, 2.0, 3.0, 4, 5, 6]);
            var yLb = new DenseVector([2.51, 2.04, 1.67, 1.37, 1.12, 0.93]) - 0.1;
            var yUb = new DenseVector([2.51, 2.04, 1.67, 1.37, 1.12, 0.93]) + 0.1;
            var tol = new Tol(xLb, xUb, yLb, yUb);
            var result = tol.Optimization(new RalgbSubgradientMinimizer(1e-5, 1000), new DenseVector([1.0, 2.0, 5.0, 4.0]));
            result.TolValue.Should().BeInRange(0.0942, 0.0943);
        }

        [Fact]
        public void SuccessMultistartOptimization()
        {
            var xLb = new DenseVector([1.0, 2.0, 3.0, 4, 5, 6]);
            var xUb = new DenseVector([1.0, 2.0, 3.0, 4, 5, 6]);
            var yLb = new DenseVector([2.51, 2.04, 1.67, 1.37, 1.12, 0.93]) - 0.1;
            var yUb = new DenseVector([2.51, 2.04, 1.67, 1.37, 1.12, 0.93]) + 0.1;
            var tol = new Tol(xLb, xUb, yLb, yUb);
            var result = tol.MultistartOptimization(new RalgbSubgradientMinimizer(1e-5, 1000), 20, 2);
            result.TolValue.Should().BeInRange(0.096, 0.097);
        }
    }
}