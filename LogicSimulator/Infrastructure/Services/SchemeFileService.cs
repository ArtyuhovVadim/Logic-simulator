using System.IO;
using System.Text;
using LogicSimulator.Infrastructure.YamlConverters;
using LogicSimulator.Models;
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

        _serializer = new SerializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .Build();

        _deserializer = new DeserializerBuilder()
            .WithTypeConverter(vector2Converter)
            .WithTypeConverter(color4Converter)
            .Build();

        _fileWriteStreamOptions = new FileStreamOptions { Access = FileAccess.Write, Mode = FileMode.Create };
        _fileReadStreamOptions = new FileStreamOptions { Access = FileAccess.Read, Mode = FileMode.Open };
    }

    public void SaveToFile(string path, Scheme scheme)
    {
        using var streamWriter = new StreamWriter(path, Encoding.Default, _fileWriteStreamOptions);

        var serializedScheme = _serializer.Serialize(scheme);
        
        streamWriter.Write(serializedScheme);
    }

    public Scheme ReadFromFile(string path)
    {
        using var streamReader = new StreamReader(path, Encoding.Default, false, _fileReadStreamOptions);

        var serializedScheme = streamReader.ReadToEnd();

        return _deserializer.Deserialize<Scheme>(serializedScheme);
    }
}