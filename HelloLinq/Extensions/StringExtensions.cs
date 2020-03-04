namespace HelloLinq.Extensions
{
    public static class StringExtensions
    {
        public static string TrimPad<T>(this T obj, int length)
            =>  ((obj?.ToString() ?? string.Empty) + new string(' ', length)).Substring(0, length);
    }
}
