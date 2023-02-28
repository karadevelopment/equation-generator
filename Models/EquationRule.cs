namespace EG.Models
{
    public class EquationRule
    {
        public int Size { get; set; }
        public int Rule { get; set; }

        public EquationRule()
        {
            this.Size = default;
            this.Rule = default;
        }
    }
}