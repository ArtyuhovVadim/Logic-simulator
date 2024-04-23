using System.Globalization;
using LogicSimulator.Models;
using LogicSimulator.Utils;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace LogicSimulator.Infrastructure.YamlConverters;

public class PortModelYamlConverter : IYamlTypeConverter
{
    public bool Accepts(Type type) => type == typeof(PortModel);

    public object ReadYaml(IParser parser, Type type)
    {
        if (type != typeof(PortModel))
            throw new YamlException("Wrong type.");

        parser.Consume<MappingStart>();
        parser.ConsumeScalarOrThrow();
        var name = parser.ConsumeScalarOrThrow();
        parser.ConsumeScalarOrThrow();
        var length = parser.ConsumeScalarAsDoubleOrThrow();
        parser.Consume<MappingEnd>();

        return new PortModel { Name = name, Length = (float)length };
    }

    public void WriteYaml(IEmitter emitter, object? value, Type type)
    {
        if (type != typeof(PortModel))
            throw new YamlException("Wrong type.");

        var model = (PortModel)value!;

        emitter.Emit(new MappingStart());
        emitter.Emit(new Scalar(null, nameof(PortModel.Name)));
        emitter.Emit(new Scalar(null, model.Name));
        emitter.Emit(new Scalar(null, nameof(PortModel.Length)));
        emitter.Emit(new Scalar(null, model.Length.ToString(CultureInfo.InvariantCulture)));
        emitter.Emit(new MappingEnd());
    }
}