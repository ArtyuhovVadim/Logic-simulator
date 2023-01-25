using System;
using System.Collections.Generic;
using System.IO;
using LogicSimulator.Scene;
using LogicSimulator.Scene.SceneObjects;
using SharpDX;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.Utilities;

namespace LogicSimulator.Infrastructure.YamlConverters;

public class LineYamlConverter : IYamlTypeConverter
{
    public IValueSerializer ValueSerializer { get; set; }

    public IValueDeserializer ValueDeserializer { get; set; }

    public bool Accepts(Type type) => type == typeof(Line);

    public object ReadYaml(IParser parser, Type type)
    {
        if (type != typeof(Line))
            throw new InvalidDataException("Failed to retrieve Line!");

        var line = new Line();

        parser.TryConsume<MappingStart>(out _);

        if (!parser.TryConsume<Scalar>(out var locationScalar) || locationScalar.Value != nameof(Line.Location))
            throw new YamlException(locationScalar!.Start, locationScalar.End, $"Expected a scalar named '{nameof(Line.Location)}'");

        line.Location = (Vector2)ValueDeserializer.DeserializeValue(parser, typeof(Vector2), new SerializerState(), ValueDeserializer)!;

        if (!parser.TryConsume<Scalar>(out var rotationScalar) || rotationScalar.Value != nameof(Line.Rotation))
            throw new YamlException(rotationScalar!.Start, rotationScalar.End, $"Expected a scalar named '{nameof(Line.Rotation)}'");

        line.Rotation = (Rotation)ValueDeserializer.DeserializeValue(parser, typeof(Rotation), new SerializerState(), ValueDeserializer)!;

        if (!parser.TryConsume<Scalar>(out var strokeThicknessScalar) || strokeThicknessScalar.Value != nameof(Line.StrokeThickness))
            throw new YamlException(strokeThicknessScalar!.Start, strokeThicknessScalar.End, $"Expected a scalar named '{nameof(Line.StrokeThickness)}'");

        line.StrokeThickness = (float)ValueDeserializer.DeserializeValue(parser, typeof(float), new SerializerState(), ValueDeserializer)!;

        if (!parser.TryConsume<Scalar>(out var strokeColorScalar) || strokeColorScalar.Value != nameof(Line.StrokeColor))
            throw new YamlException(strokeColorScalar!.Start, strokeColorScalar.End, $"Expected a scalar named '{nameof(Line.StrokeColor)}'");

        line.StrokeColor = (Color4)ValueDeserializer.DeserializeValue(parser, typeof(Color4), new SerializerState(), ValueDeserializer)!;

        if (!parser.TryConsume<Scalar>(out var vertexScalar) || vertexScalar.Value != nameof(Line.Vertexes))
            throw new YamlException(vertexScalar!.Start, vertexScalar.End, $"Expected a scalar named '{nameof(Line.Vertexes)}'");

        var vertexes = (IEnumerable<Vector2>)ValueDeserializer.DeserializeValue(parser, typeof(IEnumerable<Vector2>), new SerializerState(), ValueDeserializer);

        foreach (var vertex in vertexes!)
        {
            line.AddVertex(vertex);
        }

        parser.TryConsume<MappingEnd>(out _);

        return line;
    }

    public void WriteYaml(IEmitter emitter, object value, Type type)
    {
        var line = (Line)value;

        emitter.Emit(new MappingStart(AnchorName.Empty, new TagName("!Line"), false, MappingStyle.Block));

        ValueSerializer.SerializeValue(emitter, nameof(Line.Location), typeof(string));
        ValueSerializer.SerializeValue(emitter, line.Location, typeof(Vector2));

        ValueSerializer.SerializeValue(emitter, nameof(Line.Rotation), typeof(string));
        ValueSerializer.SerializeValue(emitter, line.Rotation, typeof(Rotation));

        ValueSerializer.SerializeValue(emitter, nameof(Line.StrokeThickness), typeof(string));
        ValueSerializer.SerializeValue(emitter, line.StrokeThickness, typeof(float));

        ValueSerializer.SerializeValue(emitter, nameof(Line.StrokeColor), typeof(string));
        ValueSerializer.SerializeValue(emitter, line.StrokeColor, typeof(Color4));

        ValueSerializer.SerializeValue(emitter, nameof(Line.Vertexes), typeof(string));

        emitter.Emit(new SequenceStart(AnchorName.Empty, TagName.Empty, false, SequenceStyle.Flow));

        foreach (var vertex in line.Vertexes)
        {
            ValueSerializer.SerializeValue(emitter, vertex, typeof(Vector2));
        }

        emitter.Emit(new SequenceEnd());

        emitter.Emit(new MappingEnd());
    }
}