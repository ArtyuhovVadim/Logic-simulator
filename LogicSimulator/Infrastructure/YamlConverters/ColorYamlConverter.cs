using System.Globalization;
using System.Windows.Media;
using LogicSimulator.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace LogicSimulator.Infrastructure.YamlConverters;

public class ColorYamlConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(Color);

    public object ReadYaml(IParser parser, Type type)
    {
        if (type != typeof(Color))
            throw new YamlException("Wrong type.");

        parser.BeginSequenceOrThrow();

        var r = Convert.ToByte(parser.ConsumeScalarAsDoubleOrThrow() * 255);
        var g = Convert.ToByte(parser.ConsumeScalarAsDoubleOrThrow() * 255);
        var b = Convert.ToByte(parser.ConsumeScalarAsDoubleOrThrow() * 255);

        parser.EndSequenceOrThrow();

        return Color.FromRgb(r, g, b);
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        if (type != typeof(Color))
            throw new YamlException("Wrong type.");

        var color = ((Color)value!).ToColor4();

        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));

        emitter.Emit(new Scalar(null, color.Red.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(null, color.Green.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(null, color.Blue.ToString(CultureInfo.InvariantCulture)));

        emitter.Emit(new SequenceEnd());
    }
}