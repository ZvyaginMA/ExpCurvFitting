using ExpCurvFitting.Core.Models;
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
        
        public double cLb = 0;
        public double cUb = 100;
        
        public double CostA { get; set; } = 100;
        public double CostB { get; set; } = 100;

        public PenatlyOptionsDto(int numberOfExp)
        {
            NumberOfExp = numberOfExp;
            Items = new Item[numberOfExp];
            for(int i = 0; i < numberOfExp; i++)
            {
                Items[i] = new Item()
                {
                    ALb = 0,
                    AUb = 100,
                    BLb = 0,
                    BUb = 100,
                };
            }
        }

        public Item[] Items { get; init; }
        public class Item
        {
            public double ALb { get; set; }
            public double AUb { get; set; }
            public double BLb { get; set; }
            public double BUb { get; set; }
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
                CLb = new DenseVector([cLb]),
                CUb = new DenseVector([cUb]),
                CostC = CostA,
                CostA = CostA,
                CostB = CostB,
            };
        }
    }
}
