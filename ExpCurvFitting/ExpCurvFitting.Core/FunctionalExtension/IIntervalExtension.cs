namespace ExpCurvFitting.Core.FunctionalExtension
{
    public interface IIntervalExtension
    {
        double Val(double t);
        double Mid(double lowerBound, double upperBound);
        double Rad(double lowerBound, double upperBound);
    }
}