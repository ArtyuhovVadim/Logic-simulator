using System.IO;
using System.Text;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Models;
using YamlDotNet.Serialization;

namespace LogicSimulator.Infrastructure.Services;

public interface IProjectFileService
{
    public bool ReadFromFile(string path, out Project project);
}

public class ProjectFileService : IProjectFileService
{
    private readonly ISchemeFileService _schemeFileService;

    private readonly ISerializer _serializer;
    private readonly IDeserializer _deserializer;

    private readonly FileStreamOptions _fileWriteStreamOptions;
    private readonly FileStreamOptions _fileReadStreamOptions;

    public ProjectFileService(ISchemeFileService schemeFileService)
    {
        _schemeFileService = schemeFileService;

        var serializerBuilder = new SerializerBuilder()
            ;

        var deserializerBuilder = new DeserializerBuilder()
            ;

        _serializer = serializerBuilder.Build();
        _deserializer = deserializerBuilder.Build();

        _fileWriteStreamOptions = new FileStreamOptions { Access = FileAccess.Write, Mode = FileMode.Create };
        _fileReadStreamOptions = new FileStreamOptions { Access = FileAccess.Read, Mode = FileMode.Open };
    }

    public bool ReadFromFile(string path, out Project project)
    {
        project = null;

        if (!File.Exists(path) || Path.GetExtension(path) != Project.Extension)
        {
            return false;
        }

        try
        {
            using var streamReader = new StreamReader(path, Encoding.Default, false, _fileReadStreamOptions);

            project = _deserializer.Deserialize<Project>(streamReader);

            var projectDirectoryPath = Path.GetDirectoryName(path)!;

            var schemeFilePaths = Directory.GetFiles(projectDirectoryPath, $"*{Scheme.Extension}");

            var schemes = new List<Scheme>();

            foreach (var schemeFilePath in schemeFilePaths)
            {
                _schemeFileService.ReadFromFile(schemeFilePath, out var scheme);
                schemes.Add(scheme);
            }

            project.Schemes = schemes;

            return true;
        }
        catch
        {
            return false;
        }
    }
}