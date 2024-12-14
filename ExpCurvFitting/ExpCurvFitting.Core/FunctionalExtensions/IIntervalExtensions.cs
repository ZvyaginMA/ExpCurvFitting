namespace ExpCurvFitting.Core.FunctionalExtensions
{
    public interface IIntervalExtensions
    {
        double Val(double t);
        double Mid(double lowerBound, double upperBound);
        double Rad(double lowerBound, double upperBound);
    }
}