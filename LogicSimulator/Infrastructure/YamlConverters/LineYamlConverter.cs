using System.IO;
using LogicSimulator.ViewModels.ObjectViewModels;
using SharpDX;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Utilities;
using Color = System.Windows.Media.Color;

namespace LogicSimulator.Infrastructure.YamlConverters;

public class LineYamlConverter : IYamlTypeConverter
{
    public IValueSerializer ValueSerializer { get; set; } = null!;

    public IValueDeserializer ValueDeserializer { get; set; } = null!;

    public bool Accepts(Type type) => type == typeof(LineViewModel);

    public object ReadYaml(IParser parser, Type type)
    {
        if (type != typeof(LineViewModel))
            throw new InvalidDataException("Failed to retrieve Line!");

        var line = new LineViewModel();

        parser.TryConsume<MappingStart>(out _);

        if (!parser.TryConsume<Scalar>(out var locationScalar) || locationScalar.Value != nameof(LineViewModel.Location))
            throw new YamlException(locationScalar!.Start, locationScalar.End, $"Expected a scalar named '{nameof(LineViewModel.Location)}'");

        line.Location = (Vector2)ValueDeserializer.DeserializeValue(parser, typeof(Vector2), new SerializerState(), ValueDeserializer)!;

        if (!parser.TryConsume<Scalar>(out var rotationScalar) || rotationScalar.Value != nameof(LineViewModel.Rotation))
            throw new YamlException(rotationScalar!.Start, rotationScalar.End, $"Expected a scalar named '{nameof(LineViewModel.Rotation)}'");

        line.Rotation = (Rotation)ValueDeserializer.DeserializeValue(parser, typeof(Rotation), new SerializerState(), ValueDeserializer)!;

        if (!parser.TryConsume<Scalar>(out var strokeThicknessScalar) || strokeThicknessScalar.Value != nameof(LineViewModel.StrokeThickness))
            throw new YamlException(strokeThicknessScalar!.Start, strokeThicknessScalar.End, $"Expected a scalar named '{nameof(LineViewModel.StrokeThickness)}'");

        line.StrokeThickness = (float)ValueDeserializer.DeserializeValue(parser, typeof(float), new SerializerState(), ValueDeserializer)!;

        if (!parser.TryConsume<Scalar>(out var strokeColorScalar) || strokeColorScalar.Value != nameof(LineViewModel.StrokeColor))
            throw new YamlException(strokeColorScalar!.Start, strokeColorScalar.End, $"Expected a scalar named '{nameof(LineViewModel.StrokeColor)}'");

        line.StrokeColor = (Color)ValueDeserializer.DeserializeValue(parser, typeof(Color), new SerializerState(), ValueDeserializer)!;

        if (!parser.TryConsume<Scalar>(out var vertexScalar) || vertexScalar.Value != nameof(LineViewModel.Vertexes))
            throw new YamlException(vertexScalar!.Start, vertexScalar.End, $"Expected a scalar named '{nameof(LineViewModel.Vertexes)}'");

        var vertexes = (IEnumerable<Vector2>)ValueDeserializer.DeserializeValue(parser, typeof(IEnumerable<Vector2>), new SerializerState(), ValueDeserializer)!;

        line.Vertexes = new ObservableCollection<Vector2>(vertexes);

        parser.TryConsume<MappingEnd>(out _);

        return line;
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        var line = (LineViewModel)value!;

        emitter.Emit(new MappingStart(AnchorName.Empty, new TagName("!Line"), false, MappingStyle.Block));

        ValueSerializer.SerializeValue(emitter, nameof(LineViewModel.Location), typeof(string));
        ValueSerializer.SerializeValue(emitter, line.Location, typeof(Vector2));

        ValueSerializer.SerializeValue(emitter, nameof(LineViewModel.Rotation), typeof(string));
        ValueSerializer.SerializeValue(emitter, line.Rotation, typeof(Rotation));

        ValueSerializer.SerializeValue(emitter, nameof(LineViewModel.StrokeThickness), typeof(string));
        ValueSerializer.SerializeValue(emitter, line.StrokeThickness, typeof(float));

        ValueSerializer.SerializeValue(emitter, nameof(LineViewModel.StrokeColor), typeof(string));
        ValueSerializer.SerializeValue(emitter, line.StrokeColor, typeof(Color));

        ValueSerializer.SerializeValue(emitter, nameof(LineViewModel.Vertexes), typeof(string));

        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));

        foreach (var vertex in line.Vertexes)
        {
            ValueSerializer.SerializeValue(emitter, vertex, typeof(Vector2));
        }

        emitter.Emit(new SequenceEnd());

        emitter.Emit(new MappingEnd());
    }
}