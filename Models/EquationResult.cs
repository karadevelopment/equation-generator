namespace EG.Models
{
    public class EquationResult
    {
        public List<int> Numbers { get; set; }
        public EquationRule? Rule { get; set; }

        public EquationResult()
        {
            this.Numbers = new();
            this.Rule = null;
        }
    }
}