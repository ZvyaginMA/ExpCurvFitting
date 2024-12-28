using ExpCurvFitting.Application.InputData;
using ExpCurvFitting.Application.TemplateHeadersGenerator;

namespace ExpCurvFitting.Application;

public class InputDataParser
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="data">Данные приходят строками </param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public InputDataMidRad Parse(InputDataConfiguration configuration, ICollection<double[]> dataInColums)
    {
        if (dataInColums is null)
        {
            throw new ArgumentNullException();
        }

        if (dataInColums.Count() != configuration.CountInputDataColumns())
        {
            throw new ArgumentException("Column number should be compliance configuration.");
        }

        return new InputDataMidRad(dataInColums, configuration);
    }
}