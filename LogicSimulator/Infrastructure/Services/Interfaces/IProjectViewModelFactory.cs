using LogicSimulator.Models;
using LogicSimulator.ViewModels;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IProjectViewModelFactory
{
    ProjectViewModel Create(Project project);
}