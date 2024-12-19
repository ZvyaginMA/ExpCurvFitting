namespace ExpCurvFitting.Application.TemplateHeadersGenerator
{
    public partial class TemplateHeadersGenerator : ITemplateHeadersGenerator
    {
        public record Command
        {
            public InputDataConfiguration dataConfiguration { get; init; }
        }

        public Result Handle(Command command)
        {
            var headers = new List<string>();
            AddInputHeaders(headers, command);
            AddOutputHeaders(headers, command);
            return new Result
            {
                Headers = headers,
            };
        }

        private void AddOutputHeaders(IList<string> headers, Command command)
        {
            if (command.dataConfiguration.IsIntervalOutput)
            {
                if (command.dataConfiguration.IntervalPresentation == IntervalPresentation.Bounds)
                {
                    var lowBoundHeader = "y_lb";
                    var upperBoundHeader = "y_ub";
                    headers.Add(lowBoundHeader);
                    headers.Add(upperBoundHeader);
                }
                else if (command.dataConfiguration.IntervalPresentation == IntervalPresentation.MidRad)
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
        }

        private void AddInputHeaders(IList<string> headers, Command command)
        {
            if (command.dataConfiguration.IsIntervalInput)
            {
                if (command.dataConfiguration.IntervalPresentation == IntervalPresentation.Bounds)
                {
                    for (int i = 0; i < command.dataConfiguration.CountInputVariable; i++)
                    {
                        var firstHeader = $"x_{i + 1}_lb";
                        var secondHeader = $"x_{i + 1}_ub";
                        headers.Add(firstHeader);
                        headers.Add(secondHeader);
                    }
                }
                else if (command.dataConfiguration.IntervalPresentation == IntervalPresentation.MidRad)
                {
                    for (int i = 0; i < command.dataConfiguration.CountInputVariable; i++)
                    {
                        var firstHeader = $"x_{i + 1}_mid";
                        var secondHeader = $"x_{i + 1}_rad";
                        headers.Add(firstHeader);
                        headers.Add(secondHeader);
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                for (int i = 0; i < command.dataConfiguration.CountInputVariable; i++)
                {
                    var xHeader = "x_" + (i + 1);
                    headers.Add(xHeader);
                }
            }
        }
        public record Result
        {
            public IReadOnlyList<string> Headers { get; init; } = null!;
        }
    }
}
