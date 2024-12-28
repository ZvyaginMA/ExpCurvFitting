﻿using ExpCurvFitting.Core.RecognizingFunctions;
using ExpCurvFitting.Core.Optimization;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using ExpCurvFitting.Core.FunctionalExtensions;

namespace ExpCurvFitting.Core.Models;
public record ExpWithMixinModel
{
    public OptimizationWithMixinResult FittingResult { get; private set; }
    public readonly IList<IIntervalExtensions> _mixinFunctions;
    
    public ExpWithMixinModel(
        IList<IIntervalExtensions> mixinFunctions)
    {
        _mixinFunctions = mixinFunctions;
    }
    public async Task Fit(
        Vector<double> xLb, 
        Vector<double> xUb, 
        Vector<double> yLb, 
        Vector<double> yUb, 
        PenatlyOptionsWithMixin penatlyOption,
        OptimizationOptions optimizationOptions,
        CancellationToken cancellationToken = default)
    {
        var tol = new ExpTolWithPenatlyAndMixin(xLb, xUb, yLb, yUb, penatlyOption, _mixinFunctions);
        FittingResult = (await tol.MultistartOptimization(
            new RalgbSubgradientMinimizer(
                optimizationOptions.GradientTolerance, 
                optimizationOptions.MaximumIterations), 
            optimizationOptions.CountMultistarts, 
            penatlyOption.ALb.Count*2 + penatlyOption.CLb.Count, 
            cancellationToken)).ToOptimizationWithMixinResult(penatlyOption.ALb.Count, penatlyOption.CLb.Count);
    }

    public record Result
    {
        public double TolValue { get; init; }
        public double RmseForCenter { get; init; }
        public double TimeCalculation { get; init; }
        public double[] A { get; init; }
        public double[] B { get; init; }
        public double[] C { get; init; }
        public double MinYRad { get; init; }
        public IEnumerable<(double, double)> GetPoints()
        {
            for (int i = 0; i < A.Length; i++)
            {
                yield return new(A[i], B[i]);
            }
        }
        
        public IEnumerable<(double, double)> GetC()
        {
            for (int i = 0; i < C.Length; i++)
            {
                yield return new(i, C[i]);
            }
        }
    }

    public Result GetResult()
    {
        return new Result
        {
            TolValue = FittingResult.TolValue,
            A = FittingResult.A.AsArray(),
            B = FittingResult.B.AsArray(),
            C = FittingResult.C.AsArray(),
            RmseForCenter = FittingResult.Rmse,
            TimeCalculation = FittingResult.TimeCalculation.TotalSeconds,
            MinYRad = FittingResult.MinYRad
        };
    }
}
