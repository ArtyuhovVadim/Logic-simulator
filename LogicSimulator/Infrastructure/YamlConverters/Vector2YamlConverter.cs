using System.Globalization;
using LogicSimulator.Utils;
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
            throw new YamlException("Wrong type.");

        parser.BeginSequenceOrThrow();

        var x = parser.ConsumeScalarAsDoubleOrThrow();
        var y = parser.ConsumeScalarAsDoubleOrThrow();

        parser.EndSequenceOrThrow();

        return new Vector2((float)x, (float)y);
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        if (type != typeof(Vector2))
            throw new YamlException("Wrong type.");

        var vec = (Vector2)value!;

        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));

        emitter.Emit(new Scalar(null, vec.X.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(null, vec.Y.ToString(CultureInfo.InvariantCulture)));

        emitter.Emit(new SequenceEnd());
    }
}