using System.IO;
using System.Text;
using LogicSimulator.Infrastructure.Services.Interfaces;
using LogicSimulator.Infrastructure.YamlConverters;
using LogicSimulator.Models;
using YamlDotNet.Serialization;

namespace LogicSimulator.Infrastructure.Services;

public class ProjectFileService : IProjectFileService
{
    private readonly ISerializer _serializer;
    private readonly IDeserializer _deserializer;

    private readonly FileStreamOptions _fileWriteStreamOptions;
    private readonly FileStreamOptions _fileReadStreamOptions;

    public ProjectFileService()
    {
        var versionConverter = new VersionYamlConverter();

        var serializerBuilder = new SerializerBuilder()
            .WithTypeConverter(versionConverter);

        var deserializerBuilder = new DeserializerBuilder()
            .WithTypeConverter(versionConverter);

        _serializer = serializerBuilder.Build();
        _deserializer = deserializerBuilder.Build();

        _fileWriteStreamOptions = new FileStreamOptions { Access = FileAccess.Write, Mode = FileMode.Create };
        _fileReadStreamOptions = new FileStreamOptions { Access = FileAccess.Read, Mode = FileMode.Open };
    }

    public bool SaveToFile(string path, Project project)
    {
        try
        {
            using var streamWriter = new StreamWriter(path, Encoding.Default, _fileWriteStreamOptions);

            project.Version = App.Version;
            var serializedProject = _serializer.Serialize(project);
            streamWriter.Write(serializedProject);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool ReadFromFile(string path, out Project? project)
    {
        project = null;

        if (!File.Exists(path) || Path.GetExtension(path) != Project.Extension)
            return false;

        try
        {
            using var streamReader = new StreamReader(path, Encoding.Default, false, _fileReadStreamOptions);

            project = _deserializer.Deserialize<Project>(streamReader);
            project.FileInfo = new FileInfo(path);

            if (project.Version > App.Version)
                throw new InvalidOperationException($"Can not load project of {project.Version} version.");

            return true;
        }
        catch
        {
            return false;
        }
    }
}