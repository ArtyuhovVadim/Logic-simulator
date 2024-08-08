using LogicSimulator.Models;
using LogicSimulator.ViewModels.Anchorable;

namespace LogicSimulator.Infrastructure.Factories.Interfaces;

public interface ISchemeViewModelFactory
{
    SchemeViewModel Create(Scheme schemeModel);
}