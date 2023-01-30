using System.Globalization;
using System.IO;
using SharpDX;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace LogicSimulator.Infrastructure.YamlConverters;

public class Color4YamlConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(Color4);

    public object ReadYaml(IParser parser, Type type)
    {
        if (type != typeof(Color4))
            throw new InvalidDataException("Failed to retrieve Color4!");

        if (!parser.TryConsume<SequenceStart>(out _))
            throw new InvalidDataException("Invalid YAML content.");

        var vector = Color4.White;

        parser.TryConsume<Scalar>(out var scalarRed);
        vector.Red = (float)Convert.ToDouble(scalarRed!.Value);

        parser.TryConsume<Scalar>(out var scalarGreen);
        vector.Green = (float)Convert.ToDouble(scalarGreen!.Value);

        parser.TryConsume<Scalar>(out var scalarBlue);
        vector.Blue = (float)Convert.ToDouble(scalarBlue!.Value);

        parser.TryConsume<Scalar>(out var scalarAlpha);
        vector.Alpha = (float)Convert.ToDouble(scalarAlpha!.Value);

        if (!parser.TryConsume<SequenceEnd>(out _))
            throw new InvalidDataException("Invalid YAML content.");

        return vector;
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
        var vec = (Color4)value;

        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));

        emitter.Emit(new Scalar(null, vec.Red.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(null, vec.Green.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(null, vec.Blue.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(null, vec.Alpha.ToString(CultureInfo.InvariantCulture)));

        emitter.Emit(new SequenceEnd());
    }
}