using ExpCurvFitting.Core.Models;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
namespace ExpCurvFitting.Web.Models
{
    public class PenatlyOptionsDto
    {
        public int NumberOfExp { get; init; }
        public IEnumerable<double> ALb => Items.Select(i => i.ALb);
        public IEnumerable<double> AUb => Items.Select(i => i.AUb);
        public IEnumerable<double> BLb => Items.Select(i => i.BLb);
        public IEnumerable<double> BUb => Items.Select(i => i.BUb);
        
        public IEnumerable<double> cLb => Items2.Select(i => i.CLb);
        public IEnumerable<double> cUb => Items2.Select(i => i.CUb);
        
        public double CostA { get; set; } = 100;
        public double CostB { get; set; } = 100;
        public int CountOfDegree = 0;
        
        public PenatlyOptionsDto(int numberOfExp, int countC = 1)
        {
            CountOfDegree = countC;
            NumberOfExp = numberOfExp;
            Items = new ItemAB[numberOfExp];
            for(int i = 0; i < numberOfExp; i++)
            {
                Items[i] = new ItemAB()
                {
                    ALb = 0,
                    AUb = 100,
                    BLb = 0,
                    BUb = 100,
                };
            }
            
            Items2 = new ItemC[countC];
            for(int i = 0; i < countC; i++)
            {
                Items2[i] = new ItemC()
                {
                    CLb = -100,
                    CUb = 100,
                };
            }
        }
        public ItemC[] Items2 { get; init; }
        public ItemAB[] Items { get; init; }
        public class ItemAB
        {
            public double ALb { get; set; }
            public double AUb { get; set; }
            public double BLb { get; set; }
            public double BUb { get; set; }
        }
        
        
        public class ItemC
        {
            public double CLb { get; set; }
            public double CUb { get; set; }
        }

        public PenatlyOptions GetPenatlyOptions()
        {
            return new PenatlyOptions()
            { 
                ALb = new DenseVector(ALb.ToArray()),
                AUb = new DenseVector(AUb.ToArray()),
                BLb = new DenseVector(BLb.ToArray()),
                BUb = new DenseVector(BUb.ToArray()),
                CostA = CostA,
                CostB = CostB,
            };
        }
        
        public PenatlyOptionsWithMixin GetPenatlyOptionsForMixin()
        {
            return new PenatlyOptionsWithMixin()
            { 
                ALb = new DenseVector(ALb.ToArray()),
                AUb = new DenseVector(AUb.ToArray()),
                BLb = new DenseVector(BLb.ToArray()),
                BUb = new DenseVector(BUb.ToArray()),
                CLb = new DenseVector(cLb.ToArray()),
                CUb = new DenseVector(cUb.ToArray()),
                CostC = CostA,
                CostA = CostA,
                CostB = CostB,
            };
        }
        
        public PenatlyOptionsWithMixin GetPenatlyOptionsForMixin(int numberMix)
        {
            return new PenatlyOptionsWithMixin()
            { 
                ALb = new DenseVector(ALb.ToArray()),
                AUb = new DenseVector(AUb.ToArray()),
                BLb = new DenseVector(BLb.ToArray()),
                BUb = new DenseVector(BUb.ToArray()),
                CLb = new DenseVector(cLb.ToArray()),
                CUb = new DenseVector(cUb.ToArray()),
                CostC = CostA,
                CostA = CostA,
                CostB = CostB,
            };
        }
    }
}
