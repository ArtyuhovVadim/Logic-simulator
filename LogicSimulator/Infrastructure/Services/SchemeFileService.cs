using System.IO;
using System.Text;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Infrastructure.YamlConverters;
using LogicSimulator.Models;
using LogicSimulator.Scene.SceneObjects;
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
        var color4Converter = new Color4YamlConverter();
        var lineConverter = new LineYamlConverter();

        var serializerBuilder = new SerializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .WithTypeConverter(lineConverter)
            .WithTagMapping(new TagName("!Rectangle"), typeof(Rectangle))
            .WithTagMapping(new TagName("!RoundedRectangle"), typeof(RoundedRectangle))
            .WithTagMapping(new TagName("!Ellipse"), typeof(Ellipse))
            .WithTagMapping(new TagName("!Line"), typeof(Line))
            .WithTagMapping(new TagName("!BezierCurve"), typeof(BezierCurve))
            .WithTagMapping(new TagName("!TextBlock"), typeof(TextBlock))
            .WithTagMapping(new TagName("!Arc"), typeof(Arc))
            .WithTagMapping(new TagName("!Image"), typeof(Image))
            ;

        var deserializerBuilder = new DeserializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .WithTypeConverter(lineConverter)
            .WithTagMapping(new TagName("!Rectangle"), typeof(Rectangle))
            .WithTagMapping(new TagName("!RoundedRectangle"), typeof(RoundedRectangle))
            .WithTagMapping(new TagName("!Ellipse"), typeof(Ellipse))
            .WithTagMapping(new TagName("!Line"), typeof(Line))
            .WithTagMapping(new TagName("!BezierCurve"), typeof(BezierCurve))
            .WithTagMapping(new TagName("!TextBlock"), typeof(TextBlock))
            .WithTagMapping(new TagName("!Arc"), typeof(Arc))
            .WithTagMapping(new TagName("!Image"), typeof(Image))
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

    public bool ReadFromFile(string path, out Scheme scheme)
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

            scheme = _deserializer.Deserialize<Scheme>(serializedScheme);

            return true;
        }
        catch
        {
            return false;
        }
    }
}