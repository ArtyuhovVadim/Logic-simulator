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

    public async Task SaveToFileAsync(string path, Project project)
    {
        await using var streamWriter = new StreamWriter(path, Encoding.Default, _fileWriteStreamOptions);
        project.Version = App.Version;
        var serializedProject = _serializer.Serialize(project);
        await streamWriter.WriteAsync(serializedProject);
    }

    public async Task<Project> ReadFromFileAsync(string path)
    {
        using var streamReader = new StreamReader(path, Encoding.Default, false, _fileReadStreamOptions);
        var project = _deserializer.Deserialize<Project>(await streamReader.ReadToEndAsync());
        project.FileInfo = new FileInfo(path);

        if (project.Version > App.Version)
            throw new InvalidOperationException($"Can not load project of {project.Version} version.");

        return project;
    }
}