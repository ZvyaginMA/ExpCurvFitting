using ExpCurvFitting.Application.TemplateHeadersGenerator;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ExpCurvFitting.Application.InputData
{
    public class InputData
    {
        private IReadOnlyList<Vector<double>> Data { get; set; }
        private InputDataConfiguration Configuration { get; set; }

        public InputData()
        {

        }

        public Vector<double> XLb;
            
        public Vector<double> XUb => Data[1];
        public Vector<double> YLb => Data[2];
        public Vector<double> YUb => Data[3];
    }

    public class InputDataMidRad
    {
        public InputDataMidRad(IEnumerable<double[]> data, InputDataConfiguration configuration)
        {
            _data = data.Select(x => Vector<double>.Build.DenseOfArray(x)).ToArray();
            _configuration = configuration;
        }

        private readonly IReadOnlyList<Vector<double>> _data;
        private readonly InputDataConfiguration _configuration;
        public bool IsIntervalInput => _configuration.IsIntervalInput; 
        public bool IsIntervalOutput => _configuration.IsIntervalOutput; 
        
        private Vector<double> MidRadToUb(Vector<double> mid, Vector<double> rad)
        {
            return mid + rad;
        }
        
        private Vector<double> MidRadToLb(Vector<double> mid, Vector<double> rad)
        {
            return mid - rad;
        }
        
        public Vector<double>[] XUb
        {
            get
            {
                var result = new List<Vector<double>>();
                if (IsIntervalInput)
                {
                    for (var i = 0; i < _configuration.CountInputVariable; i++)
                    {
                        result.Add(MidRadToUb(_data[i * 2],_data[i * 2 + 1] ));
                    }
                }
                else
                {
                    for (var i = 0; i < _configuration.CountInputVariable; i++)
                    {
                        result.Add(_data[i]);
                    }
                }
                return result.ToArray();
            }
        }

        public Vector<double>[] XLb 
        {
            get
            {
                var result = new List<Vector<double>>();
                if (IsIntervalInput)
                {
                    for (var i = 0; i < _configuration.CountInputVariable; i++)
                    {
                        result.Add(MidRadToLb(_data[i * 2],_data[i * 2 + 1] ));
                    }

                    return result.ToArray();
                }
                
                for (var i = 0; i < _configuration.CountInputVariable; i++)
                {
                    result.Add(_data[i]);
                }
                return result.ToArray();
            }
        }

        public Vector<double> YUb
        {
            get
            {
                if (IsIntervalOutput)
                {
                    return MidRadToUb(_data[^2], _data[^1]);
                }
                
                return _data[^1];
            }
        }
            
        public Vector<double> YLb
        {
            get
            {
                if (IsIntervalOutput)
                {
                    return MidRadToLb(_data[^2], _data[^1]);
                }
                
                return _data[^1];
            }
        }
        
        public Vector<double>[] XMid {
            get
            {
                var result = new List<Vector<double>>();
                if (IsIntervalInput)
                {
                    for (var i = 0; i < _configuration.CountInputVariable; i++)
                    {
                        result.Add(_data[i * 2]);
                    }
                }
                else
                {
                    for (var i = 0; i < _configuration.CountInputVariable; i++)
                    {
                        result.Add(_data[i]);
                    }
                }
                return result.ToArray();
            }
        }
        public Vector<double>[] XRad {
            get
            {
                var result = new List<Vector<double>>();
                if (IsIntervalInput)
                {
                    for (var i = 0; i < _configuration.CountInputVariable; i++)
                    {
                        result.Add(_data[i * 2 + 1]);
                    }
                }
                else
                {
                    for (var i = 0; i < _configuration.CountInputVariable; i++)
                    {
                        result.Add(Vector<double>.Build.Dense(_data.Count, 0));
                    }
                }
                return result.ToArray();
            }
        }
        public Vector<double> YMid{
            get
            {
                if (IsIntervalOutput)
                {
                    return _data[^2];
                }
                
                return _data[^1];
            }
        }
        public Vector<double> YRad{
            get
            {
                if (IsIntervalOutput)
                {
                    return _data[^1];
                }
                
                return Vector<double>.Build.Dense(_data.Count, 0);
            }
        }
    }

    public class InputDataBoundes
    {
        private InputDataConfiguration Configuration { get; set; }

    }
}
