using LogicSimulator.Models;

namespace LogicSimulator.Infrastructure.Services.Interfaces;

public interface IProjectFileService
{
    Task SaveToFileAsync(string path, Project project);

    Task<Project> ReadFromFileAsync(string path);
}