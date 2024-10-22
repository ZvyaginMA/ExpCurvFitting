using MathNet.Numerics.LinearAlgebra;

namespace ExpCurvFitting.Application
{
    public class InputData
    {
        private IReadOnlyList<IntervalVector> InputVariable { get; set; }
        private IntervalVector OutputVariable { get; set; }

        public InputData(IntervalVector inputVariable, IntervalVector outputVariable)
        {

        }
    }

    public class IntervalVector
    {
        public Vector<double> LowerBound { get; set; }
        public Vector<double> UpperBound { get; set; }
    }
}
