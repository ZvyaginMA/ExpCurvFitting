using ExpCurvFitting.Core.FunctionalExtension;
using ExpCurvFitting.Core.Models;
using FluentAssertions;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ExpCurvFitting.Test
{
    public class FunctionalExtensionsTests
    {
        [Fact]
        public void Test()
        {
            var linear = new MonotonicFunction((x) => x);
            Assert.Equal(1.5, linear.Mid(1, 2), 3);
            Assert.Equal(0.5, linear.Rad(1, 2), 3);
            var qubic = new MonotonicFunction((x) => x * x * x);
            Assert.Equal(4.5, qubic.Mid(1, 2), 3);
            Assert.Equal(3.5, qubic.Rad(1, 2), 3);
            var sqr = new UnimodalFunction((x) => x * x + 2);
            Assert.Equal(4.5, sqr.Mid(1, 2), 3);
            Assert.Equal(4, sqr.Mid(-1, 2), 3);
            Assert.Equal(4.5, sqr.Mid(-2, -1), 3);
            Assert.Equal(1.5, sqr.Rad(1, 2), 3);
            Assert.Equal(2, sqr.Rad(-1, 2), 3);
            Assert.Equal(1.5, sqr.Rad(-2, -1), 3);
        }

        [Fact]
        public async Task test2()
        {
            


        }
    }
}
