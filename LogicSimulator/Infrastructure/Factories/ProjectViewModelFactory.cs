using LogicSimulator.Infrastructure.Factories.Interfaces;
using LogicSimulator.Models;
using LogicSimulator.ViewModels;

namespace LogicSimulator.Infrastructure.Factories;

public class ProjectViewModelFactory : IProjectViewModelFactory
{
    private readonly ISchemeViewModelFactory _schemeFactory;

    public ProjectViewModelFactory(ISchemeViewModelFactory schemeFactory)
    {
        _schemeFactory = schemeFactory;
    }

    public ProjectViewModel Create(Project project) => new(project, _schemeFactory);
}