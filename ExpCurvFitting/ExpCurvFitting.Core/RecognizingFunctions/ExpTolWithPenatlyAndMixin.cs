using ExpCurvFitting.Core.Models;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Optimization;

namespace ExpCurvFitting.Core.RecognizingFunctions;
public record ExpTolWithPenatlyAndMixin : ExpTol
{
    public IList<Func<double, double>> MixinFunction { get; init; }
    public PenatlyOptionsWithMixin PenatlyOption { get; init; }
    public ExpTolWithPenatlyAndMixin(
        Vector<double> xLowerBound, 
        Vector<double> xUpperBound, 
        Vector<double> yLowerBound, 
        Vector<double> yUpperBound, 
        PenatlyOptionsWithMixin penatlyOption, 
        IList<Func<double, double>> mixinFunctions) : base(xLowerBound, xUpperBound, yLowerBound, yUpperBound)
    {
        PenatlyOption = penatlyOption;
        MixinFunction = mixinFunctions;
    }
    
    public double TolValue(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        return CalcGeneratrix(a, b, c).Min() + CalcPenatly(a, b, c);
    }
    
    private Vector<double> CalcGeneratrix(Vector<double> a, Vector<double> b,  Vector<double> c)
    {
        var result = Vector<double>.Build.Dense(YRad.Count);
        for (int i = 0; i < YRad.Count; i++)
        {
            var eLb = (-XLowerBound[i] * b).PointwiseExp().DotProduct(a);
            var eUb = (-XUpperBound[i] * b).PointwiseExp().DotProduct(a);
            var mixinRad = 0.5 * c.DotProduct(Vector.Build.Dense(MixinFunction.Select(f =>  Math.Abs(f(XUpperBound[i]) - f(XLowerBound[i]))).ToArray())); ;
            var mixinMid = 0.5 * c.DotProduct(Vector.Build.Dense(MixinFunction.Select(f =>  f(XUpperBound[i]) + f(XLowerBound[i])).ToArray())); ;
            result[i] = YRad[i] - 0.5 * (eLb - eUb) - mixinRad
                                - Math.Abs(YMid[i] - 0.5 * (eLb + eUb) - mixinMid);
        }
        return result;
    }
    
    public double TolValue(Vector<double> x0)
    {
        var a = x0.SubVector(0, PenatlyOption.ALb.Count);
        var b = x0.SubVector(PenatlyOption.ALb.Count, PenatlyOption.ALb.Count);
        var c = x0.SubVector(PenatlyOption.ALb.Count * 2, PenatlyOption.CLb.Count);
        return TolValue(a, b, c);
    }
    

    #region Penatly
    public double CalcPenatly(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        var lowerPenA = a - PenatlyOption.ALb - (a - PenatlyOption.ALb).PointwiseAbs();
        var upperPenA = PenatlyOption.AUb - a - (PenatlyOption.AUb - a).PointwiseAbs();
        var lowerPenB = b - PenatlyOption.BLb - (b - PenatlyOption.BLb).PointwiseAbs();
        var upperPenB = PenatlyOption.BUb - b - (PenatlyOption.BUb - b).PointwiseAbs();
        var lowerPenC = c - PenatlyOption.CLb - (c - PenatlyOption.CLb).PointwiseAbs();
        var upperPenC = PenatlyOption.CUb - c - (PenatlyOption.CUb - c).PointwiseAbs();
        return   PenatlyOption.CostA * (lowerPenA + upperPenA).Sum() 
               + PenatlyOption.CostB * (lowerPenB + upperPenB).Sum() 
               + PenatlyOption.CostC * (lowerPenC + upperPenC).Sum();
    }

    public Vector<double> GradAPenatly(Vector<double> a)
    {
        var lowerPenA = a - PenatlyOption.ALb - (a - PenatlyOption.ALb).PointwiseAbs();
        var upperPenA = PenatlyOption.AUb - a - (PenatlyOption.AUb - a).PointwiseAbs();
        return -PenatlyOption.CostA * (lowerPenA - upperPenA).PointwiseSign();
    }

    public Vector<double> GradBPenatly(Vector<double> b)
    {
        var lowerPenB = b - PenatlyOption.BLb - (b - PenatlyOption.BLb).PointwiseAbs();
        var upperPenB = PenatlyOption.BUb - b - (PenatlyOption.BUb - b).PointwiseAbs();
        return -PenatlyOption.CostB * (lowerPenB - upperPenB).PointwiseSign();
    }
    
    public Vector<double> GradCPenatly(Vector<double> c)
    {
        var lowerPenC = c - PenatlyOption.CLb - (c - PenatlyOption.CLb).PointwiseAbs();
        var upperPenC = PenatlyOption.CUb - c - (PenatlyOption.CUb - c).PointwiseAbs();
        return -PenatlyOption.CostC * (lowerPenC - upperPenC).PointwiseSign();
    }
    #endregion
    
    #region Gradient
    public Vector<double> Grad(Vector<double> x0)
    {
        return Grad(x0.SubVector(0, PenatlyOption.ALb.Count), x0.SubVector(PenatlyOption.ALb.Count, PenatlyOption.ALb.Count), x0.SubVector(PenatlyOption.ALb.Count * 2, PenatlyOption.CLb.Count));
    }
    
    public Vector<double> Grad(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        var result = Vector<double>.Build.Dense(a.Count + b.Count + c.Count);
        result.SetSubVector(0, PenatlyOption.ALb.Count, GradA(a, b, c));
        result.SetSubVector(PenatlyOption.ALb.Count, PenatlyOption.BLb.Count, GradB(a, b, c));
        result.SetSubVector(PenatlyOption.ALb.Count * 2, PenatlyOption.CLb.Count, GradC(a, b, c));
        return result;    
    }
    
    public Vector<double> GradA(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        var indexMin = CalcGeneratrix(a, b, c).MinimumIndex();
        var eLb = (-XLowerBound[indexMin] * b).PointwiseExp();
        var eUb = (-XUpperBound[indexMin] * b).PointwiseExp();
        var mixinMid = 0.5 * c.DotProduct(Vector.Build.Dense(MixinFunction.Select(f =>  f(XUpperBound[indexMin]) + f(XLowerBound[indexMin])).ToArray())); ;
        var grad = -0.5 * (eLb - eUb)
                   - 0.5 * (eLb + eUb)
                         * Math.Sign(0.5 * (eLb + eUb).DotProduct(a) + mixinMid - YMid[indexMin]);
        return grad + GradAPenatly(a);
    }
    public Vector<double> GradB(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        var indexMin = CalcGeneratrix(a, b, c).MinimumIndex();
        var eLb = (-XLowerBound[indexMin] * b).PointwiseExp();
        var eUb = (-XUpperBound[indexMin] * b).PointwiseExp();
        var mixinMid = 0.5 * c.DotProduct(Vector.Build.Dense(MixinFunction.Select(f =>  f(XUpperBound[indexMin]) + f(XLowerBound[indexMin])).ToArray())); ;
        var grad = 0.5 * (XLowerBound[indexMin] * eLb - XUpperBound[indexMin] * eUb).PointwiseMultiply(a)
                   + 0.5 * (XLowerBound[indexMin] * eLb + XUpperBound[indexMin] * eUb).PointwiseMultiply(a)
                         * Math.Sign(0.5 * (eLb + eUb).DotProduct(a) + mixinMid - YMid[indexMin]);
        return grad + GradBPenatly(b);
    }

    public Vector<double> GradC(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        var indexMin = CalcGeneratrix(a, b, c).MinimumIndex();
        var eLb = (-XLowerBound[indexMin] * b).PointwiseExp();
        var eUb = (-XUpperBound[indexMin] * b).PointwiseExp();
        var mixinRad = 0.5 * Vector.Build.Dense(MixinFunction.Select(f =>  
                        Math.Abs(f(XUpperBound[indexMin]) - f(XLowerBound[indexMin]))).ToArray()); ;
        var mixinMid = 0.5 * Vector.Build.Dense(MixinFunction.Select(f =>  
                        f(XUpperBound[indexMin]) + f(XLowerBound[indexMin])).ToArray()); ;
        var grad = mixinRad - mixinMid * Math.Sign(0.5 * (eLb + eUb).DotProduct(a) + mixinMid.DotProduct(c) - YMid[indexMin]) ;
        return grad + GradCPenatly(b);
    }
    #endregion
    
    public new OptimizationWithMixinResult Optimization(IUnconstrainedMinimizer minimizer, Vector<double> x0)
    {
        Func<Vector<double>, (double, Vector<double>)> functional = (x) => new(-TolValue(x), -Grad(x));
        var objective = ObjectiveFunction.Gradient(functional);
        var result = minimizer.FindMinimum(objective, x0);
        var a = result.MinimizingPoint.SubVector(0, PenatlyOption.ALb.Count);
        var b = result.MinimizingPoint.SubVector(PenatlyOption.ALb.Count, PenatlyOption.ALb.Count);
        var c = result.MinimizingPoint.SubVector(PenatlyOption.ALb.Count * 2, PenatlyOption.CLb.Count);

        return new OptimizationWithMixinResult()
        {
            TolValue = -result.FunctionInfoAtMinimum.Value,
            A = a,
            B = b,
            C = c,
            GradL2Norm = result.FunctionInfoAtMinimum.Gradient.L2Norm()
        };
    }
}
