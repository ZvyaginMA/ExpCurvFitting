namespace ExpCurvFitting.Application.TemplateHeadersGenerator
{
    public class TemplateHeadersGenerator : ITemplateHeadersGenerator
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
            if (!command.dataConfiguration.IsIntervalOutput)
            {
                headers.Add("y");
                return;
            }

            if (command.dataConfiguration.IntervalPresentation == IntervalPresentation.Bounds)
            {
                headers.Add("y_lb");
                headers.Add("y_ub");
            }
            else if (command.dataConfiguration.IntervalPresentation == IntervalPresentation.MidRad)
            {
                headers.Add("y_mid");
                headers.Add("y_rad");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void AddInputHeaders(IList<string> headers, Command command)
        {
            if (!command.dataConfiguration.IsIntervalInput)
            {
                for (int i = 0; i < command.dataConfiguration.CountInputVariable; i++)
                {
                    headers.Add($"x_{i + 1}");
                }
                return;
            }
            
            if (command.dataConfiguration.IntervalPresentation == IntervalPresentation.Bounds)
            {
                for (int i = 0; i < command.dataConfiguration.CountInputVariable; i++)
                {
                    headers.Add($"x_{i + 1}_lb");
                    headers.Add($"x_{i + 1}_ub");
                }
                return;
            }
            
            if (command.dataConfiguration.IntervalPresentation == IntervalPresentation.MidRad)
            {
                for (int i = 0; i < command.dataConfiguration.CountInputVariable; i++)
                {
                    headers.Add($"x_{i + 1}_mid");
                    headers.Add($"x_{i + 1}_rad");
                }
                return;
            }
            
            throw new NotImplementedException();
        }
        public record Result
        {
            public IReadOnlyList<string> Headers { get; init; } = null!;
        }
    }
}
