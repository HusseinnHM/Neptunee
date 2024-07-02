namespace System.Linq;

public static class NeptuneeEnumerableExtensions
{
    public static void ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
    {
        foreach (var e in source)
        {
            action(e);
        }
    }
}