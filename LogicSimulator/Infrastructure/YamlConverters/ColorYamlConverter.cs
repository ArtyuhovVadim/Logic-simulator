using System.Globalization;
using System.IO;
using LogicSimulator.Utils;
using SharpDX;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Infrastructure.YamlConverters;

public class ColorYamlConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(Color);

    public object ReadYaml(IParser parser, Type type)
    {
        if (type != typeof(Color))
            throw new InvalidDataException("Failed to retrieve Color4!");

        if (!parser.TryConsume<SequenceStart>(out _))
            throw new InvalidDataException("Invalid YAML content.");

        var color = Color4.White;

        parser.TryConsume<Scalar>(out var scalarRed);
        color.Red = (float)Convert.ToDouble(scalarRed!.Value);

        parser.TryConsume<Scalar>(out var scalarGreen);
        color.Green = (float)Convert.ToDouble(scalarGreen!.Value);

        parser.TryConsume<Scalar>(out var scalarBlue);
        color.Blue = (float)Convert.ToDouble(scalarBlue!.Value);

        parser.TryConsume<Scalar>(out var scalarAlpha);
        color.Alpha = (float)Convert.ToDouble(scalarAlpha!.Value);

        if (!parser.TryConsume<SequenceEnd>(out _))
            throw new InvalidDataException("Invalid YAML content.");
        
        return color.ToColor();
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        var color = ((Color)value!).ToColor4();

        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));

        emitter.Emit(new Scalar(null, color.Red.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(null, color.Green.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(null, color.Blue.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new Scalar(null, color.Alpha.ToString(CultureInfo.InvariantCulture)));

        emitter.Emit(new SequenceEnd());
    }
}