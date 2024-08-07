using ExpCurvFitting.Core;
using MathNet.Numerics.LinearAlgebra.Double;
using FluentAssertions;
using System.Diagnostics;
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
            var tol = new TolWithPenatly(xLb, xUb, yLb, yUb, penatlyOptions);
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

        [Fact]
        public void TestPenatlyGradA()
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
            var tol = new TolWithPenatly(xLb, xUb, yLb, yUb, penatlyOptions);
            var b = new DenseVector([1.7, 2.5]);

            var a = new DenseVector([1.5, 1.5]);
            var gradA1 = tol.GradAPenatly(a, b);
            gradA1.Should().BeEquivalentTo(new DenseVector([0.0, 1.0]));
            a = new DenseVector([1.5, 3.5]);
            var gradA2 = tol.GradAPenatly(a, b);
            gradA2.Should().BeEquivalentTo(new DenseVector([0.0, -1.0]));
            a = new DenseVector([1.5, 2.5]);
            var gradA3 = tol.GradAPenatly(a, b);
            gradA3.Should().BeEquivalentTo(new DenseVector([0.0, 0.0]));
        }

        [Fact]
        public void TestPenatlyGradB()
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
            var tol = new TolWithPenatly(xLb, xUb, yLb, yUb, penatlyOptions);
            var a = new DenseVector([1.5, 2.5]);

            var b = new DenseVector([0.7, 2.5]);
            var gradA1 = tol.GradBPenatly(a, b);
            gradA1.Should().BeEquivalentTo(new DenseVector([1.0, 0.0]));
            b = new DenseVector([1.7, 2.5]);
            var gradA2 = tol.GradBPenatly(a, b);
            gradA2.Should().BeEquivalentTo(new DenseVector([0.0, 0.0]));
            b = new DenseVector([2.7, 2.5]);
            var gradA3 = tol.GradBPenatly(a, b);
            gradA3.Should().BeEquivalentTo(new DenseVector([-1.0, 0.0]));
        }
    }
}
