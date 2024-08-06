using OfficeOpenXml;
using System.Collections;

namespace ExpCurvFitting.Application
{
    public class ExcelService
    {
        public ExcelService() {

        }

        public Result LoadDateFromFile(Stream file)
        {
            double?[] x = null!;
            double?[] yMid = null!;
            double?[] yRad = null!;

            using (var package = new ExcelPackage(file))
            {
                var worksheet = package.Workbook.Worksheets[0];
                x = worksheet.Cells[2, 1, worksheet.Dimension.End.Row, 1].Select(cell => cell.Value as double?).ToArray();
                yMid = worksheet.Cells[2, 2, worksheet.Dimension.End.Row, 2].Select(cell => cell.Value as double?).ToArray();
                yRad = worksheet.Cells[2, 3, worksheet.Dimension.End.Row, 3].Select(cell => cell.Value as double?).ToArray();
            }
            if (x is not null
                && yMid is not null
                && yMid is not null)
            {
                return new Result
                {
                    IsSuccess = true,
                    X = x.Select(x => x.Value).ToArray(),
                    YMid = yMid.Select(x => x.Value).ToArray(),
                    YRad = yRad.Select(x => x.Value).ToArray(),
                };
            }
            else
            {
                return new Result
                {
                    IsSuccess = false,
                };
            }
        }

        public record Result
        {
            public bool IsSuccess { get; init; }
            public double[] X { get; init; }
            public double[] YMid { get; init; }
            public double[] YRad { get; init; }

            public IEnumerable<(double, double, double)> GetPoints()
            {
                for (int i = 0; i < X.Length; i++) {
                    yield return new(X[i], YMid[i], YRad[i]);
                }
            }
        }
    }
}
