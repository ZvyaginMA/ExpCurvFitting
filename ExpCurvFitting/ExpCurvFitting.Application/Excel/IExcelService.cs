namespace ExpCurvFitting.Application.Excel
{
    public interface IExcelService
    {
        Result LoadDateFromFile(Stream file);
    }
}