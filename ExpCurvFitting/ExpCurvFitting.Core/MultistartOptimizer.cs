using MathNet.Numerics.Optimization;

namespace ExpCurvFitting.Core
{
    public class MultistartOptimizer
    {
        private readonly IUnconstrainedMinimizer _minimizer;

        public MultistartOptimizer(IUnconstrainedMinimizer minimizer)
        {
            _minimizer = minimizer;
        }

        public void Start(int countStart)
        {

        }
    }
}
