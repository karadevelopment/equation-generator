using EG.Models;

namespace EG.Utils
{
    public class EquationWorker
    {
        public bool IsRunning { get; private set; }
        private EquationGeneratorState State { get; }
        public List<EquationResult> Results { get; }
        public Exception? Error { get; private set; }

        public string StateInfo
        {
            get
            {
                var timeDiff = DateTime.Now - this.State.StartTime;
                var iterationsPerSecond = this.State.CurrentIteration / timeDiff.TotalSeconds;
                return string.Join("\n", new List<string>
                {
                    this.State.CurrentStack,
                    $"{this.State.CurrentIteration:0.##E+00} ITERATIONEN ({iterationsPerSecond:N0}/s)",
                    $"{this.Results.Count} GLEICHUNGEN GENERIERT",
                });
            }
        }

        public EquationWorker()
        {
            this.IsRunning = false;
            this.State = new();
            this.Results = new();
            this.Error = null;
        }

        public async Task Run(EquationGeneratorSettings settings)
        {
            if (this.IsRunning)
            {
                return;
            }

            this.IsRunning = true;
            this.State.StartTime = DateTime.Now;
            this.Results.Clear();
            this.Error = null;

            try
            {
                foreach (var equation in EquationGenerator.GetEquations(this.State,
                    settings.Rules,
                    settings.SumRangeMin,
                    settings.SumRangeMax,
                    settings.Numbers,
                    settings.Digits,
                    settings.Combine,
                    settings.AllowDuplicate,
                    settings.Shuffle,
                    settings.SumRangeMin + 1,
                    settings.SumRangeMax,
                    new List<int>()))
                {
                    if (this.IsRunning == false)
                    {
                        return;
                    }
                    if (equation != null)
                    {
                        this.Results.Add(equation);
                        await Task.Yield();
                    }
                }
            }
            catch (Exception e)
            {
                this.Error = e;
            }

            this.IsRunning = false;
        }
    }
}