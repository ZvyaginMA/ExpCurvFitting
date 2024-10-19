namespace ExpCurvFitting.Application.TemplateGenerator
{
    public class TemplateGenerator : ITemplateGenerator
    {
        public enum IntervalPresentation
        {
            Bounds = 0,
            MidRad = 1,
        }

        public record Command
        {
            public IntervalPresentation IntervalPresentation { get; init; } = IntervalPresentation.Bounds;
            public bool IsIntervalInput { get; init; } = true;
            public bool IsIntervalOutput { get; init; } = true;
            public int CountInputVariable { get; init; } = 1;
        }

        public Result Handle(Command command)
        {
            var headers = new List<string>();

            for (int i = 0; i < command.CountInputVariable; i++)
            {
                if (command.IsIntervalInput)
                {
                    if (command.IntervalPresentation == IntervalPresentation.Bounds)
                    {
                        var firstHeader = $"x_{i + 1}_lb" ;
                        var secondHeader = $"x_{i + 1}_ub";
                        headers.Add(firstHeader);
                        headers.Add(secondHeader);
                    }
                    else if (command.IntervalPresentation == IntervalPresentation.MidRad)
                    {
                        var firstHeader = $"x_{i + 1}_mid";
                        var secondHeader = $"x_{i + 1}_rad";
                        headers.Add(firstHeader);
                        headers.Add(secondHeader);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    var xHeader = "x_" + (i + 1);
                    headers.Add(xHeader);
                }
            }

            if (command.IsIntervalOutput)
            {
                if (command.IntervalPresentation == IntervalPresentation.Bounds)
                {
                    var lowBoundHeader = "y_lb";
                    var upperBoundHeader = "y_ub";
                    headers.Add(lowBoundHeader);
                    headers.Add(upperBoundHeader);
                }
                else if (command.IntervalPresentation == IntervalPresentation.MidRad)
                {
                    var lowBoundHeader = "y_mid";
                    var upperBoundHeader = "y_rad";
                    headers.Add(lowBoundHeader);
                    headers.Add(upperBoundHeader);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                var yHeader = "y";
                headers.Add(yHeader);
            }

            return new Result
            {
                Headers = headers,
            };
        }

        public record Result
        {
            public IReadOnlyList<string> Headers { get; init; } = null!;
        }
    }
}
