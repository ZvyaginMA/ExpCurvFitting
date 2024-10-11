namespace ExpCurvFitting.Application.Excel
{
    public record Result
    {
        public bool IsSuccess { get; init; }
        public IReadOnlyList<double> X { get; init; }
        public IReadOnlyList<double> YMid { get; init; }
        public IReadOnlyList<double> YRad { get; init; }

        public int DataId = new Random().Next();
        public IEnumerable<(double, double, double)> GetPoints()
        {
            for (int i = 0; i < X.Count; i++)
            {
                yield return new(X[i], YMid[i], YRad[i]);
            }
        }
    }
}
