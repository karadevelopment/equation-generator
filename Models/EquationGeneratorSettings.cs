namespace EG.Models
{
    public class EquationGeneratorSettings
    {
        public List<EquationRule> Rules { get; set; }
        public int SumRangeMin { get; set; }
        public int SumRangeMax { get; set; }
        public int Numbers { get; set; }
        public int Digits { get; set; }
        public bool Combine { get; set; }
        public bool AllowDuplicate { get; set; }
        public bool Shuffle { get; set; }

        public EquationGeneratorSettings()
        {
            this.Rules = new();
            this.SumRangeMin = default;
            this.SumRangeMax = default;
            this.Numbers = default;
            this.Digits = default;
            this.Combine = default;
            this.AllowDuplicate = default;
            this.Shuffle = default;
        }
    }
}