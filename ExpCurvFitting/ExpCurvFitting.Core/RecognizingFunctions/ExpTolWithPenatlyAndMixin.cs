using ExpCurvFitting.Core.FunctionalExtension;
using ExpCurvFitting.Core.Models;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Optimization;

namespace ExpCurvFitting.Core.RecognizingFunctions;
public record ExpTolWithPenatlyAndMixin : ExpTol
{
    public IList<IIntervalExtension> MixinFunction { get; init; }
    public PenatlyOptionsWithMixin PenatlyOptions { get; init; }
    public ExpTolWithPenatlyAndMixin(
        Vector<double> xLowerBound, 
        Vector<double> xUpperBound, 
        Vector<double> yLowerBound, 
        Vector<double> yUpperBound, 
        PenatlyOptionsWithMixin penatlyOptions, 
        IList<IIntervalExtension> mixinFunctions) : base(xLowerBound, xUpperBound, yLowerBound, yUpperBound)
    {
        PenatlyOptions = penatlyOptions;
        MixinFunction = mixinFunctions;
    }
    
    public double TolValue(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        return CalcGeneratrix(a, b, c).Min() + CalcPenatly(a, b, c);
    }

    public Vector<double> Pridict(Vector<double> t, Vector<double> a, Vector<double> b, Vector<double> c)
    {
        var result = Vector.Build.Dense(t.Count);
        for (int i = 0; i < t.Count; i++)
        {
            result[i] = a.DotProduct((-t[i] * b).PointwiseExp()) + c.DotProduct(Vector.Build.Dense(MixinFunction.Select(f => f.Val(t[i])).ToArray()));
        }
        return result;
    }

    public double CalcRmse(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        var yPredict = Pridict(XMid, a, b, c);
        return Math.Pow((yPredict - YMid).DotProduct(yPredict - YMid) / YMid.Count, 0.5);
    }

    public override double CalcRmse(Vector<double> x0)
    {
        var a = x0.SubVector(0, PenatlyOptions.ALb.Count);
        var b = x0.SubVector(PenatlyOptions.ALb.Count, PenatlyOptions.ALb.Count);
        var c = x0.SubVector(PenatlyOptions.ALb.Count * 2, PenatlyOptions.CLb.Count);
        return CalcRmse(a, b, c);
    }

    private Vector<double> CalcGeneratrix(Vector<double> a, Vector<double> b,  Vector<double> c)
    {
        var result = Vector<double>.Build.Dense(YRad.Count);
        for (int i = 0; i < YRad.Count; i++)
        {
            var eLb = (-XLowerBound[i] * b).PointwiseExp().DotProduct(a);
            var eUb = (-XUpperBound[i] * b).PointwiseExp().DotProduct(a);
            var mixinRad = c.PointwiseAbs().DotProduct(Vector.Build.Dense(MixinFunction.Select(f => f.Rad(XLowerBound[i], XUpperBound[i])).ToArray())); ;
            var mixinMid = c.DotProduct(Vector.Build.Dense(MixinFunction.Select(f => f.Mid(XLowerBound[i], XUpperBound[i])).ToArray())); ;
            result[i] = YRad[i] - 0.5 * (eLb - eUb) - mixinRad
                                - Math.Abs(YMid[i] - 0.5 * (eLb + eUb) - mixinMid);
        }
        return result;
    }
    
    public override double TolValue(Vector<double> x0)
    {
        var a = x0.SubVector(0, PenatlyOptions.ALb.Count);
        var b = x0.SubVector(PenatlyOptions.ALb.Count, PenatlyOptions.ALb.Count);
        var c = x0.SubVector(PenatlyOptions.ALb.Count * 2, PenatlyOptions.CLb.Count);
        return TolValue(a, b, c);
    }

    #region Penatly
    public double CalcPenatly(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        var lowerPenA = a - PenatlyOptions.ALb - (a - PenatlyOptions.ALb).PointwiseAbs();
        var upperPenA = PenatlyOptions.AUb - a - (PenatlyOptions.AUb - a).PointwiseAbs();
        var lowerPenB = b - PenatlyOptions.BLb - (b - PenatlyOptions.BLb).PointwiseAbs();
        var upperPenB = PenatlyOptions.BUb - b - (PenatlyOptions.BUb - b).PointwiseAbs();
        var lowerPenC = c - PenatlyOptions.CLb - (c - PenatlyOptions.CLb).PointwiseAbs();
        var upperPenC = PenatlyOptions.CUb - c - (PenatlyOptions.CUb - c).PointwiseAbs();
        return   PenatlyOptions.CostA * (lowerPenA + upperPenA).Sum() 
               + PenatlyOptions.CostB * (lowerPenB + upperPenB).Sum() 
               + PenatlyOptions.CostC * (lowerPenC + upperPenC).Sum();
    }

    public Vector<double> GradAPenatly(Vector<double> a)
    {
        var lowerPenA = a - PenatlyOptions.ALb - (a - PenatlyOptions.ALb).PointwiseAbs();
        var upperPenA = PenatlyOptions.AUb - a - (PenatlyOptions.AUb - a).PointwiseAbs();
        return -PenatlyOptions.CostA * (lowerPenA - upperPenA).PointwiseSign();
    }

    public Vector<double> GradBPenatly(Vector<double> b)
    {
        var lowerPenB = b - PenatlyOptions.BLb - (b - PenatlyOptions.BLb).PointwiseAbs();
        var upperPenB = PenatlyOptions.BUb - b - (PenatlyOptions.BUb - b).PointwiseAbs();
        return -PenatlyOptions.CostB * (lowerPenB - upperPenB).PointwiseSign();
    }
    
    public Vector<double> GradCPenatly(Vector<double> c)
    {
        var lowerPenC = c - PenatlyOptions.CLb - (c - PenatlyOptions.CLb).PointwiseAbs();
        var upperPenC = PenatlyOptions.CUb - c - (PenatlyOptions.CUb - c).PointwiseAbs();
        return -PenatlyOptions.CostC * (lowerPenC - upperPenC).PointwiseSign();
    }
    #endregion
    
    #region Gradient
    public override Vector<double> Grad(Vector<double> x0)
    {
        return Grad(x0.SubVector(0, PenatlyOptions.ALb.Count), x0.SubVector(PenatlyOptions.ALb.Count, PenatlyOptions.ALb.Count), x0.SubVector(PenatlyOptions.ALb.Count * 2, PenatlyOptions.CLb.Count));
    }
    
    public Vector<double> Grad(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        var result = Vector<double>.Build.Dense(a.Count + b.Count + c.Count);
        result.SetSubVector(0, PenatlyOptions.ALb.Count, GradA(a, b, c));
        result.SetSubVector(PenatlyOptions.ALb.Count, PenatlyOptions.BLb.Count, GradB(a, b, c));
        result.SetSubVector(PenatlyOptions.ALb.Count * 2, PenatlyOptions.CLb.Count, GradC(a, b, c));
        return result;    
    }
    
    public Vector<double> GradA(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        var indexMin = CalcGeneratrix(a, b, c).MinimumIndex();
        var eLb = (-XLowerBound[indexMin] * b).PointwiseExp();
        var eUb = (-XUpperBound[indexMin] * b).PointwiseExp();
        var mixinMid = c.DotProduct(Vector.Build.Dense(MixinFunction.Select(f => f.Mid(XLowerBound[indexMin], XUpperBound[indexMin])).ToArray())); ;
        var grad = -0.5 * (eLb - eUb)
                   - 0.5 * (eLb + eUb)
                         * Math.Sign(0.5 * (eLb + eUb).DotProduct(a) + mixinMid - YMid[indexMin]);
        var bv = GradAPenatly(a);
        return grad + bv;
    }
    public Vector<double> GradB(Vector<double> a, Vector<double> b, Vector<double> c)
    {
        var indexMin = CalcGeneratrix(a, b, c).MinimumIndex();
        var eLb = (-XLowerBound[indexMin] * b).PointwiseExp();
        var eUb = (-XUpperBound[indexMin] * b).PointwiseExp();
        var mixinMid = c.DotProduct(Vector.Build.Dense(MixinFunction.Select(f => f.Mid(XLowerBound[indexMin], XUpperBound[indexMin])).ToArray())); ;
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
        var mixinRad = Vector.Build.Dense(MixinFunction.Select(f => f.Rad(XLowerBound[indexMin], XUpperBound[indexMin])).ToArray()); ;
        var mixinMid = Vector.Build.Dense(MixinFunction.Select(f => f.Mid(XLowerBound[indexMin], XUpperBound[indexMin])).ToArray()); ;
        var grad = c.PointwiseSign().DotProduct(mixinRad) - mixinMid * Math.Sign(0.5 * (eLb + eUb).DotProduct(a) + mixinMid.DotProduct(c) - YMid[indexMin]) ;
        return grad + GradCPenatly(c);
    }
    #endregion
}
