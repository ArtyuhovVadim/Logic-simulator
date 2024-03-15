using LogicSimulator.Models;
using LogicSimulator.ViewModels.AnchorableViewModels;

namespace LogicSimulator.Infrastructure.Factories.Interfaces;

public interface ISchemeViewModelFactory
{
    SchemeViewModel Create(Scheme schemeModel);
}