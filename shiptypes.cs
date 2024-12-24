namespace EffortCalculator
{
    public class ShipType
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string FormulaLow { get; set; } = string.Empty;
        public string FormulaHigh { get; set; } = string.Empty;
        public double MaxDisplacement { get; set; }
    }
}
