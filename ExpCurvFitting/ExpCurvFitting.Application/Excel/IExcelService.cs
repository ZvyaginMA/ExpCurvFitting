using ExpCurvFitting.Application.TemplateHeadersGenerator;

namespace ExpCurvFitting.Application.Excel
{
    public interface IExcelService
    {
        Result LoadDateFromFile(Stream file, InputDataConfiguration inputDataConfiguration);
    }
}