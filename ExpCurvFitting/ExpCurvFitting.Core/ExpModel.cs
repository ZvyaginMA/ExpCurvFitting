using MathNet.Numerics.LinearAlgebra;
namespace ExpCurvFitting.Core;
public record ExpModel
{
    public int QuantityExp { get; init; }
    public ExpModel(int quantityExp)
    {
        QuantityExp = quantityExp;
    }

    public void Fit()
    {

    }

    public void Pridict(Vector<double> xLb, Vector<double> xUb, Vector<double> yLb, Vector<double> yUb)
    {
        
    }
}
