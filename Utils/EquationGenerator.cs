using EG.Extensions;
using EG.Models;

namespace EG.Utils
{
    public static class EquationGenerator
    {
        private static (int Min, int Max) GetRange(int rangeMin, int rangeMax, int value)
        {
            var min = rangeMin - value;
            var max = min + rangeMax;
            return (min, max);
        }

        private static bool ContainsRules(List<EquationRule> rules, int sumRangeMin, int sumRangeMax, bool allowDuplicate, List<int> sums, out EquationRule? result)
        {
            var rulesUsed = new List<EquationRule>();
            var rulesLeft = rules.ToList();
            result = null;

            foreach (var rule in rules)
            {
                for (var i = 1; i <= sums.Count - 1; i++)
                {
                    var sum1 = sums[i - 1];
                    var sum2 = sums[i + 0];
                    var inRange = sumRangeMin <= sum1 && sum1 <= sumRangeMax && sumRangeMin <= sum2 && sum2 <= sumRangeMax;

                    if (inRange == false)
                    {
                        continue;
                    }

                    var sumDiff = sum2 - sum1;
                    var sumDiffAbs = Math.Abs(sumDiff);
                    var sizeDelta = rule.Size == 1 ? 5 : 10;
                    var interval1 = (int)Math.Floor((double)sum1 / sizeDelta);
                    var interval2 = (int)Math.Floor((double)sum2 / sizeDelta);
                    var intervalDiff = Math.Abs(interval1 - interval2);
                    var interval1Min = interval1 * sizeDelta;
                    var interval1Max = interval1 * sizeDelta + sizeDelta - 1;
                    var interval2Min = interval2 * sizeDelta;
                    var interval2Max = interval2 * sizeDelta + sizeDelta - 1;

                    if (sumDiffAbs < sizeDelta && intervalDiff == 1)
                    {
                        if (interval1Min <= sum1 && sum1 <= interval1Max && interval1Max < sum2 || interval2Min <= sum2 && sum2 <= interval2Max && interval2Max < sum1)
                        {
                            var checkRulePermission = rules.Any(x => x.Rule == sumDiff);
                            var checkDuplicate = rulesUsed.Any(x => x.Rule == sumDiff) == false || allowDuplicate;

                            if (checkRulePermission && checkDuplicate)
                            {
                                if (rule.Rule == sumDiff)
                                {
                                    result = rule;
                                    rulesUsed.Add(rule);
                                    rulesLeft.Remove(rule);
                                }
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return rulesLeft.Any() == false;
        }

        public static IEnumerable<EquationResult> GetEquations(EquationGeneratorState state, List<EquationRule> rules, int sumRangeMin, int sumRangeMax, int numbers, int digits, bool combine, bool allowDuplicate, bool shuffle, int min, int max, List<int> stack)
        {
            var iteration = Enumerable.Range(min, max - min + 1)
                .Where(x => x != 0)
                .Select(x => (value: x, digits: $"{Math.Abs(x)}".Length))
                .Where(x => 1 <= x.digits && x.digits <= digits)
                .Select(x => x.value)
                .ToList();
            if (shuffle)
            {
                iteration.Shuffle();
            }
            foreach (var i in iteration)
            {
                var stackNew = stack.Append(i).ToList();
                var sum = stackNew.Sum();
                var (iMin, iMax) = EquationGenerator.GetRange(sumRangeMin, sumRangeMax, sum);

                if (numbers - 1 == 0)
                {
                    var sums = Enumerable.Range(1, stackNew.Count).Select(x => Enumerable.Range(1, x).Select(y => stackNew[y - 1]).Sum()).ToList();

                    if (combine)
                    {
                        if (EquationGenerator.ContainsRules(rules, sumRangeMin, sumRangeMax, allowDuplicate, sums, out var rule))
                        {
                            yield return new EquationResult { Numbers = stackNew, Rule = rule };
                        }
                    }
                    else
                    {
                        var rule = default(EquationRule);
                        if (rules.Any(x => EquationGenerator.ContainsRules(new List<EquationRule> { x }, sumRangeMin, sumRangeMax, allowDuplicate, sums, out rule)))
                        {
                            yield return new EquationResult { Numbers = stackNew, Rule = rule };
                        }
                    }

                    state.CurrentIteration++;
                    state.CurrentStack = string.Join(" ", stackNew);
                }
                if (0 < numbers - 1)
                {
                    foreach (var equation in EquationGenerator.GetEquations(state, rules, sumRangeMin, sumRangeMax, numbers - 1, digits, combine, allowDuplicate, shuffle, iMin, iMax, stackNew))
                    {
                        yield return equation;
                    }
                }
            }
        }
    }
}