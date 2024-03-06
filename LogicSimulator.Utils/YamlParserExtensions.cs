using System.Globalization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace LogicSimulator.Utils;

public static class YamlParserExtensions
{
    public static void BeginSequenceOrThrow(this IParser parser)
    {
        if (!parser.TryConsume<SequenceStart>(out _))
            throw new YamlException($"Invalid YAML at ${parser.Current?.Start.Line ?? -1} line.");
    }

    public static void EndSequenceOrThrow(this IParser parser)
    {
        if (!parser.TryConsume<SequenceEnd>(out _))
            throw new YamlException($"Invalid YAML at ${parser.Current?.Start.Line ?? -1} line.");
    }

    public static double ConsumeScalarAsDoubleOrThrow(this IParser parser)
    {
        if (!parser.TryConsume<Scalar>(out var scalar))
            throw new YamlException($"Invalid YAML at ${parser.Current?.Start.Line ?? -1} line.");

        return double.Parse(scalar.Value, CultureInfo.InvariantCulture);
    }

    public static string ConsumeScalarOrThrow(this IParser parser)
    {
        if (!parser.TryConsume<Scalar>(out var scalar))
            throw new YamlException($"Invalid YAML at ${parser.Current?.Start.Line ?? -1} line.");

        return scalar.Value;
    }
}