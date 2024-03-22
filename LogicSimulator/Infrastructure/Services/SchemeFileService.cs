using System.IO;
using System.Text;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Infrastructure.YamlConverters;
using LogicSimulator.Models;
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
        var versionConverter = new VersionYamlConverter();

        var serializerBuilder = new SerializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .WithTypeConverter(versionConverter)
            .WithTagMapping(new TagName("!Rectangle"), typeof(RectangleModel))
            .WithTagMapping(new TagName("!RoundedRectangle"), typeof(RoundedRectangleModel))
            .WithTagMapping(new TagName("!Ellipse"), typeof(EllipseModel))
            .WithTagMapping(new TagName("!Line"), typeof(LineModel))
            .WithTagMapping(new TagName("!BezierCurve"), typeof(BezierCurveModel))
            .WithTagMapping(new TagName("!TextBlock"), typeof(TextBlockModel))
            .WithTagMapping(new TagName("!Arc"), typeof(ArcModel))
            .WithTagMapping(new TagName("!AndGate"), typeof(AndGateModel))
            ;

        var deserializerBuilder = new DeserializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .WithTypeConverter(versionConverter)
            .WithTagMapping(new TagName("!Rectangle"), typeof(RectangleModel))
            .WithTagMapping(new TagName("!RoundedRectangle"), typeof(RoundedRectangleModel))
            .WithTagMapping(new TagName("!Ellipse"), typeof(EllipseModel))
            .WithTagMapping(new TagName("!Line"), typeof(LineModel))
            .WithTagMapping(new TagName("!BezierCurve"), typeof(BezierCurveModel))
            .WithTagMapping(new TagName("!TextBlock"), typeof(TextBlockModel))
            .WithTagMapping(new TagName("!Arc"), typeof(ArcModel))
            .WithTagMapping(new TagName("!AndGate"), typeof(AndGateModel))
            ;

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

            scheme.Version = App.Version;
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
            return false;

        try
        {
            using var streamReader = new StreamReader(path, Encoding.Default, false, _fileReadStreamOptions);
            scheme = _deserializer.Deserialize<Scheme>(streamReader);
            scheme.FileInfo = new FileInfo(path);

            if (scheme.Version > App.Version)
                throw new InvalidOperationException($"Can not load scheme of {scheme.Version} version.");

            return true;
        }
        catch
        {
            return false;
        }
    }
}