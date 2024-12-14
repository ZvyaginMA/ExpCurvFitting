using System;

namespace ExpCurvFitting.Core.FunctionalExtensions
{
    public class MonotonicFunction : IIntervalExtensions
    {
        private readonly Func<double, double> _function;

        public MonotonicFunction(Func<double, double> function)
        {
            _function = function;
        }

        public double Val(double t) => _function(t);

        public double Rad(double lowerBound, double upperBound)
            => 0.5 * Math.Abs(_function(lowerBound) - _function(upperBound));
        public double Mid(double lowerBound, double upperBound)
            => 0.5 * (_function(lowerBound) + _function(upperBound));
    }
}
