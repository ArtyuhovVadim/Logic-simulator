using System.IO;
using System.Text;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Infrastructure.YamlConverters;
using LogicSimulator.Models;
using LogicSimulator.Models.Logic;
using LogicSimulator.Models.Logic.Gates;
using LogicSimulator.Models.Logic.Gates.Base;
using LogicSimulator.Models.Objects;
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
        var portModelConverter = new PortModelYamlConverter();

        var serializerBuilder = new SerializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .WithTypeConverter(versionConverter)
            .WithTypeConverter(portModelConverter)
            .WithTagMapping(new TagName("!Rectangle"), typeof(RectangleModel))
            .WithTagMapping(new TagName("!RoundedRectangle"), typeof(RoundedRectangleModel))
            .WithTagMapping(new TagName("!Ellipse"), typeof(EllipseModel))
            .WithTagMapping(new TagName("!Line"), typeof(LineModel))
            .WithTagMapping(new TagName("!BezierCurve"), typeof(BezierCurveModel))
            .WithTagMapping(new TagName("!TextBlock"), typeof(TextBlockModel))
            .WithTagMapping(new TagName("!Arc"), typeof(ArcModel))
            .WithTagMapping(new TagName("!Path"), typeof(PathModel))
            .WithTagMapping(new TagName("!InputGate"), typeof(InputGateModel))
            .WithTagMapping(new TagName("!OutputGate"), typeof(OutputGateModel))
            .WithTagMapping(new TagName("!AndGate"), typeof(AndGateModel))
            .WithTagMapping(new TagName("!Wire"), typeof(WireModel))
            ;

        var deserializerBuilder = new DeserializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .WithTypeConverter(versionConverter)
            .WithTypeConverter(portModelConverter)
            .WithTagMapping(new TagName("!Rectangle"), typeof(RectangleModel))
            .WithTagMapping(new TagName("!RoundedRectangle"), typeof(RoundedRectangleModel))
            .WithTagMapping(new TagName("!Ellipse"), typeof(EllipseModel))
            .WithTagMapping(new TagName("!Line"), typeof(LineModel))
            .WithTagMapping(new TagName("!BezierCurve"), typeof(BezierCurveModel))
            .WithTagMapping(new TagName("!TextBlock"), typeof(TextBlockModel))
            .WithTagMapping(new TagName("!Arc"), typeof(ArcModel))
            .WithTagMapping(new TagName("!Path"), typeof(PathModel))
            .WithTagMapping(new TagName("!InputGate"), typeof(InputGateModel))
            .WithTagMapping(new TagName("!OutputGate"), typeof(OutputGateModel))
            .WithTagMapping(new TagName("!AndGate"), typeof(AndGateModel))
            .WithTagMapping(new TagName("!Wire"), typeof(WireModel))
            ;

        _serializer = serializerBuilder.Build();
        _deserializer = deserializerBuilder.Build();

        _fileWriteStreamOptions = new FileStreamOptions { Access = FileAccess.Write, Mode = FileMode.Create };
        _fileReadStreamOptions = new FileStreamOptions { Access = FileAccess.Read, Mode = FileMode.Open };
    }

    public async Task SaveToFileAsync(string path, Scheme scheme)
    {
        await using var streamWriter = new StreamWriter(path, Encoding.Default, _fileWriteStreamOptions);
        scheme.Version = App.Version;
        var serializedScheme = _serializer.Serialize(scheme);
        await streamWriter.WriteAsync(serializedScheme);
    }

    public async Task<Scheme> ReadFromFileAsync(string path)
    {
        using var streamReader = new StreamReader(path, Encoding.Default, false, _fileReadStreamOptions);
        var scheme = _deserializer.Deserialize<Scheme>(await streamReader.ReadToEndAsync());
        scheme.FileInfo = new FileInfo(path);

        foreach (var gate in scheme.Objects.OfType<BaseGateModel>())
        {
            foreach (var port in gate.Ports)
            {
                port.Parent = gate;
            }
        }

        if (scheme.Version > App.Version)
            throw new InvalidOperationException($"Can not load scheme of {scheme.Version} version.");

        return scheme;
    }
}