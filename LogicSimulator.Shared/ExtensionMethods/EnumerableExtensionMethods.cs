namespace LogicSimulator.Shared.ExtensionMethods;

public static class EnumerableExtensionMethods
{
    public static string ToStr<T>(this IEnumerable<T> enumerable) => $"[{string.Join(", ", enumerable)}]";

    public static string ToStr<T>(this IEnumerable<T> enumerable, Func<T, string> formatter) => $"[{string.Join(", ", enumerable.Select(formatter))}]";
}