using ExpCurvFitting.Application;
using FluentAssertions;

namespace ExpCurvFitting.Test
{
    public class ExcelServiceTests
    {
        [Fact]
        public void Test1()
        {
            var excelService = new ExcelService();
            using var file = new FileStream("data.xlsx", FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();
            file.CopyToAsync(ms);
            var result = excelService.LoadDateFromFile(ms);
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
        }
    }
}
