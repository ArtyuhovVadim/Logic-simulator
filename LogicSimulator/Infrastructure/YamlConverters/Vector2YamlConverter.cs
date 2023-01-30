using System.Globalization;
using System.IO;
using SharpDX;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace LogicSimulator.Infrastructure.YamlConverters;

public class Vector2YamlConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(Vector2);

    public object ReadYaml(IParser parser, Type type)
    {
        if (type != typeof(Vector2))
            throw new InvalidDataException("Failed to retrieve Vector2!");

        if (!parser.TryConsume<SequenceStart>(out var a))
            throw new InvalidDataException("Invalid YAML content.");

        var vector = Vector2.Zero;

        parser.TryConsume<Scalar>(out var scalarX);
        vector.X = (float)Convert.ToDouble(scalarX!.Value);

        parser.TryConsume<Scalar>(out var scalarY);
        vector.Y = (float)Convert.ToDouble(scalarY!.Value);

        if (!parser.TryConsume<SequenceEnd>(out _))
            throw new InvalidDataException("Invalid YAML content.");

        return vector;
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
        var vec = (Vector2)value;

        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));

        emitter.Emit(new Scalar(null, vec.X.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(null, vec.Y.ToString(CultureInfo.InvariantCulture)));

        emitter.Emit(new SequenceEnd());
    }
}