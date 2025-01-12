
using Prometheus;

namespace ExpCurvFitting.Web.Infrastructure;

public class AppMetric
{
    private readonly Counter _calcStartCount = Metrics.CreateCounter("calc_start_count", "", "sourse");
    
    private readonly Counter _calcDoneCount = Metrics.CreateCounter("calc_done_count", "", "sourse");
    
    private readonly Counter _calcExceptCount = Metrics.CreateCounter("calc_ex_count", "", "sourse");
    
    private readonly Counter _loadStartCount = Metrics.CreateCounter("load_start_count", "", "sourse");
    
    private readonly Counter _loadDoneCount = Metrics.CreateCounter("load_done_count", "", "sourse");
    
    private readonly Counter _loadExceptCount = Metrics.CreateCounter("load_ex_count", "", "sourse");


    public void IncCalcStart(string label)
    {
        _calcStartCount.WithLabels(label).Inc();
    }
    
    public void IncCalcDone(string label)
    {
        _calcDoneCount.WithLabels(label).Inc();
    }
    
    public void IncCalcEx(string label)
    {
        _calcExceptCount.WithLabels(label).Inc();
    }
    
    public void IncLoadStart(string label)
    {
        _loadStartCount.WithLabels(label).Inc();
    }
    
    public void IncLoadDone(string label)
    {
        _loadDoneCount.WithLabels(label).Inc();
    }
    
    public void IncLoadEx(string label)
    {
        _loadExceptCount.WithLabels(label).Inc();
    }
}