using ExpCurvFitting.Application.TemplateHeadersGenerator;
using MathNet.Numerics.LinearAlgebra;

namespace ExpCurvFitting.Application.InputData
{
    public class InputData
    {
        private IReadOnlyList<Vector<double>> Data { get; set; }
        private InputDataConfiguration Configuration { get; set; }

        public InputData()
        {

        }

        public Vector<double>[] XLb;
            
        public Vector<double> XUb => Data[1];
        public Vector<double> YLb => Data[2];
        public Vector<double> YUb => Data[3];
    }
}
