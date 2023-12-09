using System.IO;
using System.Text;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Infrastructure.YamlConverters;
using LogicSimulator.Models;
using LogicSimulator.ViewModels.ObjectViewModels;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace LogicSimulator.Infrastructure.Services;

public class SchemeFileService : ISchemeFileService
{
    private readonly ISerializer _serializer;
    private readonly IDeserializer _deserializer;

    private readonly FileStreamOptions _fileWriteStreamOptions;
    private readonly FileStreamOptions _fileReadStreamOptions;

    public SchemeFileService()
    {
        var vector2Converter = new Vector2YamlConverter();
        var color4Converter = new ColorYamlConverter();
        var lineConverter = new LineYamlConverter();

        var serializerBuilder = new SerializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .WithTypeConverter(lineConverter)
            .WithTagMapping(new TagName("!Rectangle"), typeof(RectangleViewModel))
            .WithTagMapping(new TagName("!RoundedRectangle"), typeof(RoundedRectangleViewModel))
            .WithTagMapping(new TagName("!Ellipse"), typeof(EllipseViewModel))
            .WithTagMapping(new TagName("!Line"), typeof(LineViewModel))
            .WithTagMapping(new TagName("!BezierCurve"), typeof(BezierCurveViewModel))
            .WithTagMapping(new TagName("!TextBlock"), typeof(TextBlockViewModel))
            .WithTagMapping(new TagName("!Arc"), typeof(ArcViewModel))
            .WithTagMapping(new TagName("!Image"), typeof(ImageViewModel))
            ;

        var deserializerBuilder = new DeserializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .WithTypeConverter(lineConverter)
            .WithTagMapping(new TagName("!Rectangle"), typeof(RectangleViewModel))
            .WithTagMapping(new TagName("!RoundedRectangle"), typeof(RoundedRectangleViewModel))
            .WithTagMapping(new TagName("!Ellipse"), typeof(EllipseViewModel))
            .WithTagMapping(new TagName("!Line"), typeof(LineViewModel))
            .WithTagMapping(new TagName("!BezierCurve"), typeof(BezierCurveViewModel))
            .WithTagMapping(new TagName("!TextBlock"), typeof(TextBlockViewModel))
            .WithTagMapping(new TagName("!Arc"), typeof(ArcViewModel))
            .WithTagMapping(new TagName("!Image"), typeof(ImageViewModel))
            ;


        lineConverter.ValueSerializer = serializerBuilder.BuildValueSerializer();
        lineConverter.ValueDeserializer = deserializerBuilder.BuildValueDeserializer();

        _serializer = serializerBuilder.Build();
        _deserializer = deserializerBuilder.Build();

        _fileWriteStreamOptions = new FileStreamOptions { Access = FileAccess.Write, Mode = FileMode.Create };
        _fileReadStreamOptions = new FileStreamOptions { Access = FileAccess.Read, Mode = FileMode.Open };
    }

    public bool SaveToFile(string path, Scheme scheme)
    {
        try
        {
            using var streamWriter = new StreamWriter(path, Encoding.Default, _fileWriteStreamOptions);

            var serializedScheme = _serializer.Serialize(scheme);

            streamWriter.Write(serializedScheme);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool ReadFromFile(string path, out Scheme? scheme)
    {
        scheme = null;

        if (!File.Exists(path))
        {
            return false;
        }

        try
        {
            using var streamReader = new StreamReader(path, Encoding.Default, false, _fileReadStreamOptions);

            var serializedScheme = streamReader.ReadToEnd();
            //TODO: scheme = _deserializer.Deserialize<Scheme>(streamReader);
            scheme = _deserializer.Deserialize<Scheme>(serializedScheme);

            return true;
        }
        catch
        {
            return false;
        }
    }
}