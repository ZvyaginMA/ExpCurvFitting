using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;

namespace ExpCurvFitting.Core.Optimization
{
    public class RalgbSubgradientMinimizer : IUnconstrainedMinimizer
    {
        public double GradientTolerance { get; set; }
        public int MaximumIterations { get; set; }

        public RalgbSubgradientMinimizer(double gradientTolerance, int maximumIterations)
        {
            GradientTolerance = gradientTolerance;
            MaximumIterations = maximumIterations;
        }

        public MinimizationResult FindMinimum(IObjectiveFunction objective, Vector<double> initialGuess)
        {
            return Minimum(objective, initialGuess, GradientTolerance, MaximumIterations);
        }

        public static MinimizationResult Minimum(IObjectiveFunction objective, Vector<double> initialGuess, double gradientTolerance = 1e-8, int maxIterations = 1000)
        {
            if (!objective.IsGradientSupported)
            {
                throw new IncompatibleObjectiveException("Gradient not supported in objective function, but required for ConjugateGradient minimization.");
            }

            objective.EvaluateAt(initialGuess);
            var gradient = objective.Gradient;
            ValidateGradient(objective);

            // Check that we're not already done
            if (gradient.Norm(2.0) < gradientTolerance)
            {
                return new MinimizationResult(objective, 0, ExitCondition.AbsoluteGradient);
            }
            var B = Matrix<double>.Build.DenseIdentity(initialGuess.Count);
            var iteration = 0;
            var tolx = 1e-5;
            var gradient1 = objective.Gradient;
            var x = initialGuess;
            var xr = initialGuess;
            var f = objective.Value;
            var fr = objective.Value;
            var q1 = 0.9;
            var q2 = 1.1;
            var alpha = 2.3;
            var w = 1 / alpha - 1;
            var h0 = 1.0;
            while (iteration < maxIterations)
            {
                gradient1 = B.TransposeThisAndMultiply(gradient);
                if (gradient1.Norm(2.0) < gradientTolerance)
                {
                    return new MinimizationResult(objective, iteration, ExitCondition.AbsoluteGradient);
                }

                var dx = B.Multiply(gradient1 / gradient1.L2Norm());
                var normadx = dx.L2Norm();
                var hs = h0;
                var d = 1.0;
                var cal = 0;
                var ncall = 0;
                var deltax = 0.0;

                while (d > 0 && cal <= 500)
                {
                    x = x - hs * dx;
                    deltax = deltax + hs * normadx;
                    ncall++;
                    objective.EvaluateAt(x);
                    gradient1 = objective.Gradient;
                    f = objective.Value;

                    if (f < fr)
                    {
                        fr = f;
                        xr = x;
                    }

                    if (cal % 5 == 0)
                    {
                        hs = hs * q2;
                    }


                    if (gradient1.Norm(2.0) < gradientTolerance)
                    {
                        return new MinimizationResult(objective, iteration, ExitCondition.AbsoluteGradient);
                    }

                    d = dx.DotProduct(gradient1);
                    cal++;
                }

                if (cal > 500)
                {
                    return new MinimizationResult(objective, iteration, ExitCondition.AbsoluteGradient);
                }

                if (cal == 1)
                {
                    hs = hs * q1;
                }

                if (deltax < tolx)
                {
                    return new MinimizationResult(objective, iteration, ExitCondition.AbsoluteGradient);
                }

                var dg = B.TransposeThisAndMultiply(gradient1 - gradient);
                var xi = dg / dg.L2Norm();
                B = B + w * B.Multiply(xi).OuterProduct(xi);
                gradient = gradient1;
                if (false)
                {
                    return new MinimizationResult(objective, iteration, ExitCondition.AbsoluteGradient);
                }
            }



            return new MinimizationWithLineSearchResult(objective, 0, ExitCondition.AbsoluteGradient, 0, 0);
        }

        static void ValidateGradient(IObjectiveFunctionEvaluation objective)
        {
            foreach (var x in objective.Gradient)
            {
                if (double.IsNaN(x) || double.IsInfinity(x))
                {
                    throw new EvaluationException("Non-finite gradient returned.", objective);
                }
            }
        }
    }
}
