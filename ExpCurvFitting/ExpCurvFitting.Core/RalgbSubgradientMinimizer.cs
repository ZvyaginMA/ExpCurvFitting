using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;
using MathNet.Numerics.Optimization.LineSearch;

namespace ExpCurvFitting.Core
{
    internal class RalgbSubgradientMinimizer : IUnconstrainedMinimizer
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

            
            return new MinimizationWithLineSearchResult(objective, 0, ExitCondition.AbsoluteGradient, totalLineSearchSteps, iterationsWithNontrivialLineSearch);
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

        static void ValidateObjective(IObjectiveFunctionEvaluation objective)
        {
            if (double.IsNaN(objective.Value) || double.IsInfinity(objective.Value))
            {
                throw new EvaluationException("Non-finite objective function returned.", objective);
            }
        }

        public MinimizationResult FindMinimum(IObjectiveFunction objective, Vector<double> initialGuess)
        {
            throw new NotImplementedException();
        }
    }
}
