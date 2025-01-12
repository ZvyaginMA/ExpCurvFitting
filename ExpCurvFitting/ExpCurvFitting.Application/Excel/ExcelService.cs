

using ExcelDataReader;
using System.Data;
using ExpCurvFitting.Application.TemplateHeadersGenerator;

namespace ExpCurvFitting.Application.Excel
{
    public class ExcelService : IExcelService
    {
        public Result LoadDateFromFile(Stream file, InputDataConfiguration inputDataConfiguration)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using var excelReader = ExcelReaderFactory.CreateReader(file);
            var dataSet = excelReader.AsDataSet();

            var dataArrays = GetDataInArray(dataSet);

            var parser = new InputDataParser();
            var inputData = parser.Parse(inputDataConfiguration, dataArrays);
            
            return new Result
            {
                IsSuccess = true,
                InputData = inputData,
            };
        }

        private static double[][] GetDataInArray(DataSet dataSet)
        {
            int rowCount = dataSet.Tables[0].Rows.Count;
            int columnCount = dataSet.Tables[0].Columns.Count;

            // Создаем массивы для хранения данных
            double[][] dataArrays = new double[columnCount][];

            // Инициализируем массивы
            for (int i = 0; i < columnCount; i++)
            {
                dataArrays[i] = new double[rowCount - 1]; // -1 для исключения заголовка
            }

            // Заполняем массивы данными, пропуская заголовки
            for (int i = 1; i < rowCount; i++) // Начинаем с 1, чтобы пропустить заголовок
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (double.TryParse(dataSet.Tables[0].Rows[i][j].ToString(), out double value))
                    {
                        dataArrays[j][i - 1] = value; // Заполняем массив
                    }
                    else
                    {
                        // Обработка случая, если значение не может быть преобразовано в double
                        throw new ArgumentException($"invalid data {dataSet.Tables[0].Rows[i][j]}");
                    }
                }
            }

            return dataArrays;
        }
    }
}
