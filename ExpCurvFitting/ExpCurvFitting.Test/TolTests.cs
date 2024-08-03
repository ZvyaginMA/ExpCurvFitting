using ExpCurvFitting.Core;
using FluentAssertions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ExpCurvFitting.Test
{
    public class TolTests
    {
        [Fact]
        public void Test1()
        {
            var xLb = new DenseVector(new[] { 1.0, 2.0 });
            var xUb = new DenseVector(new[] { 1.0, 2.0 });
            var yLb = new DenseVector(new[] { 1.0, 2.0 });
            var yUb = new DenseVector(new[] { 1.0, 2.0 });
            var tol = new Tol(xLb, xUb, yLb, yUb);

            var a = new DenseVector(new[] { 1.0, 2.0 });
            var b = new DenseVector(new[] { 1.0, 2.0 });

            var tolResult = tol.TolValue(a, b);
            var gradAResult = tol.GradA(a, b);
            var gradBResult = tol.GradB(a, b);
            tolResult.Should().BeInRange(-1.82804, -1.82803);
            gradAResult[0].Should().BeInRange(0.135335, 0.135336);
            gradAResult[1].Should().BeInRange(0.018315, 0.018316);
            gradBResult[0].Should().BeInRange(-0.270671, -0.270670);
            gradBResult[1].Should().BeInRange(-0.073263, -0.073262);
        }
    }
}