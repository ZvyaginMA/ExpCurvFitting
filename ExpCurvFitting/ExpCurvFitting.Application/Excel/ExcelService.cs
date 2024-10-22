

using ExcelDataReader;
using System.Data;

namespace ExpCurvFitting.Application.Excel
{
    public class ExcelService : IExcelService
    {
        public Result LoadDateFromFile(Stream file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var excelReader = ExcelReaderFactory.CreateReader(file);
            var dataSet = excelReader.AsDataSet();


            var x = new List<double?>();
            var yMid = new List<double?>();
            var yRad = new List<double?>();

            for (int i = 1; i < dataSet.Tables[0].Rows.Count; i++)
            {
                DataRow row = dataSet.Tables[0].Rows[i];
                x.Add(row[0] as double?);
                yMid.Add(row[1] as double?);
                yRad.Add(row[2] as double?);
            }


            if (x.Any()
                && yMid.Any()
                && yRad.Any())
            {
                IList<double?> xx, yyMid, yyRad;
                RemoveNullRows(x, yMid, yRad, out xx, out yyMid, out yyRad);

                return new Result
                {
                    IsSuccess = true,
                    X = xx.Select(t => t!.Value).ToArray(),
                    YMid = yyMid.Select(t => t!.Value).ToArray(),
                    YRad = yyRad.Select(t => t!.Value).ToArray(),
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

        private static void RemoveNullRows(IList<double?> x, IList<double?> yMid, IList<double?> yRad, out IList<double?> newArray1, out IList<double?> newArray2, out IList<double?> newArray3)
        {
            var rowsToRemove = new List<int>();
            if(!(x.Count == yMid.Count && yMid.Count == yRad.Count))
            {
                throw new ArgumentException("Не совпадает количество входных данных");
            }

            for (int i = 0; i < x.Count; i++)
            {
                bool isNull1 = x[i] == null;
                bool isNull2 = yMid[i] == null;
                bool isNull3 = yRad[i] == null;

                if ((isNull1 && !isNull2) || (isNull1 && !isNull3) ||
                    (isNull2 && !isNull1) || (isNull2 && !isNull3) ||
                    (isNull3 && !isNull1) || (isNull3 && !isNull2))
                {
                    throw new InvalidOperationException($"Ошибка: строка {i} содержит null в одном массиве и не содержит в другом.");
                }

                if (isNull1 && isNull2 && isNull3)
                {
                    rowsToRemove.Add(i);
                }
            }

            newArray1 = x.Where((x, index) => !rowsToRemove.Contains(index)).ToList();
            newArray2 = yMid.Where((x, index) => !rowsToRemove.Contains(index)).ToList();
            newArray3 = yRad.Where((x, index) => !rowsToRemove.Contains(index)).ToList();
        }
    }
}
