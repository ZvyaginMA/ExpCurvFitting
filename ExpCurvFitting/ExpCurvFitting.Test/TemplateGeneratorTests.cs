using ExpCurvFitting.Application.TemplateHeadersGenerator;
using FluentAssertions;

namespace ExpCurvFitting.Test
{
    public class TemplateGeneratorTests
    {
        [Fact]
        public void Test()
        {
            var config = new InputDataConfiguration
            {
                CountInputVariable = 2,
                IntervalPresentation = IntervalPresentation.MidRad,
                IsIntervalInput = true,
                IsIntervalOutput = true,
            };

            var handler = new TemplateHeadersGenerator();

            var result = handler.Handle(new TemplateHeadersGenerator.Command
            {
                dataConfiguration = config
            });

            result.Headers.Should().NotBeNullOrEmpty();
            result.Headers.Should().BeEquivalentTo([ "x_1_mid", "x_1_rad", "x_2_mid", "x_2_rad", "y_mid" , "y_rad"]);
        }

        [Fact]
        public void Test2()
        {
            var config = new InputDataConfiguration
            {
                CountInputVariable = 1,
                IntervalPresentation = IntervalPresentation.Bounds,
                IsIntervalInput = false,
                IsIntervalOutput = true,
            };

            var handler = new TemplateHeadersGenerator();

            var result = handler.Handle(new TemplateHeadersGenerator.Command
            {
                dataConfiguration = config
            });

            result.Headers.Should().NotBeNullOrEmpty();
            result.Headers.Should().BeEquivalentTo(["x_1", "y_lb", "y_ub"]);
        }

        [Fact]
        public void Test3()
        {
            var config = new InputDataConfiguration
            {
                CountInputVariable = 1,
                IntervalPresentation = IntervalPresentation.Bounds,
                IsIntervalInput = true,
                IsIntervalOutput = true,
            };

            var handler = new TemplateHeadersGenerator();

            var result = handler.Handle(new TemplateHeadersGenerator.Command
            {
                dataConfiguration = config
            });

            result.Headers.Should().NotBeNullOrEmpty();
            result.Headers.Should().BeEquivalentTo(["x_1_lb","x_1_ub", "y_lb", "y_ub"]);
        }

        [Fact]
        public void Test4()
        {
            var config = new InputDataConfiguration
            {
                IntervalPresentation = (IntervalPresentation)3,
            };

            var handler = new TemplateHeadersGenerator();

            var function = () => handler.Handle(new TemplateHeadersGenerator.Command
            {
                dataConfiguration = config
            }); ;
            function.Should().Throw<NotImplementedException>();
        }
    }
}
