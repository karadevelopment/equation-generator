namespace EG.Extensions
{
    public static class CollectionExtensions
    {
        private static Random RNG { get; } = new Random(DateTime.Now.Millisecond);

        public static void Shuffle<T>(this List<T> param)
        {
            for (var i = param.Count; 1 <= i; i--)
            {
                var i1 = i - 1;
                var i2 = CollectionExtensions.RNG.Next(1, i) - 1;
                (param[i1], param[i2]) = (param[i2], param[i1]);
            }
        }
    }
}