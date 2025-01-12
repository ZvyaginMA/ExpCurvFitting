
using MathNet.Numerics.LinearAlgebra;

namespace ExpCurvFitting.Application.Excel
{
    public record Result
    {
        public bool IsSuccess { get; init; }
        public InputData.InputDataMidRad InputData { get; init; }
        public int DataId = new Random().Next();
        
        public IEnumerable<double[]> GetPoints(double[]? generators = null)
        {
            if (generators is not null && generators.Length != InputData.XMid[0].Count)
            {
                throw new ArgumentException("generators must be of the same length");
            }
            
            if (InputData.IsIntervalInput)
            {
                for (int i = 0; i < InputData.XMid[0].Count; i++)
                {
                    var result = new List<double>();
                    for (int j = 0; j < InputData.XMid.Length; j++)              
                    {
                        result.Add(InputData.XMid[j][i]);
                        result.Add(InputData.XRad[j][i]);
                    }
                    
                    result.Add(InputData.YMid[i]);
                    result.Add(InputData.YRad[i]);

                    if (generators is not null)
                    {
                        result.Add(generators[i]);
                    }
                    
                    yield return result.ToArray();
                }
            }
            else
            {
                for (int i = 0; i < InputData.XMid[0].Count; i++)
                {
                    var result = new List<double>();
                    for (int j = 0; j < InputData.XMid.Length; j++)              
                    {
                        result.Add(InputData.XMid[j][i]);
                    }
                    
                    result.Add(InputData.YMid[i]);
                    result.Add(InputData.YRad[i]);
                    
                    if (generators is not null)
                    {
                        result.Add(generators[i]);
                    }
                    
                    yield return result.ToArray();
                }
            }
        }
    }
}
