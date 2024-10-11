namespace ExpCurvFitting.Application.Excel
{
    public record Result
    {
        public bool IsSuccess { get; init; }
        public double[] X { get; init; }
        public double[] YMid { get; init; }
        public double[] YRad { get; init; }

        public int DataId = new Random().Next();
        public IEnumerable<(double, double, double)> GetPoints()
        {
            for (int i = 0; i < X.Length; i++)
            {
                yield return new(X[i], YMid[i], YRad[i]);
            }
        }
    }
}
