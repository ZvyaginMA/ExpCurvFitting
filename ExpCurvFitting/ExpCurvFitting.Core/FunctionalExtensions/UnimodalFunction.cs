namespace ExpCurvFitting.Core.FunctionalExtensions
{
    public record UnimodalFunction : IIntervalExtensions
    {
        private readonly Func<double, double> _function;
        private double OptimumPoint { get; init; }

        public UnimodalFunction(Func<double, double> function, double optimumPoint = 0.0)
        {
            _function = function;
            OptimumPoint = optimumPoint;
        }
        
        public double Val(double t) => _function(t);

        public double Mid(double lowerBound, double upperBound)
        {
            var (lb, ub) = CalcFunctionalEstimation(lowerBound, upperBound);
            return 0.5 * (lb + ub);
        }

        public double Rad(double lowerBound, double upperBound)
        {
            var (lb, ub) = CalcFunctionalEstimation(lowerBound, upperBound);
            return 0.5 * (ub - lb);
        }

        /// <summary>
        /// Calculate the lower and upper bounds of the function at the interval 
        /// </summary>
        private (double flb, double fub) CalcFunctionalEstimation(double lowerBound, double upperBound)
        {
            var f1 = _function(lowerBound);
            var f2 = _function(upperBound);
            var points = (lowerBound - OptimumPoint) * (upperBound - OptimumPoint) < 0 ? [f1, _function(OptimumPoint), f2] : new [] { f1, f2 };
            return (points.Min(), points.Max());
        }
    }
}
