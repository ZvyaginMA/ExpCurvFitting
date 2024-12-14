namespace ExpCurvFitting.Core.FunctionalExtensions
{
    public class EvenFunction : IIntervalExtensions
    {
        private readonly Func<double, double> _function;

        public EvenFunction(Func<double, double> function)
        {
            _function = function;
        }
        public double Val(double t) => _function(t);

        public double Mid(double lowerBound, double upperBound)
        {
            var f1 = _function(lowerBound);
            var f2 = _function(upperBound);
            var points = f1 * f2 < 0 ? [f1, _function(0), f2] : new [] { f1, f2 };
            var lb = points.Min();
            var ub = points.Max();
            return 0.5 * (lb + ub);
        }

        public double Rad(double lowerBound, double upperBound)
        {
            var f1 = _function(lowerBound);
            var f2 = _function(upperBound);
            var points = f1 * f2 < 0 ? [f1, _function(0), f2] : new[] { f1, f2 };
            var lb = points.Min();
            var ub = points.Max();
            return 0.5 * (ub - lb);
        }
    }
}
