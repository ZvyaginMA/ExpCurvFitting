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

        [Fact]
        public void Test2()
        {
            var command = new TemplateGenerator.Command
            {
                CountInputVariable = 1,
                IntervalPresentation = TemplateGenerator.IntervalPresentation.Bounds,
                IsIntervalInput = false,
                IsIntervalOutput = true,
            };

            var handler = new TemplateGenerator();

            var result = handler.Handle(command);

            result.Headers.Should().NotBeNullOrEmpty();
            result.Headers.Should().BeEquivalentTo(["x_1", "y_lb", "y_ub"]);
        }

        [Fact]
        public void Test3()
        {
            var command = new TemplateGenerator.Command
            {
                IntervalPresentation = (TemplateGenerator.IntervalPresentation)3,
            };

            var handler = new TemplateGenerator();

            var function = () => handler.Handle(command);
            function.Should().Throw<NotImplementedException>();
        }
    }
}
