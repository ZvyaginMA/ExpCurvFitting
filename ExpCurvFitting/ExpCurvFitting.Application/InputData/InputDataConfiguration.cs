namespace ExpCurvFitting.Application.TemplateHeadersGenerator
{
    public record InputDataConfiguration
    {
        public IntervalPresentation IntervalPresentation { get; init; } = IntervalPresentation.Bounds;
        public bool IsIntervalInput { get; init; } = true;
        public bool IsIntervalOutput { get; init; } = true;
        public int CountInputVariable { get; init; } = 1;
        
        public int CountInputDataColumns()
        {
            var countInputColumns = IsIntervalInput ? 2 *  CountInputVariable : CountInputVariable;
            var countOutputColumns = IsIntervalOutput ? 2 : 1;
            return countInputColumns + countOutputColumns;
        }
    }
}
