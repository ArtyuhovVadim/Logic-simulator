using LogicSimulator.Shared.ExtensionMethods;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Version = System.Version;

namespace LogicSimulator.Infrastructure.YamlConverters;

public class VersionYamlConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(Version);

    public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        if (type != typeof(Version))
            throw new YamlException("Wrong type.");

        return Version.Parse(parser.ConsumeScalarOrThrow());
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
    {
        if (type != typeof(Version))
            throw new YamlException("Wrong type.");

        var version = (Version)value!;

        emitter.Emit(new Scalar(null, version.ToString()));
    }
}