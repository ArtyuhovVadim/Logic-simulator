using LogicSimulator.Models;
using LogicSimulator.ViewModels.AnchorableViewModels;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface ISchemeViewModelFactory
{
    SchemeViewModel Create(Scheme schemeModel);
}