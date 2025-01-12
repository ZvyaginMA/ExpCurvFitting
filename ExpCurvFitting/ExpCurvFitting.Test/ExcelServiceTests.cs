using ExpCurvFitting.Application.Excel;
using ExpCurvFitting.Application.TemplateHeadersGenerator;
using FluentAssertions;

namespace ExpCurvFitting.Test.Infrastructure
{
    public class ExcelServiceTests
    {
        //[Fact]
        public async Task Test2()
        {
            InputDataConfiguration inputDataConfiguration = new InputDataConfiguration()
            {
                CountInputVariable = 1,
                IntervalPresentation = IntervalPresentation.MidRad,
                IsIntervalOutput = true,
                IsIntervalInput = false
            };
            
            var excelService = new ExcelService();
            using var file = new FileStream("/TestData/data.xlsx", FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var result = excelService.LoadDateFromFile(ms, inputDataConfiguration);
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }
    }
}
