using LogicSimulator.Models.Logic;
using LogicSimulator.Models.Objects.Base;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface ISchemePreprocessorService
{
    IPreprocessedLogicScheme Process(IReadOnlyList<BaseObjectModel> objects);
}