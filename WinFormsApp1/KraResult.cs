namespace WinFormsApp1
{
    public class KraResult
    {
        public double epsilonSquared { get; set; }
        public double epsilonY { get; set; }
        public double avgApproximation { get; set; }
        public double unshiftedDispersion { get; set; }
        public double avgSquaredShift { get; set; }
        public List<double> paramsDispersion { get; set; }
        public List<double> elastics { get; set; }
        public List<double> studentCriterias { get; set; }
        public double rSquared { get; set; }
        public double fisher { get; set; }
    }
}
