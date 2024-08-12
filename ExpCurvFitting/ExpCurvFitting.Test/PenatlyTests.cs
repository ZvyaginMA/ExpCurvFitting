using ExpCurvFitting.Core.Models;
using MathNet.Numerics.LinearAlgebra.Double;
using FluentAssertions;
using ExpCurvFitting.Core.RecognizingFunctions;
namespace ExpCurvFitting.Test
{
    public class PenatlyTests
    {
        [Fact]
        public void TestPenatlyVal()
        {
            var xLb = new DenseVector([1.0, 2.0]);
            var xUb = new DenseVector([1.0, 2.0]);
            var yLb = new DenseVector([1.0, 2.0]);
            var yUb = new DenseVector([1.0, 2.0]);

            var aLb = new DenseVector([1.0, 2.0]);
            var aUb = new DenseVector([2.0, 3.0]);
            var bLb = new DenseVector([1.0, 2.0]);
            var bUb = new DenseVector([2.0, 3.0]);

            var penatlyOptions = new PenatlyOptions()
            {
                ALb = aLb,
                AUb = aUb,
                BLb = bLb,
                BUb = bUb,
                CostA = 1,
                CostB = 1,
            };
            var tol = new ExpTolWithPenatly(xLb, xUb, yLb, yUb, penatlyOptions);
            var a = new DenseVector([1.5, 1.5]);
            var b = new DenseVector([1.7, 2.5]);

            var result = tol.CalcPenatly(a, b);

            result.Should().BeInRange(-1.1, -0.9);

            a = new DenseVector([1.5, 1.5]);
            b = new DenseVector([2.7, 2.5]);

            result = tol.CalcPenatly(a, b);

            result.Should().BeInRange(-2.5, -2.4);

            a = new DenseVector([1.5, 1.5]);
            b = new DenseVector([1.7, 6.5]);

            result = tol.CalcPenatly(a, b);

            result.Should().BeInRange(-8.1, -7.1);
        }

        [Theory]
        [InlineData(1.5, 1.0)]
        [InlineData(2.5, 0.0)]
        [InlineData(3.5, -1.0)]
        public void TestPenatlyGradA(double val, double except)
        {
            var xLb = new DenseVector([1.0, 2.0]);
            var xUb = new DenseVector([1.0, 2.0]);
            var yLb = new DenseVector([1.0, 2.0]);
            var yUb = new DenseVector([1.0, 2.0]);

            var aLb = new DenseVector([1.0, 2.0]);
            var aUb = new DenseVector([2.0, 3.0]);
            var bLb = new DenseVector([1.0, 2.0]);
            var bUb = new DenseVector([2.0, 3.0]);

            var penatlyOptions = new PenatlyOptions()
            {
                ALb = aLb,
                AUb = aUb,
                BLb = bLb,
                BUb = bUb,
                CostA = 1,
                CostB = 1,
            };
            var tol = new ExpTolWithPenatly(xLb, xUb, yLb, yUb, penatlyOptions);
            var b = new DenseVector([1.7, 2.5]);

            var a = new DenseVector([1.5, val]);
            var gradA = tol.GradAPenatly(a, b);
            gradA.Should().BeEquivalentTo(new DenseVector([0.0, except]));
        }

        [Theory]
        [InlineData(0.7, 1.0)]
        [InlineData(1.7, 0.0)]
        [InlineData(2.7, -1.0)]
        public void TestPenatlyGradB(double val, double except)
        {
            var xLb = new DenseVector([1.0, 2.0]);
            var xUb = new DenseVector([1.0, 2.0]);
            var yLb = new DenseVector([1.0, 2.0]);
            var yUb = new DenseVector([1.0, 2.0]);

            var aLb = new DenseVector([1.0, 2.0]);
            var aUb = new DenseVector([2.0, 3.0]);
            var bLb = new DenseVector([1.0, 2.0]);
            var bUb = new DenseVector([2.0, 3.0]);

            var penatlyOptions = new PenatlyOptions()
            {
                ALb = aLb,
                AUb = aUb,
                BLb = bLb,
                BUb = bUb,
                CostA = 1,
                CostB = 1,
            };
            var tol = new ExpTolWithPenatly(xLb, xUb, yLb, yUb, penatlyOptions);
            var a = new DenseVector([1.5, 2.5]);

            var b = new DenseVector([val, 2.5]);
            var gradA = tol.GradBPenatly(a, b);
            gradA.Should().BeEquivalentTo(new DenseVector([except, 0.0]));
        }
    }
}
