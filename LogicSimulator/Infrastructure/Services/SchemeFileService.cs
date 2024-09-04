using System.IO;
using System.Reflection;
using System.Text;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Infrastructure.YamlConverters;
using LogicSimulator.Models;
using LogicSimulator.Models.Logic.Gates.Base;
using LogicSimulator.Models.Objects.Base;
using Microsoft.Extensions.Logging;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace LogicSimulator.Infrastructure.Services;

public class SchemeFileService : ISchemeFileService
{
    private readonly ILogger<SchemeFileService> _logger;
    private readonly ISerializer _serializer;
    private readonly IDeserializer _deserializer;

    private readonly FileStreamOptions _fileWriteStreamOptions;
    private readonly FileStreamOptions _fileReadStreamOptions;

    public SchemeFileService(ILogger<SchemeFileService> logger)
    {
        _logger = logger;

        var vector2Converter = new Vector2YamlConverter();
        var color4Converter = new ColorYamlConverter();
        var versionConverter = new VersionYamlConverter();
        var portModelConverter = new PortModelYamlConverter();

        var schemeObjectTypes = Assembly.GetEntryAssembly()!.DefinedTypes.Where(type => type.IsSubclassOf(typeof(BaseObjectModel)) && !type.IsAbstract).ToArray();

        var serializerBuilder = new SerializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .WithTypeConverter(versionConverter)
            .WithTypeConverter(portModelConverter)
            ;

        var deserializerBuilder = new DeserializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .WithTypeConverter(versionConverter)
            .WithTypeConverter(portModelConverter)
            ;

        foreach (var type in schemeObjectTypes)
        {
            var name = type.Name;
            var tagName = $"!{(name.EndsWith("Model") ? name[..^5] : name)}";

            serializerBuilder.WithTagMapping(new TagName(tagName), type);
            deserializerBuilder.WithTagMapping(new TagName(tagName), type);

            _logger.LogInformation("{type} scheme type registered.", type.Name);
        }

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
        scheme.Name = Path.GetFileName(path);

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