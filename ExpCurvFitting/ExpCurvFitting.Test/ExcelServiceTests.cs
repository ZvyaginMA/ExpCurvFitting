using ExpCurvFitting.Application;
using FluentAssertions;

namespace ExpCurvFitting.Test
{
    public class ExcelServiceTests
    {
        //[Fact]
        public async Task Test1()
        {
            var excelService = new ExcelService();
            using var file = new FileStream("TestData/data.xlsx", FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            file.CopyToAsync(ms);
            var result = excelService.LoadDateFromFile(ms);
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }
    }
}
