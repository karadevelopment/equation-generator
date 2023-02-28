namespace EG.Models
{
    public class EquationGeneratorState
    {
        public DateTime StartTime { get; set; }
        public int CurrentIteration { get; set; }
        public string CurrentStack { get; set; }

        public EquationGeneratorState()
        {
            this.StartTime = DateTime.Now;
            this.CurrentIteration = 0;
            this.CurrentStack = string.Empty;
        }
    }
}