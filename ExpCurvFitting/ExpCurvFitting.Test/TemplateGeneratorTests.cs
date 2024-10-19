using ExpCurvFitting.Application.TemplateGenerator;
using FluentAssertions;
using System.Collections;

namespace ExpCurvFitting.Test
{
    public class TemplateGeneratorTests
    {
        [Fact]
        public void Test()
        {
            var command = new TemplateGenerator.Command
            {
                CountInputVariable = 2,
                IntervalPresentation = TemplateGenerator.IntervalPresentation.MidRad,
                IsIntervalInput = true,
                IsIntervalOutput = true,
            };

            var handler = new TemplateGenerator();

            var result = handler.Handle(command);

            result.Headers.Should().NotBeNullOrEmpty();
            result.Headers.Should().BeEquivalentTo([ "x_1_mid", "x_1_rad", "x_2_mid", "x_2_rad", "y_mid" , "y_rad"]);
        }
    }
}
