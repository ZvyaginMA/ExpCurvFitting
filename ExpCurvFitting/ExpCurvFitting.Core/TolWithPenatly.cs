using MathNet.Numerics.LinearAlgebra;

namespace ExpCurvFitting.Core
{
    public record TolWithPenatly : Tol, ITol
    {
        public PenatlyOptions PenatlyOption { get; private set; }
        public TolWithPenatly(Vector<double> xLowerBound, Vector<double> xUpperBound, Vector<double> yLowerBound, Vector<double> yUpperBound, PenatlyOptions penatlyOption) : base(xLowerBound, xUpperBound, yLowerBound, yUpperBound)
        {
            PenatlyOption = penatlyOption;
        }

        public override double TolValue(Vector<double> a, Vector<double> b)
        {
            return base.TolValue(a, b) + CalcPenatly(a, b);
        }

        public override Vector<double> GradA(Vector<double> a, Vector<double> b)
        {
            return base.GradA(a, b) + GradAPenatly(a, b);
        }

        public override Vector<double> GradB(Vector<double> a, Vector<double> b)
        {
            return base.GradB(a, b) + GradBPenatly(a, b);
        }

        #region Penatly
        public double CalcPenatly(Vector<double> a, Vector<double> b)
        {
            var lowerPenA = (a - PenatlyOption.ALb) - (a - PenatlyOption.ALb).PointwiseAbs();
            var upperPenA = (PenatlyOption.AUb - a) - (PenatlyOption.AUb - a).PointwiseAbs();
            var lowerPenB = (b - PenatlyOption.BLb) - (b - PenatlyOption.BLb).PointwiseAbs();
            var upperPenB = (PenatlyOption.BUb - b) - (PenatlyOption.BUb - b).PointwiseAbs();
            return PenatlyOption.CostA * (lowerPenA + upperPenA).Sum() + PenatlyOption.CostB * (lowerPenB + upperPenB).Sum();
        }

        public Vector<double> GradAPenatly(Vector<double> a, Vector<double> b)
        {
            var lowerPenA = (a - PenatlyOption.ALb) - (a - PenatlyOption.ALb).PointwiseAbs();
            var upperPenA = (PenatlyOption.AUb - a) - (PenatlyOption.AUb - a).PointwiseAbs();
            return -PenatlyOption.CostA * (lowerPenA - upperPenA).PointwiseSign();
        }

        public Vector<double> GradBPenatly(Vector<double> a, Vector<double> b)
        {
            var lowerPenB = (b - PenatlyOption.BLb) - (b - PenatlyOption.BLb).PointwiseAbs();
            var upperPenB = (PenatlyOption.BUb - b) - (PenatlyOption.BUb - b).PointwiseAbs();
            return -PenatlyOption.CostB * (lowerPenB - upperPenB).PointwiseSign();
        }
        #endregion
    }
}
