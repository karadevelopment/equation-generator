using EG.Models;
using EG.Utils;
using System.Diagnostics;

namespace EG
{
    public class Program
    {
        private const int RESULT_LIMIT = 100;

        public static void Main()
        {
            var settings = new EquationGeneratorSettings();
            settings.Rules.Add(new EquationRule { Size = 1, Rule = -1 }); // 4-5
            settings.Rules.Add(new EquationRule { Size = 1, Rule = +1 }); // 5-4
            settings.Rules.Add(new EquationRule { Size = 1, Rule = -2 }); // 3-5
            settings.Rules.Add(new EquationRule { Size = 1, Rule = +2 }); // 5-3
            settings.Rules.Add(new EquationRule { Size = 1, Rule = -3 }); // 2-5
            settings.Rules.Add(new EquationRule { Size = 1, Rule = +3 }); // 5-2
            settings.Rules.Add(new EquationRule { Size = 1, Rule = -4 }); // 1-5
            settings.Rules.Add(new EquationRule { Size = 1, Rule = +4 }); // 5-1
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = -1 }); // 9-10
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = +1 }); // 10-9
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = -2 }); // 8-10
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = +2 }); // 10-8
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = -3 }); // 7-10
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = +3 }); // 10-7
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = -4 }); // 6-10
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = +4 }); // 10-6
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = -5 }); // 5-10
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = +5 }); // 10-5
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = -6 }); // 4-10
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = +6 }); // 10-4
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = -7 }); // 3-10
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = +7 }); // 10-3
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = -8 }); // 2-10
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = +8 }); // 10-2
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = -9 }); // 1-10
            //settings.Rules.Add(new EquationRule { Size = 2, Rule = +9 }); // 10-1
            settings.SumRangeMin = 0; // 0
            settings.SumRangeMax = 99; // 9/99/999
            settings.Numbers = 6; // 2-10
            settings.Digits = 1; // 1-3
            settings.Combine = false;
            settings.AllowDuplicate = false;
            settings.Shuffle = false;

            while (true)
            {
                var sw = Stopwatch.StartNew();
                var worker = new EquationWorker();
                _ = Task.Run(() => worker.Run(settings));
                do
                {
                    Console.Clear();
                    Console.WriteLine(worker.StateInfo);
                    Console.WriteLine(sw.Elapsed);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                while (worker.IsRunning);

                Console.Clear();
                if (worker.Error is null)
                {
                    foreach (var result in worker.Results.Take(Program.RESULT_LIMIT))
                    {
                        Console.WriteLine(string.Join("\t", result.Numbers));
                    }
                    Console.WriteLine($"{worker.Results.Count} GLEICHUNGEN GENERIERT IN {sw.Elapsed}");
                }
                else
                {
                    Console.WriteLine(worker.Error);
                }
                Console.ReadLine();
            }
        }
    }
}