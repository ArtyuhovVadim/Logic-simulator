using LogicSimulator.Models;
using LogicSimulator.ViewModels;

namespace LogicSimulator.Infrastructure.Factories.Interfaces;

public interface IProjectViewModelFactory
{
    ProjectViewModel Create(Project project);
}